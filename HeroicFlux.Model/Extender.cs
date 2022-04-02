using HeroicFlux.Model.Equipment;
using System;
using System.Collections.Generic;

namespace HeroicFlux.Model
{
    public static class Extender
    {
        public static String ToDescription(this ItemSlot slot)
        {
            switch (slot)
            {
                case ItemSlot.OneHand:
                    return "One-Handed";
                case ItemSlot.TwoHands:
                    return "Two-Handed";
                case ItemSlot.Body:
                    return "Body";
                case ItemSlot.Jewel:
                    return "Jewel";
                default:
                    throw new ArgumentException();
            }
        }
        public static Boolean IsDefensive(this BaseItemCategory category)
        {
            switch (category)
            {
                case BaseItemCategory.Armor:
                case BaseItemCategory.Shield:
                    return true;

                default:
                    return false;
            }
        }

        public static Boolean IsWeapon(this BaseItemCategory category)
        {
            switch (category)
            {
                case BaseItemCategory.Melee:
                case BaseItemCategory.Ranged:
                case BaseItemCategory.Magic:
                    return true;

                default:
                    return false;
            }
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Alea.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static BaseItemCategory? ToBaseItemCategory(this String text)
        {
            text = text.ToLowerInvariant().Trim();
            switch (text)
            {
                case "melee":
                    return BaseItemCategory.Melee;

                case "ranged":
                    return BaseItemCategory.Ranged;

                case "magic":
                    return BaseItemCategory.Magic;

                case "armor":
                    return BaseItemCategory.Armor;

                case "shield":
                    return BaseItemCategory.Shield;

                case "jewel":
                    return BaseItemCategory.Jewel;

                default:
                    return null;
            }
        }

        public static FigueSize ToFigureSize(this String text)
        {
            text = text.ToLowerInvariant().Trim();
            switch (text)
            {
                case "small":
                    return FigueSize.Small;

                case "large":
                    return FigueSize.Large;

                default:
                    throw new ArgumentException();
            }
        }

        public static ItemSlot? ToItemSlot(this String text)
        {
            text = text.ToLowerInvariant().Trim();
            switch (text)
            {
                case "1 hand":
                case "one-handed":
                    return ItemSlot.OneHand;

                case "2 hands":
                case "two-handed":
                    return ItemSlot.TwoHands;

                case "body":
                    return ItemSlot.Body;

                case "jewel":
                    return ItemSlot.Jewel;

                default:
                    return null;
            }
        }

        public static ModifierPosition ToModifierPosition(this String text)
        {
            text = text.ToLowerInvariant().Trim();
            if (text=="prefix")
                return ModifierPosition.Prefix;
            if (text=="suffix")
                return ModifierPosition.Suffix;
            return ModifierPosition.Prefix;
        }

        public static PropertyItemCategory? ToPropertyCategory(this BaseItemCategory basicItemCategory)
        {
            switch (basicItemCategory)
            {
                case BaseItemCategory.Melee:
                case BaseItemCategory.Ranged:
                case BaseItemCategory.Magic:
                    return PropertyItemCategory.Weapon;

                case BaseItemCategory.Armor:
                case BaseItemCategory.Shield:
                    return PropertyItemCategory.Defensive;

                case BaseItemCategory.Jewel:
                    return PropertyItemCategory.Jewel;

                case BaseItemCategory.None:
                default:
                    return null;
            }
        }

        public static List<PropertyItemCategory> ToPropertyItemCategories(this String text)
        {
            var list = new List<PropertyItemCategory>();
            if (String.IsNullOrEmpty(text))
                return list;
            text = text.ToLowerInvariant();
            if (text.Contains("w"))
                list.Add(PropertyItemCategory.Weapon);
            if (text.Contains("d"))
                list.Add(PropertyItemCategory.Defensive);
            if (text.Contains("j"))
                list.Add(PropertyItemCategory.Jewel);

            return list;
        }

        public static PropertyType ToPropertyType(this String text)
        {
            text = text.ToLowerInvariant().Trim();
            if (text=="static")
                return PropertyType.Static;
            if (text=="triggered")
                return PropertyType.Triggered;
            if (text=="cursed")
                return PropertyType.Cursed;
            return PropertyType.Static;
        }

        private static Random Alea = new Random();
    }
}