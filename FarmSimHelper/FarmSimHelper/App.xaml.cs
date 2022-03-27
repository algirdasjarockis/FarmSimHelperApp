using FarmSimHelper.Services;
using FarmSimHelper.ViewModels;
using System;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;

namespace FarmSimHelper
{
    public partial class App : Application
    {
        private readonly IContainer container;
        public static ILifetimeScope Scope { get; private set; }

        public static class Config
        {
            public static string DataRoot => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static string DataPathProducts => Path.Combine(DataRoot, "fillTypes.xml");
            public static string DataPathYield => Path.Combine(DataRoot, "fruitTypes.xml");

            public static string GetDataPathFields(string mapName)
            {
                return Path.Combine(DataRoot, $"fields_{mapName.ToLower()}");
            }
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("##SyncfusionLicense##");

            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            var builder = new ContainerBuilder();
            //builder.Register(c => new SellPriceLoader(new HttpClient(), new ProductPriceCalculator())).As<ISellPriceLoader>();
            builder.RegisterType<ProductPriceCalculator>().As<IProductPriceCalculator>();
            builder.RegisterType<SellPriceLoader>().As<ISellPriceLoader>();
            builder.RegisterType<YieldInfoLoader>().As<IYieldInfoLoader>();
            builder.RegisterType<DataDownloader>().As<IDataDownloader>();
            builder.RegisterType<HttpClient>();
            builder.RegisterType<PricesViewModel>().SingleInstance();
            builder.RegisterType<YieldViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            container = builder.Build();
            Scope = container.BeginLifetimeScope();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
