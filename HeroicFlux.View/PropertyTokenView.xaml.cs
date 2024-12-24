using HeroicFlux.Model.Equipment;
using HeroicFlux.Model.Essence;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for PropertyToken.xaml
    /// </summary>
    public partial class PropertyTokenView: UserControl
    {
        public PropertyTokenView()
        {
            InitializeComponent();
            _originalBorderThickness = BorderThickness;
            UpdateFromModel();
        }

        public PropertyToken Model
        {
            get => _model;
            set
            {
                _model = value;
                UpdateFromModel();

                if (RefreshedModel != null)
                    RefreshedModel(this, new EventArgs());
            }
        }

        public event EventHandler RefreshedModel;

        private void UpdatedAsVoid()
        {
            TextBox.Text = "<no text>";

            Effects.Text = "";
            Effects.Visibility = Visibility.Collapsed;

            CategoryWeapon.Visibility = Visibility.Hidden;
            CategoryDefensive.Visibility = Visibility.Hidden;
            CategoryJewel.Visibility = Visibility.Hidden;
            EssencePool.Model = null;
        }

        public void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            var titleColor =_model.IsFlipped
               ?new SolidColorBrush(Colors.Gold)
               : (_model.Type==PropertyType.Cursed
                    ? new SolidColorBrush(Colors.Red)
                    : new SolidColorBrush(Colors.Black));

            TextBox.Inlines.Clear();
            TextBox.Inlines.Add(new Run(_model.DisplayedSide.Name) { FontWeight = FontWeights.Bold, FontSize = 10, Foreground = titleColor });

            Effects.SetText(_model.DisplayedSide.Effect);
            Effects.Visibility = String.IsNullOrEmpty(Effects.Text)
               ? System.Windows.Visibility.Collapsed
               : System.Windows.Visibility.Visible;

            CategoryWeapon.Visibility = _model.SupportedCategories.Contains(PropertyItemCategory.Weapon) ? Visibility.Visible : Visibility.Hidden;
            CategoryDefensive.Visibility = _model.SupportedCategories.Contains(PropertyItemCategory.Defensive) ? Visibility.Visible : Visibility.Hidden;
            CategoryJewel.Visibility = _model.SupportedCategories.Contains(PropertyItemCategory.Jewel) ? Visibility.Visible : Visibility.Hidden;

            EssencePool.Model = _model.EssencePool;
        }

        public static Size DefaultSize = new Size(200, 50);

        private PropertyToken _model;

        private Thickness _originalBorderThickness;

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //const double delta = 1;
            //BorderThickness = new Thickness(_originalBorderThickness.Left+delta, _originalBorderThickness.Top+delta, _originalBorderThickness.Right+delta, _originalBorderThickness.Bottom+delta);            
            BorderBrush = Brushes.Blue;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BorderBrush = Brushes.Black;
            //   BorderThickness = _originalBorderThickness;
        }

        private void MenuItemDiscard_Click(object sender, RoutedEventArgs e)
        {
            Model.CurrentLocation = Model.Game.PropertyTokensBag;
            Model = null;
        }

        private void MenuItemFlip_Click(object sender, RoutedEventArgs e)
        {
            if (Model.IsFlippable)
            {
                Model.IsFlipped=!Model.IsFlipped;
                UpdateFromModel();
            }
        }
    }
}