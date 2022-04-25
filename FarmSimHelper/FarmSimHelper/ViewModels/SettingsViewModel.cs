using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using Xamarin.Forms;
using FarmSimHelper.Models;
using FarmSimHelper.Services;

namespace FarmSimHelper.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
#if DEBUG
        const string BaseUrl = @"http://10.0.2.2:8000";
#else
        const string BaseUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data";
#endif
        const string FieldDataUrl = BaseUrl + @"/fields_%mapName%.xml";
        const string ProductionDataUrl = BaseUrl + @"/productions/%production%.xml";

        readonly IDataDownloader downloader;
        readonly IDataLoader<FieldInfo, string> fieldInfoLoader;
        readonly Settings settings;
        float downloadProgressValue = 0.0f;
        bool downloadDone = false;
        bool downloadFailed = false;
        List<string[]> dataLocations;

        public Settings Settings { get { return settings; } }
        public string SelectedMap { get; set; }
        public SquareUnit SelectedUnit { get; set; }
        public List<string> Maps { get; private set; }
        public List<FieldInfo> Fields { get; private set; }
        public List<SquareUnit> Units { get; private set; }
        public float DownloadProgressValue 
        { 
            get { return downloadProgressValue; } 
            set { SetProperty(ref downloadProgressValue, value); } 
        }
        public bool DownloadDone
        {
            get { return downloadDone; }
            set { SetProperty(ref downloadDone, value); }
        }
        public bool DownloadFailed
        {
            get { return downloadFailed; }
            set { SetProperty(ref downloadFailed, value); }
        }

        public Command DownloadDataCommand { get; private set; }
        public Command UnitChangeCommand { get; private set; }
        public Command MapChangeCommand { get; private set; }

        public SettingsViewModel(IDataDownloader downloader, IDataLoader<FieldInfo, string> fieldInfoLoader, Settings settings)
        {
            this.downloader = downloader;
            this.fieldInfoLoader = fieldInfoLoader;
            this.settings = settings;

            Maps = Settings.Maps;
            Fields = settings.Fields;
            SelectedMap = settings.Map;
            SelectedUnit = settings.Unit;

            Units = new List<SquareUnit>() { SquareUnit.Hectares, SquareUnit.Acres };

            UnitChangeCommand = new Command(ExecuteUnitChangeCommand);
            MapChangeCommand = new Command(ExecuteMapChangeCommand);
            DownloadDataCommand= new Command(ExecuteDownloadCommand);

            dataLocations = new List<string[]>()
            {
                // from, to
                { new string[2] { BaseUrl + "/fillTypes.xml", App.Config.DataPathProducts } },
                { new string[2] { BaseUrl + "/fruitTypes.xml", App.Config.DataPathYield } },
            };

            foreach (var mapName in Settings.Maps)
            {
                var mapLocations = new string[2]
                {
                    FieldDataUrl.Replace("%mapName%", mapName.ToLower()),
                    App.Config.GetDataPathFields(mapName)
                };

                dataLocations.Add(mapLocations);
            }

            foreach (var production in Settings.Productions)
            {
                var prodLocations = new string[2]
                {
                    ProductionDataUrl.Replace("%production%", production),
                    App.Config.GetDataPathProductions(production)
                };

                dataLocations.Add(prodLocations);
            }
        }

        void ExecuteUnitChangeCommand()
        {
            settings.Unit = SelectedUnit;
            SettingsService.SaveSettings(settings);
            WeakReferenceMessenger.Default.Send(new SquareUnitChangedMessage());
        }

        async void ExecuteMapChangeCommand()
        {
            settings.Map = SelectedMap;
            
            Fields.Clear();
            var fields = await fieldInfoLoader.LoadData(SelectedMap);
            foreach (var field in fields) { Fields.Add(field); }

            SettingsService.SaveSettings(settings);

            WeakReferenceMessenger.Default.Send(new MapChangedMessage());
        }

        async void ExecuteDownloadCommand()
        {
            IsBusy = true;
            DownloadProgressValue = 0.0f;
            DownloadDone = false;
            DownloadFailed = false;

            float total = dataLocations.Count;
            float downloaded = 0;

            foreach (var location in dataLocations)
            {
                DownloadFailed = !await downloader.DownloadFile(location[0], location[1]);

                if (DownloadFailed)
                {
                    break;
                }

                DownloadProgressValue = ++downloaded / total;
            }

            IsBusy = false;
            DownloadDone = !DownloadFailed;
        }
    }
}
