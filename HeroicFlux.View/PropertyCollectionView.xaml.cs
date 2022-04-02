using HeroicFlux.Model.Equipment;
using System.Windows;
using System.Windows.Controls;
using System;


namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for PropertyCollectionView.xaml
    /// </summary>
    public partial class PropertyCollectionView: UserControl
    {
        public PropertyCollectionView()
        {
            InitializeComponent();
            Width = PropertyTokenView.DefaultSize.Width + Margin.Left + Margin.Right;
            UpdateFromModel();
        }

        private PropertyCollection _model;

        public PropertyCollection Model
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
            PropertyContainer.Children.Clear();
        }

        private void UpdateFromModel()
        {
            if (_model == null)
            {
                UpdatedAsVoid();
                return;
            }

            BeginInit();
            PropertyContainer.Children.Clear();
            foreach (var propertyToken in _model)
            {
                PropertyTokenView view = new PropertyTokenView();
                view.Margin = new Thickness(0, 0, 0, 5);
                view.Model = propertyToken;
                PropertyContainer.Children.Add(view);
                view.RefreshedModel += delegate(object sender, EventArgs e){
                    if (view.Model == null)
                    {                        
                        PropertyContainer.Children.Remove(view);
                    }
                };
            }
            EndInit();
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            Model.FillUpTo(Model.Count + 1);
            UpdateFromModel();
        }      
    }
}
