using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Autofac;
using FarmSimHelper.ViewModels;

namespace FarmSimHelper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductionsPage : ContentPage
    {
        readonly ProductionsViewModel viewModel;

        public ProductionsPage()
        {
            InitializeComponent();

            using (var scope = App.Scope.BeginLifetimeScope())
            {
                viewModel = scope.Resolve<ProductionsViewModel>();
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