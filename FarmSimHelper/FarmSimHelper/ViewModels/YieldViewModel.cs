using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FarmSimHelper.Models;

namespace FarmSimHelper.ViewModels
{
    public class YieldViewModel : BaseViewModel
    {
        public List<ProductYieldInfo> Items { get; private set; }
        public Command LoadItemsCommand { get; private set; }

        public YieldViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            //var items = await this.priceLoader.LoadSellingPrices(GetFactorByCurrentDifficulty());
            //foreach (var item in items)
            //{
            //    items.add(item);
            //}

            IsBusy = false;
        }

        public async void OnAppearing()
        {

        }
    }
}
