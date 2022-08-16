using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUserInterface.ViewModels;

namespace WPFUserInterface.Helpers
{
    public class NavigationCommand : CommandBase
    {
        private readonly NavigationStore _navigationService;

        public NavigationCommand(NavigationStore navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            //_navigationService.CurrentViewModel = new PDFEditViewModel();
        }
    }
}
