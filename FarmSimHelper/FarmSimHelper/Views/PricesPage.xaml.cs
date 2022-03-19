using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using FarmSimHelper.ViewModels;

namespace FarmSimHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PricesPage : ContentPage
    {
        readonly PricesViewModel viewModel;

        public PricesPage()
        {
            InitializeComponent();

            using (var scope = App.Scope.BeginLifetimeScope())
            {
                viewModel = scope.Resolve<PricesViewModel>();
            }

            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}