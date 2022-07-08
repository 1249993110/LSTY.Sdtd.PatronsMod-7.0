namespace LSTY.Sdtd.Shared.Models
{
    public class Teleport
    {
        /// <summary>
        /// 被传送玩家的Id或昵称
        /// </summary>
        public string OriginPlayerIdOrName { get; set; }

        /// <summary>
        /// 目标玩家的Id或昵称，或坐标
        /// </summary>
        public string TargetPlayerIdOrNameOrPosition { get; set; }
    }
}