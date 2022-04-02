using System;
using System.Linq;
using System.Collections.Generic;

namespace HeroicFlux.Model.Heroes
{

    public class HeroPassiveSkillCard: GameElement, ICloneable, IComparable<HeroPassiveSkillCard>
    {
        public HeroPassiveSkillCard(Game game)
            : base(game)
        {

        }

        public HeroClass Class { get; set; }

        public String Effect { get; set; }

        public String Name { get; set; }

        public static List<HeroPassiveSkillCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;

            var all=new List<HeroPassiveSkillCard>();

            foreach (var row in a.FromSpreadsheet("HeroPassiveSkills"))
            {
                var x = new HeroPassiveSkillCard(game);

                x.Name = a.Get(row, "Name");
                x.Class = HeroClass.GetClasses().FirstOrDefault(c => c.Name == a.Get(row, "Class"));
                x.Effect= a.Get(row, "Effect");

                all.Add(x);
            }

            return all;
        }

        public Object Clone()
        {
            return Copy();
        }

        public int CompareTo(HeroPassiveSkillCard other)
        {
            var classComparison = Class.CompareTo(other.Class);
            if (classComparison != 0)
                return classComparison;
            return Name.CompareTo(other.Name);
        }

        public HeroPassiveSkillCard Copy()
        {
            return new HeroPassiveSkillCard(Game)
            {
                Name = Name,
                Class = Class,
                Effect = Effect
            };
        }
    }
}