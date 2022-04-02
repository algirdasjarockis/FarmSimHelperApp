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

        IDataDownloader downloader;
        Settings settings;

        public string SelectedMap { get; set; }
        public SquareUnit SelectedUnit { get; set; }
        public List<string> Maps { get; private set; }
        public List<FieldInfo> Fields { get; private set; }
        public List<SquareUnit> Units { get; private set; }

        public Command DownloadDataCommand { get; private set; }
        public Command UnitChangeCommand { get; private set; }

        public SettingsViewModel(IDataDownloader downloader, Settings settings)
        {
            this.downloader = downloader;
            this.settings = settings;

            Maps = Settings.Maps;
            Fields = settings.Fields;
            SelectedMap = settings.Map;
            SelectedUnit = settings.Unit;

            Units = new List<SquareUnit>() { SquareUnit.Hectares, SquareUnit.Acres };

            UnitChangeCommand = new Command(ExecuteUnitChangeCommand);
            DownloadDataCommand= new Command(ExecuteDownloadCommand);
        }

        void ExecuteUnitChangeCommand()
        {
            WeakReferenceMessenger.Default.Send(new SquareUnitChangedMessage());
            settings.Unit = SelectedUnit;
            SettingsService.SaveSettings(settings);
        }

        async void ExecuteDownloadCommand()
        {
            await downloader.DownloadFile(ProductDataUrl, App.Config.DataPathProducts);
            await downloader.DownloadFile(YieldDataUrl, App.Config.DataPathYield);

            await downloader.DownloadFile(FieldDataUrl.Replace("%mapName%", "elmcreek"), App.Config.GetDataPathFields("Elmcreek"));
        }
    }
}
