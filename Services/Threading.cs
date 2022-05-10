using System;
using System.Threading;
using System.Windows;

namespace Snippets.Services
{
   public class Threading
   {
        public static void OutputMessage(string message)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                StatusListView.Items.Add(message);
            }));
        }
    }
}
