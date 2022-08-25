using Platform.Local;

namespace LSTY.Sdtd.PatronsMod.Extensions
{
    internal static class ClientInfoExtension
    {
        public static ClientInfo GetCmdExecuteDelegate()
        {
            return new ClientInfo() { PlatformId = new UserIdentifierLocal(ModApi.ModIdentity) };
        }

        public static PlayerBase ToPlayerBase(this ClientInfo clientInfo)
        {
            return new PlayerBase()
            {
                EntityId = clientInfo.entityId,
                CrossplatformId = clientInfo.CrossplatformId.CombinedString,
                Name = clientInfo.playerName,
                PlatformType = clientInfo.PlatformId.PlatformIdentifierString,
                PlatformId = clientInfo.PlatformId.CombinedString,
            };
        }

        public static PlayerSpawned ToPlayerSpawned(this ClientInfo clientInfo, RespawnType respawnType, Vector3i position)
        {
            return new PlayerSpawned()
            {
                EntityId = clientInfo.entityId,
                CrossplatformId = clientInfo.CrossplatformId.CombinedString,
                Name = clientInfo.playerName,
                PlatformType = clientInfo.PlatformId.PlatformIdentifierString,
                PlatformId = clientInfo.PlatformId.CombinedString,
                RespawnType = (Shared.Models.RespawnType)respawnType,
                Position = position.ToPosition()
            };
        }
    }
}