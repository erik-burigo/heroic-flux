using HeroicFlux.Model.Monsters;
using System;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for HeroActionCardView.xaml
    /// </summary>
    public partial class MonsterTacticCardView: UserControl
    {
        public MonsterTacticCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        public MonsterTacticCard Model
        {
            get => _model;
            set
            {
                _model = value;
                UpdateFromModel();
            }
        }

        private void UpdatedAsVoid()
        {
            CardName.Content="<no item>";
            Section1.Visibility = System.Windows.Visibility.Visible;
            Section2.Visibility = System.Windows.Visibility.Visible;
            Section3.Visibility = System.Windows.Visibility.Visible;

            Section1.Text = "Section 1";
            Section2.Text = "Section 2";
            Section3.Text = "Section 3";

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

            if (_model.Keywords.Count>0)
            {
                Keywords.Visibility =  System.Windows.Visibility.Visible;
                Keywords.Text = _model.KeywordsText;
            }
            else
            {
                Keywords.Visibility =  System.Windows.Visibility.Collapsed;
            }

            if (!String.IsNullOrEmpty(_model.Section1))
            {
                Section1.Visibility =  System.Windows.Visibility.Visible;
                Section1.SetText(_model.Section1);
            }
            else
            {
                Section1.Visibility =  System.Windows.Visibility.Collapsed;
            }

            if (!String.IsNullOrEmpty(_model.Section2))
            {
                Section2.Visibility =  System.Windows.Visibility.Visible;
                Section2.SetText(_model.Section2);
            }
            else
            {
                Section2.Visibility =  System.Windows.Visibility.Collapsed;
            }

            if (!String.IsNullOrEmpty(_model.Section3))
            {
                Section3.Visibility =  System.Windows.Visibility.Visible;
                Section3.SetText(_model.Section3);
            }
            else
            {
                Section3.Visibility =  System.Windows.Visibility.Collapsed;
            }
        }

        private MonsterTacticCard _model;
    }
}