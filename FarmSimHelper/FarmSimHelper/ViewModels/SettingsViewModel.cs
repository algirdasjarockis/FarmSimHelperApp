using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using Xamarin.Forms;
using FarmSimHelper.Models;

namespace FarmSimHelper.ViewModels
{
    public enum SquareUnit
    {
        Hectares,
        Acres
    }

    public class SettingsViewModel : BaseViewModel
    {
        public string SelectedMap { get; set; }
        public SquareUnit SelectedUnit { get; set; }
        public List<string> Maps { get; private set; }
        public List<SquareUnit> Units { get; private set; }

        public Command UnitChangeCommand { get; private set; }

        public SettingsViewModel()
        {
            Maps = new List<string>()
            {
                "Elmcreek",
                "Erlengrat",
                "Beyleron"
            };

            Units = new List<SquareUnit>() { SquareUnit.Hectares, SquareUnit.Acres };

            SelectedMap = "Elmcreek";
            SelectedUnit = SquareUnit.Hectares;

            UnitChangeCommand = new Command(ExecuteUnitChangeCommand);
        }

        void ExecuteUnitChangeCommand()
        {
            WeakReferenceMessenger.Default.Send(new SquareUnitChangedMessage());
        }
    }
}
