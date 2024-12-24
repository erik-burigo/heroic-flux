using HeroicFlux.Model.Essence;
using System;
using System.Collections.Generic;

namespace HeroicFlux.Model.Equipment
{
    public class BaseItemCard: GameElement, ICloneable, IComparable<BaseItemCard>, IAdoptingLocation
    {
        public BaseItemCard(Game game)
            : base(game)
        {
        }

        public IEnumerable<GameElement> AdoptedItems
        {
            get
            {
                if (WeaponType!=null)
                    yield return WeaponType;
            }
        }

        public EssencePool BaseEssencePool
        {
            get => _baseEssencePool ?? (_baseEssencePool = new EssencePool());
            set => _baseEssencePool = value;
        }

        public BaseItemCategory Category
        {
            get => _category;
            set
            {
                if (WeaponType!=null)
                {
                    WeaponType.MoveTo(Game.WeaponTypeDeck);
                    Game.WeaponTypeDeck.Shuffle();
                }

                _category = value;

                Properties.Category = _category.ToPropertyCategory();
            }
        }

        public override IAdoptingLocation  CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                if (_currentLocation==Game.ItemDiscard)
                {
                    if (WeaponType!=null)
                        WeaponType.MoveTo(Game.WeaponTypeDiscard);
                    Properties.MoveTo(Game.PropertyTokensBag);
                }
            }
        }

        public String Effects { get; set; }

        public EssencePool EssencePool => BaseEssencePool + Properties.EssencePool;

        public Boolean InDeck { get; set; }

        public Boolean IsIdentified { get; set; }

        public Boolean IsMaterialized => !Category.IsWeapon() || WeaponType!=null;

        public String Name { get; set; }

        public String Icons { get; set; }

        public PropertyCollection Properties => _properties??(_properties=new PropertyCollection(Game));

        public String RandomWeaponName { get; private set; }

        public ItemSlot Slot { get; set; }

        public WeaponTypeCard WeaponType { get; private set; }

        public static List<BaseItemCard> Populate(Game game)
        {
            var a = game.Base.Ambassador;
            var all=new List<BaseItemCard>();

            foreach (var row in a.FromSpreadsheet("BaseItems"))
            {
                var copies = a.Get<Int32>(row, "Copies");
                if (copies<=0)
                    continue;

                var x = new BaseItemCard(game);

                x.Name = a.Get(row, "Name");
                x.Icons = a.Get(row, "Icons");

                BaseItemCategory category;
                if (Enum.TryParse<BaseItemCategory>(a.Get(row, "Category"), out category))
                    x.Category = category;

                x.BaseEssencePool =EssencePool.Parse(a.Get(row, "Essence"));
                x.Effects = a.Get(row, "Effects");
                x.Slot = a.Get(row, "Slot").ToItemSlot().Value;
                x.InDeck =a.Get(row, "Deck")!="no";

                for (Int32 c = 0; c<copies; c++)
                {
                    all.Add(x.Copy());
                }

                Console.WriteLine(copies);
            }
            return all;
        }

        public bool Abandon(GameElement item)
        {
            if (WeaponType!=null && WeaponType.Equals(item))
                WeaponType = null;
            return WeaponType==null;
        }

        public bool Adopt(GameElement item)
        {
            var weaponType = item as WeaponTypeCard;
            if (weaponType==null || WeaponType!=null)
                return false;

            WeaponType = weaponType;
            RandomWeaponName=  GenerateRandomWeaponTypeName();

            return true;
        }

        public object Clone()
        {
            return Copy();
        }

        public int CompareTo(BaseItemCard other)
        {
            if (other==null)
                return -1;

            var inDeckComparison = InDeck.CompareTo(other.InDeck);
            if (inDeckComparison != 0)
                return inDeckComparison;

            var categoryComparison = Category.CompareTo(other.Category);
            if (categoryComparison != 0)
                return categoryComparison;

            var slotComparison = Slot.CompareTo(other.Slot);
            if (slotComparison!=0)
                return slotComparison;

            var nameComparison = Name.CompareTo(other.Name);
            return nameComparison;
        }

        public BaseItemCard Copy()
        {
            return new BaseItemCard(Game)
            {
                Category = Category,
                Effects = Effects,
                BaseEssencePool = new EssencePool(BaseEssencePool),
                Name = Name,
                Icons = Icons,
                Slot = Slot,
                InDeck = InDeck
            };
        }

        public Boolean IdentifyOrUpgrade(Int32 targetNumber)
        {
            IsIdentified=true;
            return Properties.FillUpTo(targetNumber);
        }

        public Boolean Materialize()
        {
            if (Category.IsWeapon())
            {
                Game.WeaponTypeDeck.Draw(this, Game.WeaponTypeDiscard);
            }
            return IsMaterialized;
        }

        public bool MayAdopt(GameElement item)
        {
            var newWeaponType = item as WeaponTypeCard;

            if (newWeaponType == null)
                return false;

            if (WeaponType !=null)
                return false;

            if (Category.ToPropertyCategory()!=PropertyItemCategory.Weapon)
                return false;

            if (newWeaponType.CategoryRequirement != Category)
                return false;

            if (newWeaponType.ItemSlotRequirement.HasValue && newWeaponType.ItemSlotRequirement.Value != Slot)
                return false;

            return true;
        }

        public override String ToString()
        {
            return WeaponType!=null ? RandomWeaponName : Name;
        }

        private String GenerateRandomWeaponTypeName()
        {
            if (WeaponType==null)
                return null;

            if (Slot==ItemSlot.TwoHands)
            {
                if (WeaponType.Examples2Handed.Count==0)
                    return WeaponType.Name;

                var example = Game.Alea.Next(0, WeaponType.Examples2Handed.Count);
                return WeaponType.Examples2Handed[example];
            }
            else
            {
                if (WeaponType.Examples1Handed.Count==0)
                    return WeaponType.Name;

                var example = Game.Alea.Next(0, WeaponType.Examples1Handed.Count);
                return WeaponType.Examples1Handed[example];
            }
        }

        private EssencePool _baseEssencePool;
        private BaseItemCategory _category;
        private IAdoptingLocation _currentLocation;
        private PropertyCollection _properties;
    }
}