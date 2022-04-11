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
        bool yieldBonusVisible;

        // texts
        string? textColumnLiters;
        string? textColumnLitersForFields;
        string? textFieldSelect;
        string? textSelectedYieldBonus;

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

        public string? TextSelectedYieldBonus
        {
            get { return textSelectedYieldBonus; }
            set { SetProperty(ref textSelectedYieldBonus, value); }
        }

        public List<int> SelectedFields
        {
            get { return selectedFields; }
            set { SetProperty(ref selectedFields, value); }
        }

        public bool YieldBonusVisible
        {
            get { return yieldBonusVisible; }
            set { SetProperty(ref yieldBonusVisible, value); }
        }

        public YieldBonusSelections YieldBonus { get { return settingsViewModel.Settings.YieldBonus; } }

        public ObservableCollection<ProductYieldInfo> Items { get; private set; }
        public ObservableCollection<FieldInfo> Fields { get; set; }
        public Command LoadItemsCommand { get; private set; }
        public Command FieldsSelectCommand { get; private set; }
        public Command YieldBonusSelectCommand { get; private set; }
        public Command YieldBonusToggleCommand { get; private set; }

        public YieldViewModel(IDataLoader<ProductYieldInfo, SquareUnit> loader, SettingsViewModel vm)
        {
            yieldInfoLoader = loader;
            settingsViewModel = vm;
           
            Items = new ObservableCollection<ProductYieldInfo>();
            Fields = new ObservableCollection<FieldInfo>();
            SelectedFields = new List<int>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadCommand());
            FieldsSelectCommand = new Command(ExecuteRecalculateCommand);
            YieldBonusToggleCommand = new Command(ExecuteYieldBonusToggleCommand);
            YieldBonusSelectCommand = new Command(ExecuteYieldBonusSelectCommand);

            Title = "Yield information";
            TextFieldSelect = $"Select '{vm.SelectedMap}' fields";
            TextSelectedYieldBonus = GetYieldBonusText(vm.Settings.YieldBonus);
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

        void ExecuteYieldBonusSelectCommand()
        {
            ExecuteRecalculateCommand();
            TextSelectedYieldBonus = GetYieldBonusText(settingsViewModel.Settings.YieldBonus);
        }

        void ExecuteYieldBonusToggleCommand()
        {
            YieldBonusVisible = !YieldBonusVisible;
            if (!YieldBonusVisible)
            {
                // save selection of yield bonuses
                SettingsService.SaveSettings(settingsViewModel.Settings);
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

        string GetYieldBonusText(YieldBonusSelections selectedBonus)
        {
            List<string> bonus = new List<string>();
            int fertilized = (selectedBonus.Fertilized1 ? 1 : 0) + (selectedBonus.Fertilized2 ? 1 : 0);
           
            if (fertilized > 0)
            {
                bonus.Add($"Fertilized x{fertilized}");
            }
            if (selectedBonus.Weeded) { bonus.Add("Weeded"); }
            if (selectedBonus.Limed) { bonus.Add("Limed"); }
            if (selectedBonus.Plowed) { bonus.Add("Plowed"); }
            if (selectedBonus.Rolled) { bonus.Add("Rolled"); }
            if (selectedBonus.Mulched) { bonus.Add("Mulched"); }

            if (bonus.Count <= 0)
            {
                bonus.Add("Select yield improvements");
            }

            return string.Join(", ", bonus);
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
