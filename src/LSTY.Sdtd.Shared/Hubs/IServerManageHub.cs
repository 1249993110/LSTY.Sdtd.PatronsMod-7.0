﻿using LSTY.Sdtd.Shared.Models;

namespace LSTY.Sdtd.Shared.Hubs
{
    /// <summary>
    /// 服务器管理中心
    /// </summary>
    public interface IServerManageHub
    {
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admins">管理员</param>
        /// <returns></returns>
        Task<IEnumerable<string>> AddAdmins(IEnumerable<AdminEntry> admins);

        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="blacklist">黑名单</param>
        /// <returns></returns>
        Task<IEnumerable<string>> AddBlacklist(IEnumerable<BlacklistEntry> blacklist);

        /// <summary>
        /// 添加命令权限
        /// </summary>
        /// <param name="permissions">命令权限</param>
        /// <returns></returns>
        Task<IEnumerable<string>> AddPermissions(IEnumerable<PermissionEntry> permissions);

        /// <summary>
        /// 添加白名单
        /// </summary>
        /// <param name="whitelist">白名单</param>
        /// <returns></returns>
        Task<IEnumerable<string>> AddWhitelist(IEnumerable<WhitelistEntry> whitelist);

        /// <summary>
        /// 执行控制台命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="inMainThread">是否在主线程执行</param>
        /// <returns></returns>
        Task<IEnumerable<string>> ExecuteConsoleCommand(string command, bool inMainThread = false);

        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AdminEntry>> GetAdmins();

        /// <summary>
        /// 获取允许的命令
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AllowedCommand>> GetAllowedCommands();

        /// <summary>
        /// 获取允许生成的实体
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AllowSpawnedEntity>> GetAllowSpawnedEntities();

        /// <summary>
        /// 获取动物位置
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EntityLocation>> GetAnimalsLocation();

        /// <summary>
        /// 获取黑名单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BlacklistEntry>> GetBlacklist();

        /// <summary>
        /// 获取完整地图
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GetFullMap();

        /// <summary>
        /// 获取游戏状态
        /// </summary>
        /// <returns></returns>
        Task<GameStats> GetGameStats();

        /// <summary>
        /// 获取僵尸位置
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EntityLocation>> GetHostilesLocation();

        /// <summary>
        /// 获取物品和方块
        /// </summary>
        /// <param name="itemBlockQuery">物品方块查询参数</param>
        /// <returns></returns>
        Task<ItemBlockPaged> GetItemBlocks(ItemBlockQuery itemBlockQuery);

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="iconName"></param>
        /// <returns></returns>
        Task<byte[]> GetItemIcon(string iconName);

        /// <summary>
        /// 获取已知的语言
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetKnownLanguages();

        /// <summary>
        /// 获取指定玩家Id所有的领地石
        /// </summary>
        /// <param name="playerId">玩家平台Id</param>
        /// <returns></returns>
        Task<ClaimOwner> GetLandClaim(string playerId);

        /// <summary>
        /// 获取所有领地石
        /// </summary>
        /// <returns></returns>
        Task<LandClaims> GetLandClaims();

        /// <summary>
        /// 获取本地化字典
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetLocalization(string language = "schinese");

        /// <summary>
        /// 获取指定项目的本地化字符串
        /// </summary>
        /// <param name="itemName">项目名称</param>
        /// <param name="language">语言</param>
        /// <returns></returns>
        Task<string> GetLocalization(string itemName, string language = "schinese");

        /// <summary>
        /// 获取地图信息
        /// </summary>
        /// <returns></returns>
        Task<MapInfo> GetMapInfo();

        /// <summary>
        /// 获取地图切片
        /// </summary>
        /// <param name="zoomLevel">格式如: /z/x/y.png</param>
        /// <returns></returns>
        Task<byte[]> GetMapTile(string zoomLevel);

        /// <summary>
        /// 获取命令权限列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PermissionEntry>> GetPermissions();

        /// <summary>
        /// 获取白名单
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WhitelistEntry>> GetWhitelist();

        /// <summary>
        /// 给予玩家项目
        /// </summary>
        /// <param name="giveItem">项目</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GiveItem(GiveItem giveItem);

        /// <summary>
        /// 移除管理员
        /// </summary>
        /// <param name="playerIds">玩家Id集合</param>
        /// <returns></returns>
        Task<IEnumerable<string>> RemoveAdmins(IEnumerable<string> playerIds);

        /// <summary>
        /// 移除黑名单
        /// </summary>
        /// <param name="playerIds">玩家Id集合</param>
        /// <returns></returns>
        Task<IEnumerable<string>> RemoveBlacklist(IEnumerable<string> playerIds);

        /// <summary>
        /// 移除命令权限
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        Task<IEnumerable<string>> RemovePermissions(IEnumerable<string> command);

        /// <summary>
        /// 移除白名单
        /// </summary>
        /// <param name="playerIds">玩家Id集合</param>
        /// <returns></returns>
        Task<IEnumerable<string>> RemoveWhitelist(IEnumerable<string> playerIds);

        /// <summary>
        /// 渲染完整地图
        /// </summary>
        /// <returns></returns>
        Task RenderFullMap();

        /// <summary>
        /// 重启服务端
        /// </summary>
        /// <param name="force">强制重启</param>
        /// <returns></returns>
        Task<IEnumerable<string>> RestartServer(bool force = false);

        /// <summary>
        /// 向所有连接的客户端发送全局消息
        /// </summary>
        /// <param name="globalMessage">全局消息</param>
        /// <returns></returns>
        Task<IEnumerable<string>> SendGlobalMessage(GlobalMessage globalMessage);

        /// <summary>
        /// 向单个连接的客户端发送私人消息
        /// </summary>
        /// <param name="privateMessage">私人消息</param>
        /// <returns></returns>
        Task<IEnumerable<string>> SendPrivateMessage(PrivateMessage privateMessage);

        /// <summary>
        /// 生成实体
        /// </summary>
        /// <param name="spawnEntity">实体</param>
        /// <returns></returns>
        Task<IEnumerable<string>> SpawnEntity(SpawnEntity spawnEntity);

        /// <summary>
        /// 传送玩家
        /// </summary>
        /// <param name="teleport">传送</param>
        /// <returns></returns>
        Task<IEnumerable<string>> TeleportPlayer(Teleport teleport);
    }
}