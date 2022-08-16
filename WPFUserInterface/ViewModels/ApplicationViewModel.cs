using WPFUserInterface.Helpers;

namespace WPFUserInterface.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private readonly NavigationStore _navStore;

        public BaseViewModel CurrentViewModel {get;}

        // settings for the app should live in a hamburger menu?
        // or in general a preferences type dropdown. 
        // TODO: introduce a popup modal service for it. Have it be task based so the user can't make change to runtime during editing

        public ApplicationViewModel(NavigationStore navigationStore)
        {
            Logger logger = new Logger();
            CurrentViewModel = new PDFEditViewModel(logger);
            //_navStore = navigationStore;
        }
    }
}
