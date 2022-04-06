using FarmSimHelper.ViewModels;
using FarmSimHelper.Views;
using FarmSimHelper.Models;
using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Messaging;
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
            Routing.RegisterRoute(nameof(SettingsViewModel), typeof(SettingsPage));

            WeakReferenceMessenger.Default.Register<NoDataFilesFoundMessage>(this, async (r, m) =>
            {
                await DisplayAlert("Missing data", "There are missing data files. Please go to settings page and press the big button :)", "OK");
            });
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
