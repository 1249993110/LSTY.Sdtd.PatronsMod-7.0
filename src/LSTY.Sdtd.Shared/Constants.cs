namespace LSTY.Sdtd.Shared
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public struct Constants
    {
        /// <summary>
        /// 默认服务器名称
        /// </summary>
        public const string DefaultServerName = "Server";

        /// <summary>
        /// 非玩家
        /// </summary>
        public const string NonPlayer = "-non-player-";

        /// <summary>
        /// 传送目标类型
        /// </summary>
        public struct TeleTargetTypes
        {
            /// <summary>
            /// 城市
            /// </summary>
            public const string City = nameof(City);

            /// <summary>
            /// 家
            /// </summary>
            public const string Home = nameof(Home);

            /// <summary>
            /// 好友
            /// </summary>
            public const string Friend = nameof(Friend);
        }

        /// <summary>
        /// 文化
        /// </summary>
        public struct Cultures
        {
            /// <summary>
            /// 英文
            /// </summary>
            public const string En = "en";

            /// <summary>
            /// 中文
            /// </summary>
            public const string ZhCn = "zh-CN";
        }
    }
}