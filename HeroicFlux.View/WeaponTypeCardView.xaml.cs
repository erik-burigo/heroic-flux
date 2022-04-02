using HeroicFlux.Model;
using HeroicFlux.Model.Equipment;
using System;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for WeaponTypeCardView.xaml
    /// </summary>
    public partial class WeaponTypeCardView: UserControl
    {
        public WeaponTypeCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        private WeaponTypeCard _model;

        public WeaponTypeCard Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                UpdateFromModel();
            }
        }

        private void UpdatedAsVoid()
        {
            CardName.Content="<no weapon type>";
            Constraints.Text = "Any";
            Effects.Text = "...";
            Examples.Text = "Examples ...";
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            CardName.Content = _model.Name;
            Effects.SetText(_model.Effects);

            Constraints.Text = (_model.ItemSlotRequirement.HasValue ? _model.ItemSlotRequirement.Value.ToDescription() : "Any") +
                " " + _model.CategoryRequirement.ToString();

            var examples = "";
            if (!String.IsNullOrEmpty(_model.Examples1HandedText))
            {
                examples = "[hand] _" + _model.Examples1HandedText + "_";
            }
            if (!String.IsNullOrEmpty(_model.Examples2HandedText))
            {
                if (examples != "")
                    examples+="\n";
                examples += "[2hands] _" + _model.Examples2HandedText + "_";
            }
            if (!String.IsNullOrEmpty(examples))
                examples = "Examples:\n"+examples;
            Examples.SetText(examples);
        }
    }
}
