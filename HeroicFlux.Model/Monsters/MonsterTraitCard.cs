using HeroicFlux.Model.Essence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Monsters
{
    public class MonsterTraitCard: GameElement, ICloneable, IComparable<MonsterTraitCard>
    {
        public MonsterTraitCard(Game game)
            : base(game)
        {
        }

        public EssencePool AttackPool { get; set; }

        public Boolean IsUniversal
        {
            get
            {
                if (String.IsNullOrWhiteSpace(SupportedKeywordsText))
                    return true;
                var x = SupportedKeywordsText.Trim().ToLowerInvariant();
                return x=="any" || x=="all";
            }
        }

        public String Name { get; set; }

        public EssencePool Resistance { get; set; }

        public String SupportedKeywordsText { get; set; }

        public String Traits { get; set; }

        public Int32 Xp { get; set; }

        public static List<MonsterTraitCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all=new List<MonsterTraitCard>();

            foreach (var row in a.FromSpreadsheet("MonsterTraits"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new MonsterTraitCard(game);

                x.Name = a.Get(row, "Name");
                x.SupportedKeywordsText = a.Get(row, "Supported Keywords");
                x.Resistance = EssencePool.Parse(a.Get(row, "Resistance"));
                x.AttackPool = EssencePool.Parse(a.Get(row, "Attack Pool"));
                x.Traits = a.Get(row, "Traits");
                x.Xp = a.Get<Int32>(row, "Xp");

                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }

                Console.WriteLine(copies);
            }
            return all;
        }

        public Object Clone()
        {
            return Copy();
        }

        public Int32 CompareTo(MonsterTraitCard other)
        {
            if (other==null)
                return -1;
            return Name.CompareTo(other.Name);
        }

        public MonsterTraitCard Copy()
        {
            return new MonsterTraitCard(Game)
            {
                Name = Name,
                SupportedKeywordsText = SupportedKeywordsText,
                Resistance = new EssencePool(Resistance),
                AttackPool = new EssencePool(AttackPool),
                Traits = Traits,
                Xp = Xp,
            };
        }
        public Boolean DoesSupportKeywords(IEnumerable<String> keywords)
        {
            if (IsUniversal)
                return true;

            var parts = SupportedKeywordsText.ToLowerInvariant().Split(',').Select(p => p.Trim());

            var keywordList=keywords.Select(k => k.ToLowerInvariant().Trim()).ToList();
            foreach (var part in parts)
            {
                if (!keywordList.Contains(part))
                    return false;
            }

            return true;
        }
    }
}