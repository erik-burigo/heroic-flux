using HeroicFlux.Model.Monsters;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for MonsterTraitCardView.xaml
    /// </summary>
    public partial class MonsterTraitCardView: UserControl
    {
        public MonsterTraitCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        public MonsterTraitCard Model
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
            Constraints.Visibility = System.Windows.Visibility.Visible;
            Constraints.Text = "...";
            Traits.Text = "...";
            XP.Content = "XP";
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
            Traits.SetText(_model.Traits);

            Constraints.Visibility = _model.IsUniversal ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            Constraints.Text = _model.SupportedKeywordsText+ " only";

            XP.Content = _model.Xp+" XP";

            AttackPool.Model = _model.AttackPool;
            AttackPoolBox.Visibility = (_model.AttackPool != null && !_model.AttackPool.IsEmpty)
                   ? AttackPoolBox.Visibility = System.Windows.Visibility.Visible
                   : AttackPoolBox.Visibility = System.Windows.Visibility.Hidden;

            Resistance.Model = _model.Resistance;
            ResistanceBox.Visibility = (_model.Resistance != null && !_model.Resistance.IsEmpty)
                   ? ResistanceBox.Visibility = System.Windows.Visibility.Visible
                   : ResistanceBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private MonsterTraitCard _model;
    }
}