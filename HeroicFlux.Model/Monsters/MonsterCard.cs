using HeroicFlux.Model.Collections;
using HeroicFlux.Model.Essence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Monsters
{
    public class MonsterCard: GameElement, ICloneable, IComparable<MonsterCard>, IAdoptingLocation
    {
        public MonsterCard(Game game)
            : base(game)
        {
        }

        public String Abilities { get; set; }
        public String SoloAction { get; set; }

        public IEnumerable<GameElement> AdoptedItems
        {
            get { return _traits; }
        }

        public String Attack { get; set; }

        public EssencePool BaseAttackPool { get; set; }

        public EssencePool BaseResistance { get; set; }

        public Int32 Defense { get; set; }

        public Int32 HitPoints { get; set; }

        public List<String> Keywords
        {
            get
            {
                return String.IsNullOrEmpty(KeywordsText)
                    ? new List<String>()
                    : KeywordsText.Split(',').Select(e => e.Trim()).ToList();
            }
        }

        public String KeywordsText { get; set; }

        public String Name { get; set; }

        public Int32 Number { get; set; }

        public FigueSize Size { get; set; }

        public static List<MonsterCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;

            var all=new List<MonsterCard>();

            foreach (var row in a.FromSpreadsheet("Monsters"))
            {
                var x = new MonsterCard(game);

                x.Name = a.Get(row, "Name");
                x.KeywordsText = a.Get(row, "Keywords");
                x.HitPoints = a.Get<Int32>(row, "HP");
                x.Size = a.Get(row, "Size").ToFigureSize();
                x.Number = a.Get<Int32>(row, "Number");
                x.Defense = a.Get<Int32>(row, "Defense");
                x.BaseResistance = EssencePool.Parse(a.Get(row, "Resistance"));
                x.BaseAttackPool = EssencePool.Parse(a.Get(row, "Attack Pool"));
                x.Attack = a.Get(row, "Attack");
                x.Abilities = a.Get(row, "Abilities");
                x.SoloAction = a.Get(row, "Solo Action");

                all.Add(x);
            }

            return all;
        }

        public IEnumerable<MonsterTraitCard> Traits { get { return _traits; } }

        public bool Abandon(GameElement item)
        {
            var trait = item as MonsterTraitCard;
            if (trait == null)
                return false;
            return _traits.Remove(trait);
        }

        public bool Adopt(GameElement item)
        {
            var trait = item as MonsterTraitCard;
            if (trait == null)
                return false;

            if (!MayAdopt(trait))
                return false;

            _traits.Add(trait);
            _traits.Sort();
            return true;
        }

        public object Clone()
        {
            return Copy();
        }

        public int CompareTo(MonsterCard other)
        {
            if (other == null)
                return -1;
            return Name.CompareTo(other.Name);
        }

        public MonsterCard Copy()
        {
            return new MonsterCard(Game)
            {
                Name = Name,
                KeywordsText = KeywordsText,
                HitPoints = HitPoints,
                Size = Size,
                Number = Number,
                Defense = Defense,
                BaseAttackPool = new EssencePool(BaseAttackPool),
                BaseResistance = new EssencePool(BaseResistance),
                Attack = Attack,
                Abilities = Abilities,
                SoloAction = SoloAction
            };
        }

        public Boolean GainTraitsUpTo(Int32 targetNumber)
        {
            var temporary = new Set<MonsterTraitCard>(Game);

            var ok = true;

            while (_traits.Count<targetNumber)
            {
                var trait = Game.MonsterTraitDeck.Draw(this, temporary);
                if (trait == null)
                {
                    /* { monster trait deck has been emptied }  */
                    /* { all unusable traits have been put into "temporary" } */
                    ok = false;
                    break;
                }
            }

            temporary.MoveTo(Game.MonsterTraitDiscard);
            _traits.Sort();

            return ok;
        }
        public bool MayAdopt(GameElement item)
        {
            var trait = item as MonsterTraitCard;
            if (trait == null)
                return false;

            if (_traits.Any(t => t.Name == trait.Name))
                return false;

            return trait.DoesSupportKeywords(Keywords);
        }

        private List<MonsterTraitCard> _traits = new List<MonsterTraitCard>();
    }
}