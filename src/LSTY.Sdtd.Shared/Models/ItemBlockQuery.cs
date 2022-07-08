using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 物品方块查询参数
    /// </summary>
    public class ItemBlockQuery
    {
        /// <summary>
        /// 物品方块种类
        /// </summary>
        public ItemBlockKind ItemBlockKind { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页数量，值小于 0 时返回所有记录
        /// </summary>
        public int PageSzie { get; set; } = 10;

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; } = "schinese";

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否展示开发物品方块
        /// </summary>
        public bool ShowUserHidden { get; set; }
    }
}
