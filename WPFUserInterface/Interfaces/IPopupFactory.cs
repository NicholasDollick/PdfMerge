using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUserInterface.Interfaces
{
    internal interface IPopupFactory
    {
        Task CreateNewSettingsPopup();
    }
}
