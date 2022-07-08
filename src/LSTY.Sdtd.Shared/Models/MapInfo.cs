namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 地图信息
    /// </summary>
    public class MapInfo
    {
        /// <summary>
        /// 块大小
        /// </summary>
        public int BlockSize { get; set; } = 8;

        /// <summary>
        /// 最大缩放
        /// </summary>
        public int MaxZoom { get; set; } = 4;
    }
}