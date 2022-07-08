namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 在线玩家
    /// </summary>
    public class OnlinePlayer : PlayerBase
    {
        /// <summary>
        /// 当前生命值
        /// </summary>
        public float CurrentLife { get; set; }
        
        /// <summary>
        /// 死亡次数
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// 到下一级需要的经验
        /// </summary>
        public int ExpToNextLevel { get; set; }

        /// <summary>
        /// 健康
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 领地石保护状态
        /// </summary>
        public bool LandProtectionActive { get; set; }

        /// <summary>
        /// 领地石保护倍数
        /// </summary>
        public float LandProtectionMultiplier { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public float Level { get; set; }

        /// <summary>
        /// 延迟
        /// </summary>
        public int Ping { get; set; }

        /// <summary>
        /// 击杀玩家数量
        /// </summary>
        public int PlayerKills { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 耐力
        /// </summary>
        public float Stamina { get; set; }

        /// <summary>
        /// 总游戏时间，单位：分钟
        /// </summary>
        public float TotalPlayTime { get; set; }

        /// <summary>
        /// 击杀僵尸数量
        /// </summary>
        public int ZombieKills { get; set; }
    }
}