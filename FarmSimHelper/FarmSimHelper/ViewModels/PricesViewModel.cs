using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using FarmSimHelper.Models;
using FarmSimHelper.Services;
using Xamarin.Forms;

namespace FarmSimHelper.ViewModels
{
    public enum EconomyDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public class PricesViewModel : BaseViewModel
    {
        bool loaded;
        EconomyDifficulty selectedEconomyDifficulty;
        readonly ISellPriceLoader priceLoader;
        readonly IProductPriceCalculator priceCalculator;

        public ObservableCollection<SellingPrice> Items { get; private set; }
        public EconomyDifficulty SelectedEconomyDifficulty 
        {
            get => selectedEconomyDifficulty;
            set { SetProperty(ref selectedEconomyDifficulty, value); }
        }

        public Command LoadItemsCommand { get; private set; }
        public Command<EconomyDifficulty> RecalculateCommand { get; private set; }

        public PricesViewModel(ISellPriceLoader priceLoader, IProductPriceCalculator priceCalculator)
        {
            this.priceLoader = priceLoader;
            this.priceCalculator = priceCalculator;
            SelectedEconomyDifficulty = EconomyDifficulty.Normal;
            Title = "Average Selling Prices";
            Items = new ObservableCollection<SellingPrice>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            RecalculateCommand = new Command<EconomyDifficulty>(ExecuteRecalculateCommand);
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            var items = await this.priceLoader.LoadSellingPrices(GetFactorByCurrentDifficulty());
            foreach (var item in items)
            {
                Items.Add(item);
            }

            IsBusy = false;
        }

        async void ExecuteRecalculateCommand(EconomyDifficulty economyDifficulty)
        {
            SelectedEconomyDifficulty = economyDifficulty;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = priceCalculator.RecalculateSellingPrice(Items[i], GetFactorByCurrentDifficulty());
            }
        }

        float GetFactorByCurrentDifficulty()
        {
            float factor;
            switch (SelectedEconomyDifficulty)
            {
                case EconomyDifficulty.Easy:
                    factor = 3.0f;
                    break;
                case EconomyDifficulty.Normal:
                    factor = 1.8f;
                    break;
                default:
                    factor = 1.0f;
                    break;
            }

            return factor;
        }

        public async void OnAppearing()
        {
            if (!loaded)
            {
                await ExecuteLoadCommand();
                loaded = Items.Count > 0;
            }
        }
    }
}
