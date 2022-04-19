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
        const string ProductDataUrl = @"http://10.0.2.2:8000/fillTypes.xml";
        const string YieldDataUrl = @"http://10.0.2.2:8000/fruitTypes.xml";
        const string FieldDataUrl = @"http://10.0.2.2:8000/fields_%mapName%.xml";
#else
        const string ProductDataUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data/fillTypes.xml";
        const string YieldDataUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data/fruitTypes.xml";
        const string FieldDataUrl = @"https://raw.githubusercontent.com/algirdasjarockis/FarmSimHelperApp/master/data/fields_%mapName%.xml";
#endif

        readonly IDataDownloader downloader;
        readonly IDataLoader<FieldInfo, string> fieldInfoLoader;
        readonly Settings settings;
        float downloadProgressValue = 0.0f;
        bool downloadDone = false;

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
            float total = Settings.Maps.Count + 2;
            float downloaded = 2;

            await downloader.DownloadFile(ProductDataUrl, App.Config.DataPathProducts);
            await downloader.DownloadFile(YieldDataUrl, App.Config.DataPathYield);

            DownloadProgressValue = downloaded / total;
            foreach (var mapName in Settings.Maps)
            {
                await downloader.DownloadFile(
                    FieldDataUrl.Replace("%mapName%", mapName.ToLower()), 
                    App.Config.GetDataPathFields(mapName)
                );

                DownloadProgressValue = ++downloaded / total;
            }

            IsBusy = false;
            DownloadDone = true;
        }
    }
}
