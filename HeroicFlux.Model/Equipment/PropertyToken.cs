using HeroicFlux.Model.Essence;
using System;
using System.Collections.Generic;

namespace HeroicFlux.Model.Equipment
{
    public class PropertyToken: GameElement, ICloneable, IComparable<PropertyToken>
    {
        public PropertyToken(Game game)
            : base(game)
        {
        }

        public override IAdoptingLocation CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
            set
            {
                _currentLocation = value;
                if (_currentLocation == Game.PropertyTokensBag)
                {
                    IsFlipped = false;
                }
            }
        }

        public ProperyTokenSide DisplayedSide { get { return !IsFlipped ? Obverse : Reverse; } }

        public EssencePool EssencePool { get { return IsFlipped ? Reverse.EssencePool : Obverse.EssencePool; } }

        public Boolean IsFlippable { get { return Type!=PropertyType.Triggered; } }

        public Boolean IsFlipped
        {
            get
            {
                return IsFlippable?_isFlipped:false;
            }
            set
            {
                if (!IsFlippable)
                    value = false;
                _isFlipped=value;
            }
        }

        public ModifierPosition ModifierPosition { get; set; }

        public string Name
        {
            get
            {
                return Type==PropertyType.Triggered
                    ? Obverse.Name
                    : Obverse.Name + "/" + Reverse.Name;
            }
        }

        public ProperyTokenSide Obverse
        {
            get { return _obverse ?? (_obverse = new ProperyTokenSide()); }
            private set { _obverse = value; }
        }

        public virtual Boolean Removable { get { return Type != PropertyType.Cursed || IsFlipped; } }

        public ProperyTokenSide Reverse
        {
            get { return _reverse ?? (_reverse = new ProperyTokenSide()); }
            private set { _reverse = value; }
        }

        public List<PropertyItemCategory> SupportedCategories
        {
            get { return _supportedCategories ?? (_supportedCategories = new List<PropertyItemCategory>()); }
            private set { _supportedCategories = value; }
        }

        public String TriggeredAbility { get; set; }

        public PropertyType Type { get; set; }

        public static List<PropertyToken> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all=new List<PropertyToken>();

            foreach (var row in a.FromSpreadsheet("PropertyTokens"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new PropertyToken(game);

                x.Type = a.Get(row, "Type").ToPropertyType();
                x.ModifierPosition = a.Get(row, "Position").ToModifierPosition();
                x.Obverse.Name = a.Get(row, "Obverse Name");
                x.Obverse.EssencePool = EssencePool.Parse(a.Get(row, "Obverse Essence"));
                x.Obverse.Effect = a.Get(row, "Obverse Effect");
                x.Reverse.Name = a.Get(row, "Reverse Name");
                x.Reverse.EssencePool = EssencePool.Parse(a.Get(row, "Reverse Essence"));
                x.Reverse.Effect = a.Get(row, "Reverse Effect");
                x.SupportedCategories= a.Get(row, "Categories").ToPropertyItemCategories();

                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }

                Console.WriteLine(copies);
            }
            return all;
        }

        public object Clone()
        {
            return Copy();
        }

        public int CompareTo(PropertyToken other)
        {
            if (other==null)
                return -1;

            var positionComparison = ModifierPosition.CompareTo(other.ModifierPosition);
            if (positionComparison != 0)
                return positionComparison;

            var essenceComparison = EssencePool.CompareTo(other.EssencePool);
            if (essenceComparison != 0)
                return essenceComparison;

            var typeComparison = Type.CompareTo(other.Type);
            if (typeComparison!=0)
                return typeComparison;

            var nameComparison = Name.CompareTo(other.Name);
            return nameComparison;
        }

        public PropertyToken Copy()
        {
            return new PropertyToken(Game)
            {
                Type = Type,
                ModifierPosition = ModifierPosition,
                Obverse = Obverse.Copy(),
                Reverse = Reverse.Copy(),
                SupportedCategories = new List<PropertyItemCategory>(SupportedCategories)
            };
        }

        public override string ToString()
        {
            return Id+": "+Name;
        }

        private IAdoptingLocation _currentLocation;
        private Boolean _isFlipped;
        private ProperyTokenSide _obverse;
        private ProperyTokenSide _reverse;
        private List<PropertyItemCategory> _supportedCategories;
    }
}