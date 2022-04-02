using HeroicFlux.Model.Equipment;
using System.Windows;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for ItemBlockView.xaml
    /// </summary>
    public partial class ItemBlockView: UserControl
    {
        public ItemBlockView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        private BaseItemCard _model;

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
            BaseItemView.Model = null;

            WeaponTypeView.Model = null;
            WeaponTypeView.Visibility = Visibility.Collapsed;

            PropertyCollectionView.Model = null;
            PropertyCollectionView.Visibility = Visibility.Collapsed;
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            BaseItemView.Model = _model;

            WeaponTypeView.Model = _model.WeaponType;
            WeaponTypeView.Visibility = _model.WeaponType == null
                ? Visibility.Collapsed
                : Visibility.Visible;


            PropertyCollectionView.Model = _model.Properties;
            PropertyCollectionView.Visibility = _model.Properties == null || _model.Properties.Count == 0
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
