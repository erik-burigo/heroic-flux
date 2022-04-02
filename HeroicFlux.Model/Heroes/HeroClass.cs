using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace HeroicFlux.Model.Heroes
{
    public class HeroClass: IComparable<HeroClass>
    {
        public System.Windows.Media.Color Color { get; set; }

        public String Name { get; set; }

        public static List<HeroClass> GetClasses()
        {
            if (_list == null)
            {
                _list = new List<HeroClass>();
                _list.Add(new HeroClass { Name = "None", Color=Colors.Gray });
                _list.Add(new HeroClass { Name = "Barbarian", Color=Colors.DarkRed });
                _list.Add(new HeroClass { Name = "Knight", Color=Colors.SteelBlue });
                _list.Add(new HeroClass { Name = "Hunter", Color=Colors.ForestGreen });
                _list.Add(new HeroClass { Name = "Wizard", Color=Colors.Purple });
            }
            return _list;
        }

        private static List<HeroClass> _list;

        public int CompareTo(HeroClass other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}