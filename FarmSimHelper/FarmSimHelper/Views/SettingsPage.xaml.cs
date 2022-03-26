using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FarmSimHelper.ViewModels;

namespace FarmSimHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        readonly SettingsViewModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();

            using (var scope = App.Scope.BeginLifetimeScope())
            {
                viewModel = scope.Resolve<SettingsViewModel>();
            }

            BindingContext = viewModel;
        }
    }
}