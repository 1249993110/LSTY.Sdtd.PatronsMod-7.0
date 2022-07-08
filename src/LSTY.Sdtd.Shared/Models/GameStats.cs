namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 游戏统计
    /// </summary>
    public class GameStats
    {
        /// <summary>
        /// 动物数
        /// </summary>
        public int Animals { get; set; }

        /// <summary>
        /// 游戏时间
        /// </summary>
        public GameTime GameTime { get; set; }

        /// <summary>
        /// 僵尸数
        /// </summary>
        public int Hostiles { get; set; }

        /// <summary>
        /// 在线玩家数
        /// </summary>
        public int OnlinePlayers { get; set; }

        /// <summary>
        /// 离线玩家数量
        /// </summary>
        public int OfflinePlayers { get; set; }
    }
}