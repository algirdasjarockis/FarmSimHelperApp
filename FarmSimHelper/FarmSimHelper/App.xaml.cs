using FarmSimHelper.Services;
using FarmSimHelper.ViewModels;
using FarmSimHelper.Models;
using System;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;

namespace FarmSimHelper
{
    public partial class App : Application
    {
        private readonly IContainer container;
        public static ILifetimeScope Scope { get; private set; }
        public static SettingsViewModel SettingsViewModel { get; private set; }

        public static class Config
        {
            public static string DataRoot => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static string DataPathProducts => Path.Combine(DataRoot, "fillTypes.xml");
            public static string DataPathYield => Path.Combine(DataRoot, "fruitTypes.xml");
            public static string DataPathProductions => Path.Combine(DataRoot, "productions");
            public static Settings Settings;

            public static string GetDataPathFields(string mapName)
            {
                return Path.Combine(DataRoot, $"fields_{mapName.ToLower()}.xml");
            }

            public static string GetDataPathProductions(string productionId)
            {
                return Path.Combine(DataRoot, $"productions/{productionId}.xml");
            }
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("##SyncfusionLicense##");

            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            var builder = new ContainerBuilder();
            
            // services
            builder.RegisterType<ProductPriceCalculator>().As<IProductPriceCalculator>();
            builder.RegisterType<SellPriceLoader>().As<ISellPriceLoader>();
            builder.RegisterType<YieldInfoLoader>().As<IDataLoader<ProductYieldInfo, SquareUnit>>();
            builder.RegisterType<FieldInfoLoader>().As<IDataLoader<FieldInfo, string>>();
            builder.RegisterType<DataDownloader>().As<IDataDownloader>();
            builder.RegisterType<HttpClient>();

            builder.Register(c => { 
                var client = new HttpClient(); 
                client.Timeout = TimeSpan.FromSeconds(3);
                return client; 
            }).As<HttpClient>();

            // viewmodels
            builder.RegisterType<PricesViewModel>().SingleInstance();
            builder.RegisterType<YieldViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            // custom stuff
            builder.Register(c => Config.Settings).As<Settings>().SingleInstance();

            container = builder.Build();
            Scope = container.BeginLifetimeScope();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            // 1. load settings file
            if (SettingsService.SettingsExist())
            {
                Config.Settings = SettingsService.LoadSettings();
            } else {
                // default settings
                Config.Settings = new Settings();
                SettingsService.SaveSettings(Config.Settings);
            }

            // 2. load fields data according to settings (selected map)
            var loader = new FieldInfoLoader();
            var fields = await loader.LoadData(Config.Settings.Map);
            foreach (var field in fields) { Config.Settings.Fields.Add(field); }

            if (!DataFilesExist())
            {
                WeakReferenceMessenger.Default.Send(new NoDataFilesFoundMessage());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        bool DataFilesExist()
        {
            bool missing = !File.Exists(Config.DataPathYield) || !File.Exists(Config.DataPathProducts);
            if (missing)
            {
                return false;
            }

            foreach (var file in Settings.Maps)
            {
                if (!File.Exists(Config.GetDataPathFields(file)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
