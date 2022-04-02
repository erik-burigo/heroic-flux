using HeroicFlux.Model;
using HeroicFlux.Model.Equipment;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for BaseItemCardView.xaml
    /// </summary>
    public partial class BaseItemCardView: UserControl
    {
        public BaseItemCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        public BaseItemCard Model
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
            CardName.Content="<no item>";
            Icons.Text = "";
            Effects.Text = "...";
            AttackPoolBox.Visibility = System.Windows.Visibility.Hidden;
            AttackPool.Model = null;
            ResistanceBox.Visibility = System.Windows.Visibility.Hidden;
            Resistance.Model = null;
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            CardName.Content = _model.Name;
            Icons.SetText(_model.Icons);
            Effects.SetText(_model.Effects);
            

            if (_model.Category.IsWeapon())
            {
                AttackPool.Model = _model.BaseEssencePool;
                AttackPoolBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                AttackPool.Model = null;
                AttackPoolBox.Visibility = System.Windows.Visibility.Hidden;
            }

            if (_model.Category.IsDefensive())
            {
                Resistance.Model = _model.BaseEssencePool;
                ResistanceBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Resistance.Model = null;
                ResistanceBox.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private BaseItemCard _model;
    }
}