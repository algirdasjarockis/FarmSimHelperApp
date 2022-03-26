using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CommunityToolkit.Mvvm.Messaging;
using FarmSimHelper.Models;
using FarmSimHelper.Services;

namespace FarmSimHelper.ViewModels
{
    public class YieldViewModel : BaseViewModel
    {
        bool loaded;
        IYieldInfoLoader yieldInfoLoader;
        SettingsViewModel settingsViewModel;

        string textColumnLiters;

        public string TextColumnLiters
        {
            get { return textColumnLiters; }
            set { SetProperty(ref textColumnLiters, value); }
        }

        public ObservableCollection<ProductYieldInfo> Items { get; private set; }
        public Command LoadItemsCommand { get; private set; }
        public Command UseHaCommand { get; private set; }

        public YieldViewModel(IYieldInfoLoader loader, SettingsViewModel vm)
        {
            yieldInfoLoader = loader;
            settingsViewModel = vm;
           
            Items = new ObservableCollection<ProductYieldInfo>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            UseHaCommand = new Command(ExecuteRecalculateCommand);

            SetTextForColumnLiters();
            WeakReferenceMessenger.Default.Register<SquareUnitChangedMessage>(this, (r, m) => ExecuteRecalculateCommand());
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            var items = await yieldInfoLoader.LoadYieldInfo();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            IsBusy = false;
        }

        void ExecuteRecalculateCommand()
        {
            SetTextForColumnLiters();
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = new ProductYieldInfo()
                {
                    LitersPerSqm = Items[i].LitersPerSqm,
                    Liters = Items[i].LitersPerSqm * (settingsViewModel.SelectedUnit == SquareUnit.Hectares ? 10000 : 4046.86f),
                    Product = Items[i].Product,
                    ProductImage = Items[i].ProductImage,
                };
            }
        }

        void SetTextForColumnLiters()
        {
            string unit = settingsViewModel.SelectedUnit == SquareUnit.Hectares
                ? "ha"
                : "acre";

            TextColumnLiters = "Liters/" + unit;
        }

        public async void OnAppearing()
        {
            if (!loaded)
            {
                await ExecuteLoadCommand();
                loaded = true;
            }
        }
    }
}
