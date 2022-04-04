using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CommunityToolkit.Mvvm.Messaging;
using FarmSimHelper.Models;
using FarmSimHelper.Services;

namespace FarmSimHelper.ViewModels
{
    public class YieldViewModel : BaseViewModel
    {
        bool loaded;
        readonly IDataLoader<ProductYieldInfo, SquareUnit> yieldInfoLoader;
        readonly SettingsViewModel settingsViewModel;
        List<int> selectedFields;

        // texts
        string? textColumnLiters;
        string? textColumnLitersForFields;
        string? textFieldSelect;

        public string? TextColumnLiters
        {
            get { return textColumnLiters; }
            set { SetProperty(ref textColumnLiters, value); }
        }

        public string? TextColumnLitersForFields
        {
            get { return textColumnLitersForFields; }
            set { SetProperty(ref textColumnLitersForFields, value); }
        }

        public string? TextFieldSelect
        {
            get { return textFieldSelect; }
            set { SetProperty(ref textFieldSelect, value); }
        }

        public List<int> SelectedFields
        {
            get { return selectedFields; }
            set { SetProperty(ref selectedFields, value); }
        }

        public ObservableCollection<ProductYieldInfo> Items { get; private set; }
        public ObservableCollection<FieldInfo> Fields { get; set; }
        public Command LoadItemsCommand { get; private set; }
        public Command FieldsSelectCommand { get; private set; }

        public YieldViewModel(IDataLoader<ProductYieldInfo, SquareUnit> loader, SettingsViewModel vm)
        {
            yieldInfoLoader = loader;
            settingsViewModel = vm;
           
            Items = new ObservableCollection<ProductYieldInfo>();
            Fields = new ObservableCollection<FieldInfo>();
            SelectedFields = new List<int>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            FieldsSelectCommand = new Command(ExecuteRecalculateCommand);

            Title = "Yield information";
            TextFieldSelect = $"Select '{vm.SelectedMap}' fields";
            SetTextForColumns();

            WeakReferenceMessenger.Default.Register<SquareUnitChangedMessage>(this, (r, m) => ExecuteRecalculateCommand());
            WeakReferenceMessenger.Default.Register<MapChangedMessage>(this, (r, m) => ReloadFields());
        }

        async Task ExecuteLoadCommand()
        {
            IsBusy = true;
            Items.Clear();

            var items = await yieldInfoLoader.LoadData(settingsViewModel.SelectedUnit);
            foreach (var item in items)
            {
                Items.Add(item);
            }

            ReloadFields();

            IsBusy = false;
        }

        void ReloadFields()
        {
            TextFieldSelect = $"Select '{settingsViewModel.SelectedMap}' fields";
            Fields.Clear();
            foreach (var field in settingsViewModel.Fields)
            {
                Fields.Add(field);
            }
        }

        void ExecuteRecalculateCommand()
        {
            SetTextForColumns();
            float totalFieldSize = 0.0f;

            foreach (var fieldIndex in SelectedFields)
            {
                totalFieldSize += Fields[fieldIndex].Size;
                Console.WriteLine(Fields[fieldIndex].Id + " -> " + Fields[fieldIndex].Size + "ha");
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i] = new ProductYieldInfo()
                {
                    LitersPerSqm = Items[i].LitersPerSqm,
                    Product = Items[i].Product,
                    ProductImage = Items[i].ProductImage,
                    Liters = Items[i].LitersPerSqm * totalFieldSize
                };
            }
        }

        void SetTextForColumns()
        {
            string unit = settingsViewModel.SelectedUnit == SquareUnit.Hectares
                ? "ha"
                : "acre";

            TextColumnLiters = "Liters/" + unit;
            TextColumnLitersForFields = $"Total {TextColumnLiters} for selected fields";
        }

        public async void OnAppearing()
        {
            if (!loaded)
            {
                await ExecuteLoadCommand();
                loaded = true;
            }
        }
    }
}
