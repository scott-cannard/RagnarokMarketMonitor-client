using RMMClient.RMMDataService;
using RMMSharedModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Xml.Serialization;

namespace RMMClient
{
    public class DataAbstractionLayer : IDisposable
    {
        public enum DAL_ReturnCode { Error, Success, ServiceCallFailed }
        

        private RMMDataService.RagialPollerClient DSClient = null;
        private MainViewmodel MainVM { get { return App.Current.MainWindow.DataContext as MainViewmodel; } }


        public DataAbstractionLayer()
        {
            DSClient = new RagialPollerClient(new InstanceContext(new ServiceCallback()));
        }
        public void Dispose()
        {
            if (DSClient != null)
                DSClient.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns></returns>
        public DAL_ReturnCode AddItem(string itemToAdd)
        {
            DAL_ReturnCode result;
            try
            {
                DSClient.RegisterObserver(itemToAdd);
                result = DAL_ReturnCode.Success;
            }
            catch
            {
                result = DAL_ReturnCode.ServiceCallFailed;
            }
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemInfoStream"></param>
        public void OnTrackingUpdate(MemoryStream itemInfoStream)
        {
            ItemInfo updatedModel = null;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ItemInfo));
                updatedModel = (ItemInfo)xmlSerializer.Deserialize(itemInfoStream);
            }
            catch (Exception ex)
            {
                MainVM.OnStatusNotification(ex.Message);
            }

            //Calculate avg prices
            uint sum = 0;
            foreach (ShopInfo vend in updatedModel.Vendors)
            {
                sum += vend.Price;
            }
            updatedModel.AvgPrice_Asking = updatedModel.VendorCount > 0 ? (uint)(sum / updatedModel.VendorCount) : 0;

            sum = 0;
            foreach (ShopInfo buy in updatedModel.Buyers)
            {
                sum += buy.Price;
            }
            updatedModel.AvgPrice_Offering = updatedModel.BuyerCount > 0 ? (uint)(sum / updatedModel.BuyerCount) : 0;

            //price = avg + (variance% * avg)  : factor out avg =>
            //price = avg * (1 + variance%)    : divide by variance factor =>
            //avg = price / (1 + variance%)
            //double avgReversedFromRoundedVariance = (double)(price / (1 + (variance / 100.0)));

            lock (MainVM.lock_TrackedItems)
            {
                try
                {
                    ItemInfo oldModel = MainVM.TrackedItems.FirstOrDefault(item => item.Name.Equals(updatedModel.Name));
                    if (oldModel != null)
                    {
                        MainVM.TrackedItems.Insert(MainVM.TrackedItems.IndexOf(oldModel), updatedModel);
                        MainVM.TrackedItems.Remove(oldModel);
                    }
                    else
                    {
                        MainVM.TrackedItems.Add(updatedModel);
                    }
                }
                catch (Exception ex)
                {
                    MainVM.OnStatusNotification(ex.Message);
                }
            }
        }
    }
}
