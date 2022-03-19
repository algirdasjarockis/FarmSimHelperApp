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
        public ObservableCollection<SellingPrice> Items { get; private set; }

        public Command LoadItemsCommand { get; private set; }

        public PricesViewModel(ISellPriceLoader priceLoader)
        {
            this.priceLoader = priceLoader;
            Title = "Average Selling Prices";

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            Items = new ObservableCollection<SellingPrice>();
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

        public async void OnAppearing()
        {
            await ExecuteLoadCommand();
        }
    }
}
