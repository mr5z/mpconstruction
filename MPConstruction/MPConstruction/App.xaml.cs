using MPConstruction.ViewModels;
using Prism.Ioc;
using Xamarin.Forms;

namespace MPConstruction
{
    public partial class App
    {
        public App()
        {

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync("MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

    }
}
