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
