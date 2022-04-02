using HeroicFlux.Model;
using HeroicFlux.Model.Collections;
using HeroicFlux.Model.Equipment;
using HeroicFlux.Model.Monsters;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace HeroicFlux.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var item in ListThings.Items)
            {
                var menuItem = item as MenuItem;
                if (menuItem!=null)
                {
                    menuItem.Click+=ListThings_Click;
                }
            }

            UpdateLevel();
            UpdateStatus();

            Doc.Document = Application.LoadComponent(new Uri("/TestGround.xaml", UriKind.Relative)) as FlowDocument;
        }



        public Game Game { get { return _game??(_game=new Game()); } }

        private void InitializeGame(Boolean anyway)
        {
            if (!_initialized || anyway)
            {
                Game.Initialize();
                drawnItems = new Set<BaseItemCard>(Game);
                drawnMonsters = new Set<MonsterCard>(Game);
                _initialized = true;
            }
        }

        private void ButtonDraw_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame(false);

            var baseItem = Game.ItemDeck.Draw(drawnItems, Game.ItemDiscard);
            var monster = Game.MonsterDeck.Draw(drawnMonsters, Game.MonsterDiscard);

            if (monster == null)
            {
                Message("No more monsters. Reshuffling...");
                Reshuffle();
                ButtonDraw_Click(sender, e);
                return;
            }

            if (baseItem==null)
            {
                Message("No more basic items. Reshuffling...");
                Reshuffle();
                ButtonDraw_Click(sender, e);
                return;
            }

            var currentProgressionCard = Game.ProgressionDeck.FirstOrDefault(p => p.Level == _currentLevel);
            var targetNumber = currentProgressionCard == null
                ? Game.Alea.Next(1, 3)+(baseItem.Slot== ItemSlot.TwoHands?1:0)
                : currentProgressionCard.RollItemDropPropertiesCount(baseItem.Slot== ItemSlot.TwoHands);

            monster.GainTraitsUpTo(currentProgressionCard.RollMonsterTrait());

            if (!baseItem.Materialize())
            {
                Message("No more weapon types. Reshuffling...");
                Reshuffle();
                ButtonDraw_Click(sender, e);
                return;
            }

            if (!baseItem.IdentifyOrUpgrade(targetNumber))
            {
                Message("Not enough property tokens. Reshuffling...");
                Reshuffle();
                ButtonDraw_Click(sender, e);
                return;
            }

            var pure =Game.Alea.Next(0, 3)==2;
            if (pure)
            {
                var firstCursedProperty = baseItem.Properties.FirstOrDefault(t => t.Type==PropertyType.Cursed && !t.IsFlipped);
                if (firstCursedProperty != null)
                    firstCursedProperty.IsFlipped = true;
            }

            ItemBlock.Model = baseItem;
            MonsterBlock.Model = monster;

            UpdateStatus();
        }

        private void ButtonReshuffle_Click(object sender, RoutedEventArgs e)
        {
            Reshuffle();
        }

        private void LevelMinus_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel-=1;
            if (_currentLevel <0)
                _currentLevel=0;
            UpdateLevel();
        }

        private void LevelPlus_Click(object sender, RoutedEventArgs e)
        {
            _currentLevel+=1;
            if (_currentLevel> 10)
                _currentLevel=10;
            UpdateLevel();
        }

        private void Message(String message)
        {
            if (!String.IsNullOrEmpty(message))
                MessageBox.Show(message);

            drawnItems.MoveTo(Game.ItemDiscard);
            Game.ItemDiscard.MoveTo(Game.ItemDeck);
            Game.ItemDeck.Shuffle();

            UpdateStatus();
        }

        private void Reshuffle()
        {
            drawnItems.MoveTo(Game.ItemDiscard);
            Game.Reset();
            UpdateStatus();
        }

        private void UpdateLevel()
        {
            LevelInfo.Text = "Current level: "+_currentLevel;
        }

        private void UpdateStatus()
        {
            DeckCount.Text = "Property Bag ("+Game.PropertyTokensBag.Count+")\n"
                +"Item Deck/Discard ("+Game.ItemDeck.Count+"/"+Game.ItemDiscard.Count+")\n"
                +"Weapon Deck/Discard ("+Game.WeaponTypeDeck.Count+"/"+Game.WeaponTypeDiscard.Count+")\n"
                +"Monster Deck/Discard ("+Game.MonsterDeck.Count+"/"+Game.MonsterDiscard.Count+")\n"
                +"Monster Traits Deck/Discard ("+Game.MonsterTraitDeck.Count+"/"+Game.MonsterTraitDiscard.Count+")";
        }

        private int _currentLevel;
        private Game _game;
        private bool _initialized;
        private Set<BaseItemCard> drawnItems;
        private Set<MonsterCard> drawnMonsters;


        private void PropertiesTab_GotFocus(object sender, RoutedEventArgs e)
        {
            //InitializeGame();

            //Container.BeginInit();
            //Container.Children.Clear();

            //foreach (var propertyToken in Game.Base.ItemProperties)
            //{
            //    var propertyTokenView = new PropertyTokenView();
            //    propertyTokenView.Model = propertyToken;
            //    propertyTokenView.Margin = new Thickness(2);
            //    propertyTokenView.MouseUp+=propertyTokenView_MouseUp;
            //    Container.Children.Add(propertyTokenView);
            //}

            //Container.EndInit();
        }

        void propertyTokenView_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var view = sender as PropertyTokenView;
            if (view == null)
                return;
            if (view.Model.IsFlippable)
            {
                view.Model.IsFlipped = !view.Model.IsFlipped;
                view.UpdateFromModel();
            }

        }
        void ListThings_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame(false);

            Container.BeginInit();
            Container.Children.Clear();

            if (sender == ListItemProperties)
                foreach (var item in Game.Base.ItemProperties)
                {
                    var view = new PropertyTokenView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    view.MouseUp+=propertyTokenView_MouseUp;
                    Container.Children.Add(view);
                }

            if (sender == ListBaseItems)
                foreach (var item in Game.Base.BaseItems)
                {
                    var view = new BaseItemCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListWeaponType)
                foreach (var item in Game.Base.WeaponTypes)
                {
                    var view = new WeaponTypeCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListMonsters)
                foreach (var item in Game.Base.Monsters)
                {
                    var view = new MonsterCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListMonsterTraits)
                foreach (var item in Game.Base.MonsterTraits)
                {
                    var view = new MonsterTraitCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListMonsterTactics)
                foreach (var item in Game.Base.MonsterTactics)
                {
                    var view = new MonsterTacticCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListHeroPassiveSkills)
                foreach (var item in Game.Base.HeroPassiveSkills)
                {
                    var view = new HeroPassiveSkillCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            if (sender == ListHeroActions)
                foreach (var item in Game.Base.HeroActions)
                {
                    var view = new HeroActionCardView();
                    view.Model = item;
                    view.Margin = new Thickness(2);
                    Container.Children.Add(view);
                }

            Container.EndInit();
        }

        private void ReloadData_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame(true);
        }

    }
}