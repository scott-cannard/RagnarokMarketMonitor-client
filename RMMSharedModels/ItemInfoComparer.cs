using System;
using System.Collections.Generic;

namespace RMMSharedModels
{
    public class ItemInfoComparer : IComparer<ItemInfo>
    {
        public int Compare(ItemInfo modelA, ItemInfo modelB)
        {
            if (modelA == null) throw new ArgumentNullException("modelA", "ItemInfoModel for comparison cannot be null");
            if (modelB == null) throw new ArgumentNullException("modelB", "ItemInfoModel for comparison cannot be null");

            return String.Compare(modelA.Name, modelB.Name);
        }
    }
}
