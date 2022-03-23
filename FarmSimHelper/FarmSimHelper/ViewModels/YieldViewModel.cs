using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FarmSimHelper.Models;
using FarmSimHelper.Services;

namespace FarmSimHelper.ViewModels
{
    public class YieldViewModel : BaseViewModel
    {
        bool useHectares;
        IYieldInfoLoader yieldInfoLoader;

        public float TestValue { get; set; }

        public bool UseHectares
        {
            get => useHectares;
            set { SetProperty(ref useHectares, value); }
        }
        public ObservableCollection<ProductYieldInfo> Items { get; private set; }
        public Command LoadItemsCommand { get; private set; }
        public Command UseHaCommand { get; private set; }

        public YieldViewModel(IYieldInfoLoader loader)
        {
            yieldInfoLoader = loader;
            UseHectares = true;
            TestValue = 0.92f;
            Items = new ObservableCollection<ProductYieldInfo>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            UseHaCommand = new Command(ExecuteRecalculateCommand) ;
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
            UseHectares = !UseHectares;

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = new ProductYieldInfo()
                {
                    LitersPerSqm = Items[i].LitersPerSqm,
                    Liters = Items[i].LitersPerSqm * (UseHectares ? 10000 : 4046.86f),
                    Product = Items[i].Product,
                    ProductImage = Items[i].ProductImage,
                };
            }
        }

        public async void OnAppearing()
        {
            await ExecuteLoadCommand();
        }
    }
}
