using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Monsters
{
    public class MonsterTacticCard: GameElement, ICloneable, IComparable<MonsterTacticCard>
    {
        public MonsterTacticCard(Game game)
            : base(game)
        {
        }

        public List<String> Keywords
        {
            get
            {
                return String.IsNullOrEmpty(KeywordsText)
                    ? new List<String>()
                    : KeywordsText.Split(',').Select(e => e.Trim().ToLowerInvariant()).Where(k => k!="any").ToList();
            }
        }

        public Boolean IsCompatibleWith(MonsterCard monster)
        {
            if (monster == null)
                return true;
            if (Keywords.Count == 0)
                return true;
            return Keywords.All(k => monster.Keywords.Contains(k));
        }

        public String KeywordsText { get; set; }

        public String Name { get; set; }

        public String Section1 { get; set; }

        public String Section2 { get; set; }

        public String Section3 { get; set; }

        public static List<MonsterTacticCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;

            var all = new List<MonsterTacticCard>();

            foreach (var row in a.FromSpreadsheet("MonsterTactics"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new MonsterTacticCard(game);

                x.Name = a.Get(row, "Name");
                x.KeywordsText = a.Get(row, "Supported Keywords");
                x.Section1 = a.Get(row, "Section 1");
                x.Section2 = a.Get(row, "Section 2");
                x.Section3 = a.Get(row, "Section 3");

                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }
            }

            return all;
        }

        public object Clone()
        {
            return Copy();
        }

        public int CompareTo(MonsterTacticCard other)
        {
            if (other == null)
                return -1;
            return Name.CompareTo(other.Name);
        }

        public MonsterTacticCard Copy()
        {
            return new MonsterTacticCard(Game)
            {
                Name = Name,
                KeywordsText = KeywordsText,
                Section1 = Section1,
                Section2 = Section2,
                Section3 = Section3,
            };
        }
    }
}