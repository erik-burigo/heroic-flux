using HeroicFlux.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Equipment
{
    public class WeaponTypeCard: GameElement
    {
        public WeaponTypeCard(Game game) : base(game) { }

        public BaseItemCategory CategoryRequirement { get; set; }

        public String Effects { get; set; }

        public List<String> Examples1Handed
        {
            get
            {
                return String.IsNullOrEmpty(Examples1HandedText)
                    ? new List<String>()
                    : Examples1HandedText.Split(',').Select(e => e.Trim()).ToList();
            }
        }

        public String Examples1HandedText { get; set; }

        public List<String> Examples2Handed
        {
            get
            {
                return String.IsNullOrEmpty(Examples2HandedText)
                    ? new List<String>()
                    : Examples2HandedText.Split(',').Select(e => e.Trim()).ToList();
            }
        }

        public String Examples2HandedText { get; set; }

        public ItemSlot? ItemSlotRequirement { get; set; }

        public String Name { get; set; }

        public static List<WeaponTypeCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all=new List<WeaponTypeCard>();

            foreach (var row in a.FromSpreadsheet("WeaponTypes"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new WeaponTypeCard(game);

                x.Name = a.Get(row, "Name");

                var requirements= a.Get(row, "Requirements").Split(' ');
                if (requirements.Length>0)
                    x.ItemSlotRequirement=requirements[0].ToItemSlot();
                if (requirements.Length>1)
                    x.CategoryRequirement=requirements[1].ToBaseItemCategory().Value;

                x.Effects = a.Get(row, "Effects");
                x.Examples1HandedText= a.Get(row, "Examples 1-Handed");
                x.Examples2HandedText= a.Get(row, "Examples 2-Handed");
                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }

                Console.WriteLine(copies);
            }
            return all;
        }

        public WeaponTypeCard Copy()
        {
            return new WeaponTypeCard(Game)
            {
                Name = Name,
                Effects = Effects,
                ItemSlotRequirement = ItemSlotRequirement,
                CategoryRequirement =CategoryRequirement,
                Examples1HandedText =Examples1HandedText,
                Examples2HandedText =Examples2HandedText,
            };
        }
    }
}