using LSTY.Sdtd.Shared.Models;

namespace LSTY.Sdtd.Shared.Hubs
{
    /// <summary>
    /// Mod事件中心
    /// </summary>
    public interface IModEventHub
    {
        /// <summary>
        /// 捕获聊天信息时触发
        /// </summary>
        /// <param name="chatMessage"></param>
        void OnChatMessage(ChatMessage chatMessage);

        /// <summary>
        /// 实体被击杀时触发
        /// </summary>
        /// <param name="entity">被击杀者实体</param>
        /// <param name="entityIdThatKilledMe">击杀者实体Id</param>
        void OnEntityKilled(EntityLocation entity, int entityIdThatKilledMe);

        /// <summary>
        /// 实体生成时触发
        /// </summary>
        /// <param name="entity"></param>
        void OnEntitySpawned(EntityLocation entity);

        /// <summary>
        /// 游戏唤醒时触发
        /// </summary>
        void OnGameAwake();

        /// <summary>
        /// 游戏关闭时触发
        /// </summary>
        void OnGameShutdown();

        /// <summary>
        /// 游戏启动完成时触发
        /// </summary>
        void OnGameStartDone();

        /// <summary>
        /// 日志回调时触发
        /// </summary>
        /// <param name="logEntry">日志条目</param>
        void OnLogCallback(LogEntry logEntry);
        /// <summary>
        /// 玩家断开连接时触发
        /// </summary>
        /// <param name="entityId">实体Id</param>
        void OnPlayerDisconnected(int entityId);

        /// <summary>
        /// 玩家在世界中生成时触发
        /// </summary>
        /// <param name="playerSpawned">玩家生成参数</param>
        void OnPlayerSpawnedInWorld(PlayerSpawned playerSpawned);

        /// <summary>
        /// 玩家首次生成时触发
        /// </summary>
        /// <param name="playerBase">玩家基础信息</param>
        void OnPlayerSpawning(PlayerBase playerBase);

        /// <summary>
        /// 保存玩家数据时触发
        /// </summary>
        /// <param name="playerBase">玩家基础信息</param>
        void OnSavePlayerData(PlayerBase playerBase);
    }
}