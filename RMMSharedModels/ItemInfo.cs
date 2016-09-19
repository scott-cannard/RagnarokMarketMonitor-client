using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RMMSharedModels
{
    public class ItemInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }
        [System.Xml.Serialization.XmlArray("Vendors")]
        [System.Xml.Serialization.XmlArrayItem("ShopInfo")]
        public List<ShopInfo> Vendors { get; set; }
        [System.Xml.Serialization.XmlArray("Buyers")]
        [System.Xml.Serialization.XmlArrayItem("ShopInfo")]
        public List<ShopInfo> Buyers { get; set; }
        public uint AvgPrice_Asking { get; set; }
        public uint AvgPrice_Offering { get; set; }
        public uint AvgPrice_Sold { get; set; }
        public uint AvgPrice_Bought { get; set; }
        public uint NPCBuyPrice { get; set; }


        public int VendorCount { get { return Vendors.Count; } }
        public int BuyerCount { get { return Buyers.Count; } }


        public ItemInfo()
        { }
        public ItemInfo(string name)
        {
            this.Name = name;
            Vendors = new List<ShopInfo>();
            Buyers = new List<ShopInfo>();
        }

        public static bool operator >(ItemInfo modelA, ItemInfo modelB)
        {
            return new ItemInfoComparer().Compare(modelA, modelB) > 0;
        }
        public static bool operator <(ItemInfo modelA, ItemInfo modelB)
        {
            return new ItemInfoComparer().Compare(modelA, modelB) < 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
