﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 物品方块
    /// </summary>
    public class ItemBlock
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否为方块
        /// </summary>
        public bool IsBlock { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 本地化名称
        /// </summary>
        public string LocalizationName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string ItemIcon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string IconColor { get; set; }

        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        public int MaxStackAllowed { get; set; }
    }
}
