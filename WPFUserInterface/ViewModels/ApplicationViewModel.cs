using System.IO;
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
            if(!Directory.Exists(DefaultOutputPath))
            {
                Directory.CreateDirectory(DefaultOutputPath);
                File.Create(Path.Combine(DefaultOutputPath, "LoggedOutput.txt"));
            }

            Logger = new Logger(Path.Combine(DefaultOutputPath, "LoggedOutput.txt"));
            CurrentViewModel = new PDFEditViewModel(Logger);
            //_navStore = navigationStore;
        }
    }
}
