using HeroicFlux.Model.Essence;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for EssencePoolView.xaml
    /// </summary>
    public partial class EssencePoolView: UserControl
    {
        public EssencePoolView()
        {
            InitializeComponent();
         //   UpdateFromModel();
        }

        public EssencePool Model
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
            Container.Children.Clear();
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            Container.BeginInit();

            Container.Children.Clear();

            foreach (var essence in _model)
            {
                var icon = new Image();
                icon.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/Essence"+essence+".png"));
                icon.Width = 20;
                icon.Height = 20;
                icon.Margin = new Thickness(2);
                Container.Children.Add(icon);
            }
            Container.EndInit();
        }

        private EssencePool _model;
    }
}