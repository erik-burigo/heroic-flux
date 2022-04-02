using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Essence
{
    public class EssencePool: IEnumerable<Essence>, IComparable<EssencePool>
    {
        public EssencePool()
        {
        }

        public EssencePool(IEnumerable<Essence> essences)
        {
            Add(essences);
        }

        public Int32 Count { get { return _list.Count; } }

        public Boolean IsEmpty { get { return _list.Count == 0; } }

        public static EssencePool operator +(EssencePool a, EssencePool b)
        {
            var essence = new EssencePool();
            essence.Add(a);
            essence.Add(b);
            return essence;
        }

        public static EssencePool Parse(String text)
        {
            if (String.IsNullOrEmpty(text) || text.Length < 3)
                return new EssencePool();

            var head = text.Substring(0, 3).ToLowerInvariant().Trim();
            text = text.Substring(3);

            var pool = new EssencePool();
            foreach (var candidate in Enum.GetValues(typeof(Essence)).Cast<Essence>())
            {
                if (("<" + ToText(candidate) + ">").ToLowerInvariant().Equals(head))
                    pool.Add(candidate);
            }
            return pool + Parse(text);
        }

        public static String ToText(Essence essence)
        {
            switch (essence)
            {
                case Essence.Undecided:
                    return "?";

                case Essence.Physical:
                    return "P";

                case Essence.Cold:
                    return "C";

                case Essence.Fire:
                    return "F";

                case Essence.Lightning:
                    return "L";

                default:
                    throw new ArgumentException();
            }
        }

        public void Add(IEnumerable<Essence> essences)
        {
            _list.AddRange(essences);
            _list.Sort();
        }

        public void Add(Essence essence)
        {
            _list.Add(essence);
            _list.Sort();
        }

        public Int32 CompareTo(EssencePool other)
        {
            if (other == null)
                return -1;
            if (IsEmpty && other.IsEmpty)
                return 0;
            if (IsEmpty && !other.IsEmpty)
                return 1;
            if (!IsEmpty && other.IsEmpty)
                return -1;
            return _list[0].CompareTo(other._list[0]);
        }

        public Int32 GetEfficiencyAgainst(EssencePool defense)
        {
            var efficiency = 0;
            foreach (var essence in Enum.GetValues(typeof(Essence)).Cast<Essence>())
            {
                if (!this.Contains(essence))
                    continue;
                var attackSymbols = this.Count(e => e==essence);
                var defenseSymbols = defense.Count(e => e==essence);
                efficiency += attackSymbols-defenseSymbols;
            }
            return efficiency;
        }

        public IEnumerator<Essence> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override String ToString()
        {
            if (Count == 0)
                return "<no essence>";

            var text = "";
            foreach (var essence in this)
            {
                text += "<" + ToText(essence) + ">";
            }
            return text;
        }

        private List<Essence> _list = new List<Essence>();
    }
}