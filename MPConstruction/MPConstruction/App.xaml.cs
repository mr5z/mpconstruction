using MPConstruction.Apis;
using MPConstruction.ViewModels;
using Prism;
using Prism.Ioc;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
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
            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri("https://reqres.in")
            };
            var imageService = RestService.For<IImageApi>(httpClient);
            var reportApi = RestService.For<IReportApi>(httpClient);
            containerRegistry.RegisterInstance(imageService);
            containerRegistry.RegisterInstance(reportApi);
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        class LoggingHandler : DelegatingHandler
        {
            public LoggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
            {
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Console.WriteLine("Request:");
                Console.WriteLine(request.ToString());
                if (request.Content != null)
                {
                    Console.WriteLine(await request.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

                Console.WriteLine("Response:");
                Console.WriteLine(response.ToString());
                if (response.Content != null)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                Console.WriteLine();

                return response;
            }
        }

    }
}
