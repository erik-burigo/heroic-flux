using System;
using System.Collections.Generic;

namespace HeroicFlux.Model
{
    public class ProgressionCard: GameElement, IComparable<ProgressionCard>
    {
        public ProgressionCard(Game game)
            : base(game)
        {
        }

        public Int32[] ItemDrop { get; set; }

        public Int32 Level { get; set; }

        public Int32 MonsterTraitsOn1To3 { get; set; }

        public Int32 MonsterTraitsOn4or5 { get; set; }

        public Int32 MonsterTraitsOn6 { get; set; }

        public Int32 TwoSlotBonus { get; set; }

        public Int32 XpPerHero { get; set; }

        public static List<ProgressionCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all = new List<ProgressionCard>();

            foreach (var row in a.FromSpreadsheet("ProgressionCards"))
            {
                var x = new ProgressionCard(game);

                x.Level = a.Get<Int32>(row, "Level");
                x.XpPerHero = a.Get<Int32>(row, "Xp");
                x.MonsterTraitsOn1To3 = a.Get<Int32>(row, "Monster Traits on 1-3");
                x.MonsterTraitsOn4or5 = a.Get<Int32>(row, "Monster Traits on 4 or 5");
                x.MonsterTraitsOn6 = a.Get<Int32>(row, "Monster Traits on 6");

                x.ItemDrop = new Int32[6];
                for (var i = 0; i < 6; i++)
                {
                    x.ItemDrop[i] = a.Get<Int32>(row, "Item Drop on " + (i + 1));
                }

                x.TwoSlotBonus = a.Get<Int32>(row, "Two-Slot Bonus");

                all.Add(x);
            }
            return all;
        }

        public int CompareTo(ProgressionCard other)
        {
            if (other == null)
                return -1;
            return Level.CompareTo(other.Level);
        }

        public Int32 RollItemDropPropertiesCount(Boolean twoHanded)
        {
            var d6 = Game.Alea.Next(6)+1;
            var baseSlots = ItemDrop[d6-1];
            return baseSlots + (twoHanded ? TwoSlotBonus : 0);
        }

        public Int32 RollMonsterTrait()
        {
            var d6 = Game.Alea.Next(6) + 1;

            switch (d6)
            {
                case 1:
                case 2:
                case 3:
                    return MonsterTraitsOn1To3;
                case 4:
                case 5:
                    return MonsterTraitsOn4or5;
                case 6:
                    return MonsterTraitsOn6;
                default:
                    throw new ArgumentException();
            }
        }
    }
}