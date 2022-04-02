using HeroicFlux.Model.Monsters;
using System;
using System.Windows.Controls;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for BaseItemCardView.xaml
    /// </summary>
    public partial class MonsterCardView: UserControl
    {
        public MonsterCardView()
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
            CardName.Content="<no item>";
            Abilities.Text = "...";
            SoloAction.Text = "...";
            Keywords.Text = "...";
            HitPoints.Content = "";
            Defense.Content = "";            
            AttackPool.Model = null;
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

            HitPoints.Content = _model.HitPoints.ToString();
            Defense.Content = _model.Defense.ToString();

            var keywords = _model.KeywordsText;
            keywords = keywords.Replace("Solo", "Solo ("+_model.Number+")");
            keywords = keywords.Replace("Squad", "Squad ("+_model.Number+")");
            Keywords.Text = keywords;

            if (!String.IsNullOrEmpty(_model.SoloAction))
            {
                SoloAction.SetText("*Solo Action:* "+_model.SoloAction);
                SoloAction.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                SoloAction.Visibility = System.Windows.Visibility.Collapsed;
            }

            Abilities.SetText(_model.Abilities);

            Defense.Content = _model.Defense.ToString();
            Attack.SetText(_model.Attack);
            AttackPool.Model = _model.BaseAttackPool;
            Resistance.Model = _model.BaseResistance;
        }
    }
}