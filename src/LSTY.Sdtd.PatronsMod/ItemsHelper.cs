using LSTY.Sdtd.PatronsMod.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSTY.Sdtd.PatronsMod
{
    /// <summary>
    /// see <see cref="XUiC_CreativeWindow"/>
    /// </summary>
    internal static class ItemsHelper
    {
        public static List<Shared.Models.ItemBlock> GetAllItemsAndBlocks(string language, string keyword, bool showUserHidden)
        {
            return GetItemsAndBlocks(0, ItemClass.list.Length, null, language, keyword, showUserHidden);
        }
        public static List<Shared.Models.ItemBlock> GetAllItems(string language, string keyword, bool showUserHidden)
        {
            return GetItemsAndBlocks(Block.ItemsStartHere, ItemClass.list.Length, item => item.IsBlock() == false, language, keyword, showUserHidden);
        }
        public static List<Shared.Models.ItemBlock> GetAllBlocks(string language, string keyword, bool showUserHidden)
        {
            return GetItemsAndBlocks(0, Block.ItemsStartHere, item => item.IsBlock() == true, language, keyword, showUserHidden);
        }

        public static List<Shared.Models.ItemBlock> GetItemsAndBlocks(
            int startId, 
            int endId, 
            Func<ItemClass, bool> filter, 
            string language,
            string keyword,
            bool showUserHidden)
        {
            var dict = Localization.dictionary;
            int languageIndex = Array.LastIndexOf(dict["KEY"], language);

            if (languageIndex < 0)
            {
                throw new Exception($"The specified language: {language} does not exist");
            }

            var result = new List<Shared.Models.ItemBlock>();
            for (int i = startId; i < endId; i++)
            {
                ItemClass item = ItemClass.GetForId(i);
                if (item != null)
                {
                    EnumCreativeMode creativeMode = item.CreativeMode;
                    if (creativeMode != EnumCreativeMode.None
                        && creativeMode != EnumCreativeMode.Test
                        && (creativeMode == EnumCreativeMode.All || showUserHidden)
                        && (filter == null || filter.Invoke(item)))
                    {
                        string itemName = item.GetItemName();

                        if (string.IsNullOrEmpty(itemName))
                        {
                            continue;
                        }

                        string localizationName;
                        if (dict.ContainsKey(itemName) == false)
                        {
                            localizationName = itemName;
                        }
                        else
                        {
                            localizationName = dict[itemName][languageIndex];
                        }

                        if(string.IsNullOrEmpty(keyword) == false)
                        {
                            if(itemName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) == -1
                                && localizationName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) == -1)
                            {
                                continue;
                            }
                        }

                        var itemBlock = new Shared.Models.ItemBlock()
                        {
                            Id = i,
                            ItemName = itemName,
                            IconColor = item.GetIconTint().ToHex(),
                            MaxStackAllowed = item.Stacknumber.Value,
                            ItemIcon = item.GetIconName(),
                            IsBlock = item.IsBlock(),
                            LocalizationName = localizationName
                        };

                        result.Add(itemBlock);
                    }
                }
            }

            return result;
        }


    }
}
