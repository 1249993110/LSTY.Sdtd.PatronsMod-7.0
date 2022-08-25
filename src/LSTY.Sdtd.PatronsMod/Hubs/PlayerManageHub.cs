using LSTY.Sdtd.PatronsMod.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace LSTY.Sdtd.PatronsMod
{
    [HubName(nameof(IPlayerManageHub))]
    public class PlayerManageHub : Hub, IPlayerManageHub
    {
        public async Task<OnlinePlayer> GetPlayer(int entityId)
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Factory.StartNew((state) =>
            {
                if (GameManager.Instance.World.Players.dict.TryGetValue((int)state, out var player))
                {
                    return player.ToOnlinePlayer();
                }

                return null;
            }, entityId);
        }

        public async Task<PlayerBase> GetPlayerByIdOrName(string idOrName)
        {
            return await Task.Factory.StartNew((state) => ConsoleHelper.ParseParamIdOrName((string)state)?.ToPlayerBase(), idOrName);
        }

        public async Task<int> GetPlayerCount()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return 0;
            }

            return await Task.FromResult(GameManager.Instance.World.Players.Count);
        }

        public async Task<Shared.Models.Inventory> GetPlayerInventory(int entityId)
        {
            return await Task.Factory.StartNew((state) =>
            {
                return ConnectionManager.Instance.Clients.ForEntityId((int)state)?.latestPlayerData.GetInventory();
            }, entityId);
        }

        public async Task<Position> GetPlayerPosition(int entityId)
        {
            return await Task.Run(() =>
            {
                var players = GameManager.Instance.World.Players.dict;
                if (players.TryGetValue(entityId, out var entityPlayer) == false)
                {
                    return null;
                }

                return entityPlayer.position.ToPosition();
            });
        }

        public async Task<IEnumerable<OnlinePlayer>> GetPlayers()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var result = new List<OnlinePlayer>();
                foreach (var player in GameManager.Instance.World.Players.dict.Values)
                {
                    var onlinePlayer = player.ToOnlinePlayer();
                    if (onlinePlayer != null)
                    {
                        result.Add(onlinePlayer);
                    }
                }

                return result;
            });
        }

        public async Task<IEnumerable<PlayerInventory>> GetPlayersInventory()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var result = new List<PlayerInventory>();
                foreach (var player in GameManager.Instance.World.Players.dict.Values)
                {
                    var inventory = ConnectionManager.Instance.Clients.ForEntityId(player.entityId)?.latestPlayerData.GetInventory();

                    if (inventory != null)
                    {
                        result.Add(new PlayerInventory()
                        {
                            EntityId = player.entityId,
                            Inventory = inventory
                        });
                    }
                }

                return result;
            });
        }

        public async Task<IEnumerable<EntityLocation>> GetPlayersLocation()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var entityLocations = new List<EntityLocation>();
                foreach (var players in GameManager.Instance.World.Players.dict.Values)
                {
                    entityLocations.Add(new EntityLocation()
                    {
                        EntityId = players.entityId,
                        EntityName = players.EntityName,
                        Position = players.GetPosition().ToPosition(),
                        IsPlayer = true
                    });
                }

                return entityLocations;
            });
        }
        public async Task<bool> IsFriend(int entityId, int anotherEntityId)
        {
            return await Task.Run(() =>
            {
                var players = GameManager.Instance.World.Players.dict;
                if (players.TryGetValue(entityId, out var entityPlayer) == false)
                {
                    return false;
                }

                if (players.TryGetValue(anotherEntityId, out var anotherEntityPlayer) == false)
                {
                    return false;
                }

                return entityPlayer.IsFriendsWith(anotherEntityPlayer);
            });
        }
    }
}