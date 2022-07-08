using LSTY.Sdtd.PatronsMod.Extensions;
using LSTY.Sdtd.Shared.Hubs;
using LSTY.Sdtd.Shared.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Text;

namespace LSTY.Sdtd.PatronsMod
{
    [HubName(nameof(IServerManageHub))]
    public class ServerManageHub : Hub, IServerManageHub
    {
        public async Task<IEnumerable<string>> ExecuteConsoleCommand(string command, bool inMainThread = false)
        {
            if (inMainThread == false)
            {
                return await Task.Factory.StartNew((state) =>
                {
                    return SdtdConsole.Instance.ExecuteSync((string)state, ClientInfoExtension.GetCmdExecuteDelegate());
                }, command);
            }
            else
            {
                return await Task.Factory.StartNew((state) =>
                {
                    List<string> executeResult = null;
                    ModApi.MainThreadSyncContext.Send((state1) =>
                    {
                        executeResult = SdtdConsole.Instance.ExecuteSync((string)state1, ClientInfoExtension.GetCmdExecuteDelegate());
                    }, state);

                    return executeResult;
                }, command);
            }
        }

        public async Task<IEnumerable<AllowedCommand>> GetAllowedCommands()
        {
            return await Task.Run(() =>
            {
                var result = new List<AllowedCommand>();
                foreach (var consoleCommand in SdtdConsole.Instance.GetCommands())
                {
                    var commands = consoleCommand.GetCommands();
                    int commandPermissionLevel = GameManager.Instance.adminTools.GetCommandPermissionLevel(commands);

                    result.Add(new AllowedCommand()
                    {
                        Commands = commands,
                        PermissionLevel = commandPermissionLevel,
                        Description = consoleCommand.GetDescription(),
                        Help = consoleCommand.GetHelp(),
                    });
                }

                return result;
            });
        }

        public async Task<IEnumerable<AllowSpawnedEntity>> GetAllowSpawnedEntities()
        {
            return await Task.Run(() =>
            {
                var result = new List<AllowSpawnedEntity>();
                int num = 1;
                foreach (var item in EntityClass.list.Dict.Values)
                {
                    if (item.bAllowUserInstantiate)
                    {
                        result.Add(new AllowSpawnedEntity()
                        {
                            Id = num,
                            Name = item.entityClassName
                        });

                        ++num;
                    }
                }

                return result;
            });
        }

        public async Task<IEnumerable<EntityLocation>> GetAnimalsLocation()
        {
            if(ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var entityLocations = new List<EntityLocation>();
                foreach (var entity in GameManager.Instance.World.Entities.list)
                {
                    if (entity is EntityAnimal entityAnimal && entity.IsAlive())
                    {
                        entityLocations.Add(new EntityLocation()
                        {
                            EntityId = entityAnimal.entityId,
                            EntityName = entityAnimal.EntityName ?? ("animal class #" + entityAnimal.entityClass),
                            Position = entityAnimal.GetPosition().ToPosition(),
                            IsPlayer = false
                        });
                    }
                }

                return entityLocations;
            });
        }

        public async Task<byte[]> GetFullMap()
        {
            return await Task.Run(() =>
            {
                string fileName = Path.Combine(AllocsCaller.MapDirectory, "map.png");

                if (File.Exists(fileName))
                {
                    return File.ReadAllBytes(fileName);
                }

                return null;
            });
        }

        public async Task<Shared.Models.GameStats> GetGameStats()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var worldTime = GameManager.Instance.World.worldTime;
                var entityList = GameManager.Instance.World.Entities.list;

                int hostiles = 0;
                int animals = 0;
                foreach (var entity in entityList)
                {
                    if (entity.IsAlive())
                    {
                        if (entity is EntityEnemy)
                        {
                            ++hostiles;
                        }
                        else if (entity is EntityAnimal)
                        {
                            ++animals;
                        }
                    }
                }

