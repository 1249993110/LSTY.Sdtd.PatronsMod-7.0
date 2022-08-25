using LSTY.Sdtd.Shared.Models;

namespace LSTY.Sdtd.Shared.Hubs
{
    /// <summary>
    /// 玩家管理中心
    /// </summary>
    public interface IPlayerManageHub
    {
        /// <summary>
        /// 获取指定实体Id的玩家
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns></returns>
        Task<OnlinePlayer> GetPlayer(int entityId);

        /// <summary>
        /// 通过玩家Id、实体Id或名称获取玩家
        /// </summary>
        /// <param name="idOrName">玩家Id或名称</param>
        /// <returns></returns>
        Task<PlayerBase> GetPlayerByIdOrName(string idOrName);

        /// <summary>
        /// 获取玩家数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetPlayerCount();

        /// <summary>
        /// 获取指定玩家的库存
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns></returns>
        Task<Inventory> GetPlayerInventory(int entityId);

        /// <summary>
        /// 获取所有玩家
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<OnlinePlayer>> GetPlayers();

        /// <summary>
        /// 获取所有玩家库存
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PlayerInventory>> GetPlayersInventory();

        /// <summary>
        /// 获取玩家位置
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EntityLocation>> GetPlayersLocation();

        /// <summary>
        /// 判断两个玩家是否为好友
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <param name="anotherEntityId">另一个玩家实体Id</param>
        /// <returns></returns>
        Task<bool> IsFriend(int entityId, int anotherEntityId);

        /// <summary>
        /// 通过实体Id获取玩家坐标
        /// </summary>
        /// <param name="entityId">实体Id</param>
        /// <returns></returns>
        Task<Position> GetPlayerPosition(int entityId);
    }
}