using HeroicFlux.Model.Heroes;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for HeroPassiveSkillCardView.xaml
    /// </summary>
    public partial class HeroPassiveSkillCardView: UserControl
    {
        public HeroPassiveSkillCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        public HeroPassiveSkillCard Model
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
            CardEffect.Text = "";
            Background = null;
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            CardName.Content = _model.Name;
            CardEffect.SetText(_model.Effect);            
            Background = new SolidColorBrush(_model.Class.Color+Colors.Gray);
            CardEffect.BorderBrush = new SolidColorBrush(_model.Class.Color);

        }

        private HeroPassiveSkillCard _model;
    }
}