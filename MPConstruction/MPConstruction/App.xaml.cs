using MPConstruction.Apis;
using MPConstruction.ViewModels;
using Prism;
using Prism.Ioc;
using Refit;
using Xamarin.Forms;

namespace MPConstruction
{
    public partial class App
    {
        class DummyStartup : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry) { }
        }

        public App() : base(new DummyStartup())
        {
        }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
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
            var imageService = RestService.For<IImageApi>("https://reqres.in");
            containerRegistry.RegisterInstance(imageService);
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

    }
}
