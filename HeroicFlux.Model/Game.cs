﻿using HeroicFlux.Model.Collections;
using HeroicFlux.Model.Equipment;
using HeroicFlux.Model.Monsters;
using System;
using System.Linq;

namespace HeroicFlux.Model
{
    public class Game
    {
        public Game()
        {
            Base = new GamePropsBase() { Game=this };
        }

        public GamePropsBase Base { get; private set; }

        public Deck<BaseItemCard> ItemDeck => _itemDeck??(_itemDeck = new Deck<BaseItemCard>(this));

        public Deck<BaseItemCard> ItemDiscard => _itemDiscard??(_itemDiscard = new Deck<BaseItemCard>(this) { });

        public Deck<MonsterCard> MonsterDeck => _monsterDeck??(_monsterDeck = new Deck<MonsterCard>(this));

        public Deck<MonsterCard> MonsterDiscard => _monsterDiscard ?? (_monsterDiscard = new Deck<MonsterCard>(this));

        public Deck<MonsterTraitCard> MonsterTraitDeck => _monsterTraitDeck ?? (_monsterTraitDeck = new Deck<MonsterTraitCard>(this));

        public Deck<MonsterTraitCard> MonsterTraitDiscard => _monsterTraitDiscard ?? (_monsterTraitDiscard = new Deck<MonsterTraitCard>(this));

        public Deck<ProgressionCard> ProgressionDeck => _progressionDeck??(_progressionDeck = new Deck<ProgressionCard>(this));

        public Deck<ProgressionCard> ProgressionDiscard => _progressionDiscard??(_progressionDiscard = new Deck<ProgressionCard>(this) { });

        public Bag<PropertyToken> PropertyTokensBag => _propertyTokensBag??(_propertyTokensBag=new Bag<PropertyToken>(this));

        public Deck<WeaponTypeCard> WeaponTypeDeck => _weaponTypeDeck??(_weaponTypeDeck = new Deck<WeaponTypeCard>(this));

        public Deck<WeaponTypeCard> WeaponTypeDiscard => _weaponTypeDiscard??(_weaponTypeDiscard = new Deck<WeaponTypeCard>(this));

        public void Initialize()
        {
            Base.Initialize();
            Reset();
        }

        public void Reset()
        {
            PropertyTokensBag.Introduce(Base.ItemProperties);

            ItemDeck.Introduce(Base.BaseItems.Where(c => c.InDeck));
            ItemDeck.Shuffle();

            WeaponTypeDeck.Introduce(Base.WeaponTypes);
            WeaponTypeDeck.Shuffle();

            ProgressionDeck.Introduce(Base.Progressions);
            ProgressionDeck.Sort();

            MonsterDeck.Introduce(Base.Monsters);
            MonsterDeck.Shuffle();

            MonsterTraitDeck.Introduce(Base.MonsterTraits);
            MonsterTraitDeck.Shuffle();
        }

        public Random Alea = new Random();

        private Deck<BaseItemCard> _itemDeck;
        private Deck<BaseItemCard> _itemDiscard;
        private Deck<MonsterCard> _monsterDeck;
        private Deck<MonsterCard> _monsterDiscard;
        private Deck<MonsterTraitCard> _monsterTraitDeck;
        private Deck<MonsterTraitCard> _monsterTraitDiscard;
        private Deck<ProgressionCard> _progressionDeck;
        private Deck<ProgressionCard> _progressionDiscard;
        private Bag<PropertyToken> _propertyTokensBag;
        private Deck<WeaponTypeCard> _weaponTypeDeck;
        private Deck<WeaponTypeCard> _weaponTypeDiscard;
    }
}