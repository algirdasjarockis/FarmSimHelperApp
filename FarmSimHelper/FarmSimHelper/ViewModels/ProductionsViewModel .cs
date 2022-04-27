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
    public class ProductionsViewModel : BaseViewModel
    {
        bool loaded;
        IDataLoader<ProductionInfo, int> productionLoader;

        public ObservableCollection<ProductionInfo> Items { get; private set; }

        public Command LoadItemsCommand { get; private set; }
        public Command<EconomyDifficulty> RecalculateCommand { get; private set; }

        public ProductionsViewModel(IDataLoader<ProductionInfo, int> loader)
        {
            productionLoader = loader;
            Title = "Productions";
            Items = new ObservableCollection<ProductionInfo>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            var items = await productionLoader.LoadData();
            foreach (var item in items)
            {
                Items.Add(item);
            }

            IsBusy = false;
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
