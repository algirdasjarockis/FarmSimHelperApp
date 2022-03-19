using FarmSimHelper.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace FarmSimHelper.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}