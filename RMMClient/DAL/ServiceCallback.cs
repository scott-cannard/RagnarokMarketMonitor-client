using RMMClient.RMMDataService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RMMClient
{
    public class ServiceCallback : IRagialPollerCallback
    {
        public void ClientUpdate(MemoryStream pushData)
        {
            pushData.Position = 0;
            (App.Current.MainWindow.DataContext as MainViewmodel).GetDAL.OnTrackingUpdate(pushData);
        }

        public void ClientErrorMessage(string message)
        {
            (App.Current.MainWindow.DataContext as MainViewmodel).OnStatusNotification(message);
        }
    }
}
