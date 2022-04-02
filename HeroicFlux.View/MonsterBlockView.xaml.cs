using HeroicFlux.Model.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for MonsterBlockView.xaml
    /// </summary>
    public partial class MonsterBlockView: UserControl
    {
        public MonsterBlockView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        private MonsterCard _model;

        public MonsterCard Model
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
            MonsterCard.Model = null;
            TraitContainer.Children.Clear();
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }            

            MonsterCard.Model = _model;

            TraitContainer.BeginInit();
            TraitContainer.Children.Clear();
            foreach (var trait in _model.Traits)
            {
                var view = new MonsterTraitCardView();
                view.Model = trait;
                view.Margin = new Thickness(5, 0, 0, 0);
                view.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                TraitContainer.Children.Add(view);
            }
            TraitContainer.EndInit();
        }
    }
}
