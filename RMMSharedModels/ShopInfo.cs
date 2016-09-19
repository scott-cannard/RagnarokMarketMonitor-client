using System;
using System.ComponentModel;
using System.Drawing;

namespace RMMSharedModels
{
    public class ShopInfo : INotifyPropertyChanged
    {
        public enum TransactionRole { Vendor, Buyer };

        public TransactionRole ShopType { get; set; }
        public string Item { get; set; }
        public string PlayerName { get; set; }
        public string Title { get; set; }
        public string Map { get; set; }
        public string Coords { get; set; }
        public uint Qty { get; set; }
        public uint Price { get; set; }
        public int Variance { get; set; }
        public DateTime LastSeen { get; set; }

        public ShopInfo()
        { }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
