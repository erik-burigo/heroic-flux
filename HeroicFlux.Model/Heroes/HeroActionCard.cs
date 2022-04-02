using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Heroes
{
    public class HeroActionCard: GameElement, ICloneable, IComparable<HeroActionCard>
    {
        public HeroActionCard(Game game)
            : base(game)
        {
        }

        public HeroClass Class { get; set; }

        public String EnergyCost { get; set; }

        public String Name { get; set; }

        public String Ongoing { get; set; }

        public String Played { get; set; }

        public String StartOfTurn { get; set; }
        public static List<HeroActionCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all=new List<HeroActionCard>();

            foreach (var row in a.FromSpreadsheet("HeroActions"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new HeroActionCard(game);

                x.Name = a.Get(row, "Name");
                x.Class = HeroClass.GetClasses().FirstOrDefault(c => c.Name == a.Get(row, "Class"));
                x.EnergyCost = a.Get(row, "Energy");
                x.Played = a.Get(row, "Played");
                x.Ongoing = a.Get(row, "Ongoing");
                x.StartOfTurn = a.Get(row, "Start of Turn");

                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }
            }
            return all;
        }

        public Object Clone()
        {
            return Copy();
        }

        public int CompareTo(HeroActionCard other)
        {
            var classComparison = Class.CompareTo(other.Class);
            if (classComparison != 0)
                return classComparison;
            return Name.CompareTo(other.Name);
        }

        public HeroActionCard Copy()
        {
            return new HeroActionCard(Game)
            {
                Name = Name,
                Class = Class,
                Played = Played,
                Ongoing = Ongoing,
                EnergyCost = EnergyCost,
                StartOfTurn = StartOfTurn
            };
        }
    }
}