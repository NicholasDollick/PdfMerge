using System.IO;
using WPFUserInterface.Helpers;

namespace WPFUserInterface.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private readonly NavigationStore _navStore;

        public BaseViewModel CurrentViewModel { get; }

        // settings for the app should live in a hamburger menu?
        // or in general a preferences type dropdown. 
        // TODO: introduce a popup handler for it. Have it be task based so the user can't make change to runtime during editing

        public ApplicationViewModel(NavigationStore navigationStore)
        {
            if(!Directory.Exists(DefaultOutputPath))
            {
                Directory.CreateDirectory(DefaultOutputPath);
                CreateDefaultSettingsFile();
            }

            Logger logger = new Logger(Path.Combine(DefaultOutputPath, "LoggedOutput.txt"));
            CurrentViewModel = new PDFEditViewModel(logger);
            //_navStore = navigationStore;
        }

        private void CreateDefaultSettingsFile()
        {
            Path.Combine(DefaultOutputPath, "Settings.ini");
            File.Create(Path.Combine(SettingsJsonLocation));
            object obj = new  {
                SettingsLocation = SettingsJsonLocation,
                DefaultSaveLocation = DefaultOutputPath,
                DefaultFileName = MergedFileName
            };

            // this stuff would need to be writting into the file...eventually
        }
    }
}
