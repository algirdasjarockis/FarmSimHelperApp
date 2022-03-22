using FarmSimHelper.ViewModels;
using FarmSimHelper.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FarmSimHelper
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            Routing.RegisterRoute(nameof(PricesViewModel), typeof(PricesPage));
            Routing.RegisterRoute(nameof(YieldViewModel), typeof(YieldPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
