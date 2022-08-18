using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFUserInterface.Interfaces;

namespace WPFUserInterface.Helpers
{
    internal class PopupWindowFactory : IPopupFactory
    {
        public Task CreateNewSettingsPopup()
        {
            PopupWindow popupWindow = new PopupWindow();
            popupWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            popupWindow.Show();
            return Task.FromResult(popupWindow);
        }
    }
}
