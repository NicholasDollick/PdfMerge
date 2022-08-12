using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUserInterface.Helpers;

namespace WPFUserInterface.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private readonly NavigationStore _navStore;

        public BaseViewModel CurrentViewModel {get;}

        public ApplicationViewModel(NavigationStore navigationStore)
        {
            CurrentViewModel = new LoginViewModel();
            //_navStore = navigationStore;
        }
    }
}
