using HeroicFlux.Model.Heroes;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for HeroActionCardView.xaml
    /// </summary>
    public partial class HeroActionCardView: UserControl
    {
        public HeroActionCardView()
        {
            InitializeComponent();
            UpdateFromModel();
        }

        private HeroActionCard _model;

        public HeroActionCard Model
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
            Played.SetText("Played");
            StartOfTurn.SetText("StartOfTurn");
            Ongoing.SetText("Ongoing");
            EnergyCost.Content ="";
            EnergyCost.Visibility = System.Windows.Visibility.Collapsed;
            Background = null;
        }

        private static int Count(String text, String substring)
        {
            var i = text.IndexOf(substring);
            if (i<0)
                return 0;

            return 1 + Count(text.Substring(i+substring.Length), substring);
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            CardName.Content = _model.Name;

            var paragraphs =Count(_model.Played, ">>");
            Double fontSize;
            if (paragraphs == 1)
                fontSize = 18;
            else if (paragraphs == 2)
                fontSize= 16;
            else if (paragraphs == 3)
                fontSize = 14;
            else
                fontSize=12;
            Played.FontSize = fontSize;
            Played.SetText(_model.Played);

            if (!String.IsNullOrEmpty(_model.StartOfTurn))
            {
                StartOfTurn.SetText("*Start of Turn:*\n\n"+_model.StartOfTurn);
                StartOfTurn.Visibility = System.Windows.Visibility.Visible;
                StartOfTurn.BorderBrush = new SolidColorBrush(_model.Class.Color);
            }
            else
            {
                StartOfTurn.SetText("");
                StartOfTurn.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (!String.IsNullOrEmpty(_model.Ongoing))
            {
                Ongoing.SetText("*Ongoing:*\n\n"+_model.Ongoing);
                Ongoing.Visibility = System.Windows.Visibility.Visible;
                Ongoing.BorderBrush = new SolidColorBrush(_model.Class.Color);
            }
            else
            {
                Ongoing.SetText("");
                Ongoing.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (!String.IsNullOrEmpty(_model.EnergyCost))
            {
                EnergyCost.Content = _model.EnergyCost;
                EnergyCost.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                EnergyCost.Content ="";
                EnergyCost.Visibility = System.Windows.Visibility.Collapsed;
            }

            Background = new SolidColorBrush(_model.Class.Color + Colors.Gray);
        }
    }
}