                int onlinePlayers = GameManager.Instance.World.Players.Count;
                var persistentPlayers = GameManager.Instance.GetPersistentPlayerList()?.Players;
                int offlinePlayers = (persistentPlayers == null || persistentPlayers.Count == 0) ? 0 : persistentPlayers.Count - onlinePlayers;
                return new Shared.Models.GameStats()
                {
                    GameTime = new GameTime()
                    {
                        Days = GameUtils.WorldTimeToDays(worldTime),
                        Hours = GameUtils.WorldTimeToHours(worldTime),
                        Minutes = GameUtils.WorldTimeToMinutes(worldTime),
                    },
                    OnlinePlayers = onlinePlayers,
                    OfflinePlayers = offlinePlayers,
                    Hostiles = hostiles,
                    Animals = animals,
                };
            });
        }

        public async Task<IEnumerable<EntityLocation>> GetHostilesLocation()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var entityLocations = new List<EntityLocation>();
                foreach (var entity in GameManager.Instance.World.Entities.list)
                {
                    if (entity is EntityEnemy entityEnemy && entity.IsAlive())
                    {
                        entityLocations.Add(new EntityLocation()
                        {
                            EntityId = entityEnemy.entityId,
                            EntityName = entityEnemy.EntityName ?? ("enemy class #" + entityEnemy.entityClass),
                            Position = entityEnemy.GetPosition().ToPosition(),
                            IsPlayer = false
                        });
                    }
                }

                return entityLocations;
            });
        }

        public async Task<ItemBlockPaged> GetItemBlocks(ItemBlockQuery itemBlockQuery)
        {
            return await Task.Factory.StartNew((state) =>
            {
                var param = (ItemBlockQuery)state;
                int pageSzie = param.PageSzie;
                List<ItemBlock> itemBlocks = null;

                switch (param.ItemBlockKind)
                {
                    case ItemBlockKind.All:
                        itemBlocks = ItemsHelper.GetAllItemsAndBlocks(param.Language, param.Keyword, param.ShowUserHidden);
                        break;
                    case ItemBlockKind.Item:
                        itemBlocks = ItemsHelper.GetAllItems(param.Language, param.Keyword, param.ShowUserHidden);
                        break;
                    case ItemBlockKind.Block:
                        itemBlocks = ItemsHelper.GetAllBlocks(param.Language, param.Keyword, param.ShowUserHidden);
                        break;
                }
                
                var items = pageSzie == -1 ? itemBlocks : itemBlocks.Skip((param.PageIndex - 1) * pageSzie).Take(pageSzie);

                var result = new ItemBlockPaged()
                {
                    Items = items,
                    Total = itemBlocks.Count
                };

                return result;
            }, itemBlockQuery);
        }

        public async Task<byte[]> GetItemIcon(string iconName)
        {
            return await Task.Factory.StartNew((state) =>
            {
                string iconPath = FindIconPath((string)state);
                if (iconPath == null)
                {
                    return null;
                }
                else
                {
                    return File.ReadAllBytes(iconPath);
                }
            }, iconName);
        }

        public async Task<IEnumerable<string>> GetKnownLanguages()
        {
            return await Task.Run(() =>
            {
                return Localization.dictionary["KEY"];
            });
        }

        public async Task<ClaimOwner> GetLandClaim(string playerId)
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Factory.StartNew((state) =>
            {
                var _playerId = (string)state;
                var claims = new List<Position>();
                var persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
                 var claimOwner = new ClaimOwner()
                {
                    ClaimPositions = claims
                };

                foreach (var item in persistentPlayerList.m_lpBlockMap)
                {
                    if (_playerId == item.Value.UserIdentifier.CombinedString)
                    {
                        var persistentPlayerData = item.Value;
                        claims.Add(item.Key.ToPosition());
                        claimOwner.PlatformId = persistentPlayerData.UserIdentifier.CombinedString;
                        claimOwner.PlayerName = persistentPlayerData.PlayerName;
                        bool claimActive = GameManager.Instance.World.IsLandProtectionValidForPlayer(persistentPlayerList.GetPlayerData(persistentPlayerData.UserIdentifier));
                        claimOwner.ClaimActive = claimActive;
                    }
                }

                return claimOwner;
            }, playerId);
        }

        public async Task<LandClaims> GetLandClaims()
        {
            if (ModApi.IsGameStartDone == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                int claimsize = GamePrefs.GetInt(EnumGamePrefs.LandClaimSize);
                var claimOwners = new Dictionary<int, ClaimOwner>();

                var persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
                if (persistentPlayerList == null)
                {
                    goto _Return;
                }

                var lpBlockMap = persistentPlayerList.m_lpBlockMap;
                if (lpBlockMap == null || lpBlockMap.Count == 0)
                {
                    goto _Return;
                }

                foreach (var item in lpBlockMap)
                {
                    var persistentPlayerData = item.Value;
                    int entityId = persistentPlayerData.EntityId;
                    if (claimOwners.ContainsKey(entityId))
                    {
                        ((List<Position>)claimOwners[entityId].ClaimPositions).Add(item.Key.ToPosition());
                    }
                    else
                    {
                        bool claimActive = GameManager.Instance.World.IsLandProtectionValidForPlayer(persistentPlayerList.GetPlayerData(persistentPlayerData.UserIdentifier));
                        claimOwners.TryAdd(entityId, new ClaimOwner()
                        {
                            ClaimActive = claimActive,
                            PlatformId = persistentPlayerData.UserIdentifier.CombinedString,
                            PlayerName = persistentPlayerData.PlayerName,
                            ClaimPositions = new List<Position>() { item.Key.ToPosition() }
                        });
                    }
                }
                _Return:
                return new LandClaims()
                {
                    ClaimOwners = claimOwners.Values,
                    ClaimSize = claimsize
                };
            });
        }

        public async Task<IDictionary<string, string>> GetLocalization(string language = "schinese")
        {
            return await Task.Factory.StartNew((state) =>
            {
                string language = (string)state;
                var dict = Localization.dictionary;
                int languageIndex = Array.LastIndexOf(dict["KEY"], language);

                if (languageIndex < 0)
                {
                    throw new Exception($"The specified language: {language} does not exist");
                }

                return dict.ToDictionary(p => p.Key, p => p.Value[languageIndex]);
            }, language);
        }

        public async Task<string> GetLocalization(string itemName, string language = "schinese")
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(itemName))
                {
                    throw new ArgumentNullException(nameof(itemName));
                }

                var dict = Localization.dictionary;
                int languageIndex = Array.LastIndexOf(dict["KEY"], language);

                if (languageIndex < 0)
                {
                    throw new Exception($"The specified language: {language} does not exist");
                }

                if (dict.ContainsKey(itemName) == false)
                {
                    throw new Exception($"The specified itemName: {itemName} does not exist");
                }

                return dict[itemName][languageIndex];
            });
        }

        public async Task<MapInfo> GetMapInfo()
        {
            return await Task.Run(() =>
            {
                string fileName = Path.Combine(AllocsCaller.MapDirectory, "mapinfo.json");

                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName, Encoding.UTF8);
                    return JsonConvert.DeserializeObject<MapInfo>(json);
                }

                return new MapInfo();
            });
        }

        public async Task<byte[]> GetMapTile(string zoomLevel)
        {
            return await Task.Factory.StartNew((state) =>
            {
                string fileName = AllocsCaller.MapDirectory + (string)state;
                return AllocsCaller.GetMapTileDelegate.Invoke(fileName);
            }, zoomLevel);
        }

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

        public async Task<IEnumerable<string>> GiveItem(GiveItem giveItemEntry)
        {
            return await ExecuteConsoleCommand($"ty-give {giveItemEntry.ItemName} {giveItemEntry.Count ?? 1} {giveItemEntry.Quality ?? 1} {giveItemEntry.Durability ?? 100}");
        }

        public Task RenderFullMap()
        {
            ModApi.MainThreadSyncContext.Post((state) =>
            {
                AllocsCaller.RenderFullMap();
            }, null);

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<string>> RestartServer(bool force = false)
        {
            string cmd = "ty-rs";
            if (force)
            {
                cmd += " -f";
            }

            return await ExecuteConsoleCommand(cmd);
        }

        public async Task<IEnumerable<string>> SendGlobalMessage(GlobalMessage globalMessage)
        {
            return await ExecuteConsoleCommand($"ty-say {FormatCommandArgs(globalMessage.Message)} {FormatCommandArgs(globalMessage.SenderName) ?? Shared.Constants.DefaultServerName}");
        }

        public async Task<IEnumerable<string>> SendPrivateMessage(PrivateMessage privateMessage)
        {
            return await ExecuteConsoleCommand($"ty-pm {FormatCommandArgs(privateMessage.TargetPlayerIdOrName)} {FormatCommandArgs(privateMessage.Message)} {FormatCommandArgs(privateMessage.SenderName) ?? Shared.Constants.DefaultServerName}");
        }

        public async Task<IEnumerable<string>> SpawnEntity(SpawnEntity spawnEntityEntry)
        {
            return await ExecuteConsoleCommand($"se {FormatCommandArgs(spawnEntityEntry.TargetPlayerIdOrName)} {FormatCommandArgs(spawnEntityEntry.TargetPlayerIdOrName)}");
        }

        public async Task<IEnumerable<string>> TeleportPlayer(Teleport teleportEntry)
        {
            string target = teleportEntry.TargetPlayerIdOrNameOrPosition;
            if (target.Split(' ').Length != 3)
            {
                target = FormatCommandArgs(teleportEntry.TargetPlayerIdOrNameOrPosition);
            }

            return await ExecuteConsoleCommand($"tele {FormatCommandArgs(teleportEntry.OriginPlayerIdOrName)} {target}");
        }

        private static async Task<IEnumerable<string>> ExecuteConsoleCommandBatch<TObject>(IEnumerable<TObject> objects, Func<TObject, string> getCommand)
        {
            if (objects?.Any() == false)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            return await Task.Factory.StartNew((state) =>
            {
                var executeResult = new List<string>();
                var executeDelegate = ClientInfoExtension.GetCmdExecuteDelegate();
                foreach (TObject item in (IEnumerable<TObject>)state)
                {
                    string command = getCommand(item);
                    var resultEntry = SdtdConsole.Instance.ExecuteSync(command, executeDelegate);
                    executeResult.AddRange(resultEntry);
                }

                return executeResult;
            }, objects);
        }

        private static string FindIconPath(string iconName)
        {
            string path = "Data/ItemIcons/" + iconName;
            if (File.Exists(path))
            {
                return path;
            }

            foreach (Mod mod in ModManager.GetLoadedMods())
            {
                path = Path.Combine(mod.Path, "ItemIcons", iconName);
                if (File.Exists(path))
                {
                    return path;
                }

                foreach (string dir in Directory.GetDirectories(mod.Path))
                {
                    path = Path.Combine(dir, iconName);
                    if (File.Exists(path))
                    {
                        return path;
                    }

                    foreach (string subDir in Directory.GetDirectories(dir))
                    {
                        path = Path.Combine(subDir, iconName);
                        if (File.Exists(path))
                        {
                            return path;
                        }
                    }
                }
            }

            return null;
        }

        private static string FormatCommandArgs(string args)
        {
            if (args.Contains('\"'))
            {
                throw new Exception("参数不应该包含字符: '\"'");
            }

            if (args.Contains(' '))
            {
                return string.Concat("\"", args, "\"");
            }

            return args;
        }

        #region Admins

        public async Task<IEnumerable<string>> AddAdmins(IEnumerable<AdminEntry> admins)
        {
            return await ExecuteConsoleCommandBatch(admins, obj => $"admin add {obj.PlayerId} {obj.PermissionLevel} {FormatCommandArgs(obj.DisplayName)}");
        }

        public async Task<IEnumerable<AdminEntry>> GetAdmins()
        {
            return await Task.Run(() =>
            {
                var result = new List<AdminEntry>();

                foreach (var item in GameManager.Instance.adminTools.GetAdmins().Values)
                {
                    result.Add(new AdminEntry()
                    {
                        PlayerId = item.UserIdentifier.CombinedString,
                        PermissionLevel = item.PermissionLevel,
                        DisplayName = item.Name,
                    });
                }

                return result;
            });
        }

        public async Task<IEnumerable<string>> RemoveAdmins(IEnumerable<string> playerId)
        {
            return await ExecuteConsoleCommandBatch(playerId, obj => $"admin remove {obj}");
        }

        #endregion Admins

        #region Permissions

        public async Task<IEnumerable<string>> AddPermissions(IEnumerable<PermissionEntry> permissions)
        {
            return await ExecuteConsoleCommandBatch(permissions, obj => $"cp add {obj.Command} {obj.Level}");
        }

        public async Task<IEnumerable<PermissionEntry>> GetPermissions()
        {
            return await Task.Run(() =>
            {
                var result = new List<PermissionEntry>();

                foreach (var item in GameManager.Instance.adminTools.GetCommands().Values)
                {
                    result.Add(new PermissionEntry()
                    {
                        Command = item.Command,
                        Level = item.PermissionLevel
                    });
                }

                return result;
            });
        }

        public async Task<IEnumerable<string>> RemovePermissions(IEnumerable<string> playerId)
        {
            return await ExecuteConsoleCommandBatch(playerId, obj => $"cp remove {obj}");
        }

        #endregion Permissions

        #region Whitelist

        public async Task<IEnumerable<string>> AddWhitelist(IEnumerable<WhitelistEntry> whitelist)
        {
            return await ExecuteConsoleCommandBatch(whitelist, obj => $"whitelist add {obj.PlayerId} {FormatCommandArgs(obj.DisplayName)}");
        }

        public async Task<IEnumerable<WhitelistEntry>> GetWhitelist()
        {
            return await Task.Run(() =>
            {
                var result = new List<WhitelistEntry>();

                foreach (var item in GameManager.Instance.adminTools.GetWhitelistedUsers().Values)
                {
                    result.Add(new WhitelistEntry()
                    {
                        PlayerId = item.UserIdentifier.CombinedString,
                        DisplayName = item.Name
                    });
                }

                return result;
            });
        }

        public async Task<IEnumerable<string>> RemoveWhitelist(IEnumerable<string> playerId)
        {
            return await ExecuteConsoleCommandBatch(playerId, obj => $"whitelist remove {obj}");
        }

        #endregion Whitelist

        #region Blacklist

        public async Task<IEnumerable<string>> AddBlacklist(IEnumerable<BlacklistEntry> blacklist)
        {
            return await ExecuteConsoleCommandBatch(blacklist, obj =>
            {
                return $"ban add {obj.PlayerId} {(int)(DateTime.Now - obj.BannedUntil).TotalMinutes} minutes {FormatCommandArgs(obj.Reason)} {FormatCommandArgs(obj.DisplayName)}";
            });
        }

        public async Task<IEnumerable<BlacklistEntry>> GetBlacklist()
        {
            return await Task.Run(() =>
            {
                var result = new List<BlacklistEntry>();

                foreach (var item in GameManager.Instance.adminTools.GetBanned())
                {
                    result.Add(new BlacklistEntry()
                    {
                        PlayerId = item.UserIdentifier.CombinedString,
                        BannedUntil = item.BannedUntil,
                        Reason = item.BanReason,
                        DisplayName = item.Name
                    });
                }

                return result;
            });
        }

        public async Task<IEnumerable<string>> RemoveBlacklist(IEnumerable<string> playerId)
        {
            return await ExecuteConsoleCommandBatch(playerId, obj => $"ban remove {obj}");
        }

        public async Task<PlayerBase> GetPlayerByIdOrName(string idOrName)
        {
            return await Task.Factory.StartNew((state) => ConsoleHelper.ParseParamIdOrName((string)state)?.ToPlayerBase(), idOrName);
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


        #endregion Blacklist
    }
}