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
    public class PricesViewModel : BaseViewModel
    {
        readonly ISellPriceLoader priceLoader;
        readonly IProductPriceCalculator priceCalculator;
        public ObservableCollection<SellingPrice> Items { get; private set; }

        public Command LoadItemsCommand { get; private set; }
        public Command<string> RecalculateCommand { get; private set; }

        public PricesViewModel(ISellPriceLoader priceLoader, IProductPriceCalculator priceCalculator)
        {
            this.priceLoader = priceLoader;
            this.priceCalculator = priceCalculator;
            Title = "Average Selling Prices";
            Items = new ObservableCollection<SellingPrice>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            RecalculateCommand = new Command<string>(ExecuteRecalculateCommand);
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            var items = await this.priceLoader.LoadSellingPrices();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            IsBusy = false;
        }

        async void ExecuteRecalculateCommand(string economyDifficulty)
        {
            float factor;
            switch (economyDifficulty)
            {
                case "easy":
                    factor = 3.0f;
                    break;
                case "normal":
                    factor = 1.8f;
                    break;
                default:
                    factor = 1.0f;
                    break;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = priceCalculator.RecalculateSellingPrice(Items[i], factor);
            }
        }

        public async void OnAppearing()
        {
            await ExecuteLoadCommand();
        }
    }
}
