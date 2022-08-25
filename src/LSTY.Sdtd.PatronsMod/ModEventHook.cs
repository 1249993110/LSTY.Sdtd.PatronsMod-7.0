using LSTY.Sdtd.PatronsMod.Extensions;
using Microsoft.AspNet.SignalR;

namespace LSTY.Sdtd.PatronsMod
{
    internal static class ModEventHook
    {
        private static IModEventHub _hub;

        static ModEventHook()
        {
            try
            {
                _hub = GlobalHost.ConnectionManager.GetHubContext<ModEventHub, IModEventHub>().Clients.All;
            }
            catch (Exception ex)
            {
                CustomLogger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Runs each time a chunk has it's colors re-calculated. For example this is used to generate the images for allocs game map mod
        /// </summary>
        /// <param name="_chunk"></param>
        public static void CalcChunkColorsDone(Chunk _chunk)
        {
        }

        /// <summary>
        /// <para>Return true to pass the message on to the next mod, or if no other mods then it will output to chat. </para>
        /// <para>Return false to prevent the message from being passed on or output to chat</para>
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="eChatType"></param>
        /// <param name="senderId"></param>
        /// <param name="message"></param>
        /// <param name="mainName"></param>
        /// <param name="localizeMain"></param>
        /// <param name="recipientEntityIds"></param>
        /// <returns></returns>
        public static bool ChatMessage(ClientInfo clientInfo, EChatType eChatType, int senderId, string message,
            string mainName, bool localizeMain, List<int> recipientEntityIds)
        {
            ChatMessage chatMessage = new ChatMessage()
            {
                ChatType = (ChatType)eChatType,
                EntityId = senderId,
                Message = message,
                SenderName = clientInfo?.playerName ?? (localizeMain ? Localization.Get(mainName) : mainName),
            };

            Task.Factory.StartNew((state) =>
            {
                _hub.OnChatMessage((ChatMessage)state);
            }, chatMessage);

            return true;
        }

        /// <summary>
        /// Runs when entity has been killed
        /// </summary>
        /// <param name="killedEntity"></param>
        /// <param name="entityThatKilledMe"></param>
        public static void EntityKilled(Entity killedEntity, Entity entityThatKilledMe)
        {
            if (killedEntity != null
               && entityThatKilledMe != null
               && entityThatKilledMe is EntityPlayer entityPlayer
               && entityThatKilledMe.IsClientControlled())
            {
                int entityIdThatKilledMe = ConnectionManager.Instance.Clients.ForEntityId(entityPlayer.entityId).entityId;

                EntityLocation entity = null;

                if (killedEntity is EntityPlayer diedPlayer && killedEntity.IsClientControlled())
                {
                    entity = new EntityLocation()
                    {
                        EntityId = diedPlayer.entityId,
                        EntityName = diedPlayer.EntityName,
                        Position = diedPlayer.position.ToPosition(),
                        IsPlayer = true
                    };
                }
                else if (killedEntity is EntityAlive diedEntity && killedEntity.IsClientControlled() == false)
                {
                    entity = new EntityLocation()
                    {
                        EntityId = diedEntity.entityId,
                        EntityName = diedEntity.EntityName,
                        Position = diedEntity.position.ToPosition(),
                        IsPlayer = false
                    };
                }
                else
                {
                    return;
                }

                Task.Factory.StartNew((state) =>
                {
                    _hub.OnEntityKilled((EntityLocation)state, entityIdThatKilledMe);
                }, entity);
            }
        }

        /// <summary>
        /// Runs when entity spawned
        /// </summary>
        /// <param name="entity"></param>
        public static void EntitySpawned(EntityLocation entity)
        {
            Task.Factory.StartNew((state) =>
            {
                if (state is EntityAlive entityAlive)
                {
                    _hub.OnEntitySpawned(new EntityLocation()
                    {
                        EntityId = entityAlive.entityId,
                        EntityName = entityAlive.EntityName,
                        Position = entityAlive.position.ToPosition(),
                        IsPlayer = entityAlive is EntityPlayer
                    });
                }
            }, entity);
        }

        /// <summary>
        /// Runs once when the server is ready for interaction and GameManager.Instance.World is set
        /// </summary>
        public static void GameAwake()
        {
            Task.Run(_hub.OnGameAwake);
        }

        /// <summary>
        /// Runs once when the server is about to shut down
        /// </summary>
        public static void GameShutdown()
        {
            Task.Run(_hub.OnGameShutdown);
        }

        /// <summary>
        /// Runs once when the server is ready for players to join
        /// </summary>
        public static void GameStartDone()
        {
            Task.Run(_hub.OnGameStartDone);
        }

        /// <summary>
        /// <para>Runs any time the game executes an update (which is very often).</para>
        /// <para>Place any tasks that you want to process in the main thread here with an execution rate limiter (such as creating entities via the entity factory)</para>
        /// </summary>
        public static void GameUpdate()
        {
        }

        /// <summary>
        /// Runs when LogCallback has be called
        /// </summary>
        /// <param name="message"></param>
        /// <param name="trace"></param>
        /// <param name="type"></param>
        public static void LogCallback(string message, string trace, UnityEngine.LogType type)
        {
            var logEntry = new LogEntry()
            {
                Message = message,
                LogLevel = (Shared.Models.LogLevel)type,
            };
            Task.Factory.StartNew((state) =>
            {
                _hub.OnLogCallback((LogEntry)state);
            }, logEntry);
        }
        /// <summary>
        /// Runs on each player disconnect
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="shutdown"></param>
        public static void PlayerDisconnected(ClientInfo clientInfo, bool shutdown)
        {
            Task.Factory.StartNew((state) =>
            {
                _hub.OnPlayerDisconnected((int)state);
            }, clientInfo.entityId);
        }

        /// <summary>
        /// Runs on initial connection from a player, _cInfo is usually null at this point
        /// </summary>
        /// <param name="_cInfo"></param>
        /// <param name="_compatibilityVersion"></param>
        public static void PlayerLogin(ClientInfo _cInfo, string _compatibilityVersion)
        {
        }

        /// <summary>
        /// Runs each time a player spawns, including on login, respawn from death, and teleport
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="respawnType"></param>
        /// <param name="position"></param>
        public static void PlayerSpawnedInWorld(ClientInfo clientInfo, RespawnType respawnType, Vector3i position)
        {
            var obj = clientInfo.ToPlayerSpawned(respawnType, position);

            Task.Factory.StartNew((state) => _hub.OnPlayerSpawnedInWorld((PlayerSpawned)state), obj);
        }
        /// <summary>
        /// Runs just before a player is spawned int the world
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="chunkViewDim"></param>
        /// <param name="playerProfile"></param>
        public static void PlayerSpawning(ClientInfo clientInfo, int chunkViewDim, PlayerProfile playerProfile)
        {
            Task.Factory.StartNew((state) =>
            {
                _hub.OnPlayerSpawning(((ClientInfo)state).ToPlayerBase());
            }, clientInfo);
        }

        /// <summary>
        /// <para>runs each time a player file is saved from the client to the server</para>
        /// <para>this will usually run about every 30 seconds per player as well as triggered updates such as dying</para>
        /// </summary>
        /// <param name="clientInfo"></param>
        /// <param name="pdf"></param>
        public static void SavePlayerData(ClientInfo clientInfo, PlayerDataFile pdf)
        {
            Task.Factory.StartNew((state) =>
            {
                _hub.OnSavePlayerData(((ClientInfo)state).ToPlayerBase());
            }, clientInfo);
        }
    }
}