using Microsoft.VisualBasic;
using RMMSharedModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace RMMClient
{
    class MainViewmodel : ObservableItem
    {
        #region Private Members
        private DataAbstractionLayer m_DAL;
        private TrulyObservableCollection<ItemInfo> m_TrackedItems = new TrulyObservableCollection<ItemInfo>();
        private double m_HeightForScrollTrigger = App.Current.MainWindow.Height;
        private bool? m_NoSelection = true;
        private ShopInfo m_ShopSpec = null;
        private ItemInfo m_ItemGeneral = null;
        private string m_AppStatus = String.Empty;
        //Private properties
        private MainWindow MainView { get { return App.Current.MainWindow as MainWindow; } }
        #endregion Private Members



        #region VIEW-BOUND PROPERTIES
        public TrulyObservableCollection<ItemInfo> TrackedItems { get { return m_TrackedItems; } }

        public double HeightForScrollTrigger
        {
            get {return m_HeightForScrollTrigger;}
            set
            {
                m_HeightForScrollTrigger = value;
                this.OnPropertyChanged(() => HeightForScrollTrigger);
            }
        }

        public ItemInfo ItemGeneral { get { return m_ItemGeneral; }
                                      set { m_ItemGeneral = value;
                                            this.OnPropertyChanged(() => ItemGeneral); } }

        public ShopInfo ShopSpec { get { return m_ShopSpec; }
                                   set { m_ShopSpec = value;
                                         this.OnPropertyChanged(() => ShopSpec);
                                         this.OnPropertyChanged(() => ItemSpec); } }

        public ItemInfo ItemSpec { get { return m_ShopSpec != null ? m_TrackedItems.FirstOrDefault(item => item.Name.Equals(m_ShopSpec.Item)) : ItemGeneral; } }

        public bool? NoSelection { get { return m_NoSelection; }
                                   set { m_NoSelection = value;
                                         this.OnPropertyChanged(() => NoSelection); } }

        public string AppStatus { get { return m_AppStatus; }
                                  set { m_AppStatus = value;
                                        this.OnPropertyChanged(() => AppStatus); } }

        public ICommand Cmd_TrackNewItem { get; private set; }
        #endregion VIEW-BOUND PROPERTIES
        


        #region Public Properties
        public DataAbstractionLayer GetDAL { get { return m_DAL; } }
        public object lock_TrackedItems = new object();
        #endregion Public Properties



        public MainViewmodel()
        {
            Cmd_TrackNewItem = new Command() { CanExecuteDelegate = CED => true,
                                               ExecuteDelegate = ED => TrackNewItem() };
            m_DAL = new DataAbstractionLayer();
        }


        /// <summary>
        /// 
        /// </summary>
        private void TrackNewItem()
        {
            //Get item name from user input
            string itemToAdd = Interaction.InputBox("Which item would you like track?", "Add Item", String.Empty);

            //DEBUGGING
            if (App.Debug)
            {
                #region Mock ItemInfo
                ItemInfo mockItem = new ItemInfo(itemToAdd);
                mockItem.AvgPrice_Asking = 1;
                mockItem.AvgPrice_Offering = 2;
                mockItem.AvgPrice_Sold = 3;
                mockItem.AvgPrice_Bought = 4;
                mockItem.NPCBuyPrice = 12345;
                mockItem.Vendors.Add(new ShopInfo()
                    {
                        ShopType = ShopInfo.TransactionRole.Vendor,
                        Item = itemToAdd,
                        PlayerName = "Sumdood",
                        Title = "My shop",
                        Map = "Prontera",
                        Coords = "1, 2",
                        Qty = 99,
                        Price = 1000000,
                        Variance = 200,
                        LastSeen = DateTime.Now
                    });
                mockItem.Vendors.Add(new ShopInfo()
                    {
                        ShopType = ShopInfo.TransactionRole.Vendor,
                        Item = itemToAdd,
                        PlayerName = "FinalMerchant",
                        Title = "#2 shop",
                        Map = "Prontera",
                        Coords = "2, 3",
                        Qty = 299,
                        Price = 750000,
                        Variance = -150,
                        LastSeen = DateTime.Now
                    });
                mockItem.Buyers.Add(new ShopInfo()
                    {
                        ShopType = ShopInfo.TransactionRole.Buyer,
                        Item = itemToAdd,
                        PlayerName = "Thatguy",
                        Title = "ImaBuy",
                        Map = "Prontera",
                        Coords = "10, 20",
                        Qty = 400,
                        Price = 1000,
                        Variance = 0,
                        LastSeen = DateTime.Now
                    });
                mockItem.Buyers.Add(new ShopInfo()
                {
                    ShopType = ShopInfo.TransactionRole.Buyer,
                    Item = itemToAdd,
                    PlayerName = "ThaOtherGuy",
                    Title = "ImaBuy2",
                    Map = "Eden",
                    Coords = "2F 10, 20",
                    Qty = 3,
                    Price = 1500,
                    Variance = 50,
                    LastSeen = DateTime.Now
                });
                mockItem.Buyers.Add(new ShopInfo()
                {
                    ShopType = ShopInfo.TransactionRole.Buyer,
                    Item = itemToAdd,
                    PlayerName = "Player One",
                    Title = "Gimme Please",
                    Map = "Payon",
                    Coords = "10, 20",
                    Qty = 7,
                    Price = 800,
                    Variance = -20,
                    LastSeen = DateTime.Now
                });
                this.AppDispatch(() => TrackedItems.Add(mockItem));
                #endregion Mock ItemInfo
            }
            else
            {
                //Validate input
                if (itemToAdd.HasText() && TrackedItems.Where(item => item.Name.Equals(itemToAdd)).Count() == 0)
                {
                    //Send request for item to be added
                    if (m_DAL.AddItem(itemToAdd.Trim()) == DataAbstractionLayer.DAL_ReturnCode.Success)
                    {
                        lock (lock_TrackedItems)
                        {
                            this.AppDispatch(() => TrackedItems.Add(new ItemInfo(itemToAdd)));
                        }
                    }
                    else
                    {
                        OnStatusNotification(itemToAdd + " could not be added.");
                    }
                }
            }
        }

        public void OnStatusNotification(string msg)
        {
            AppStatus = msg;
        }
    }
}
