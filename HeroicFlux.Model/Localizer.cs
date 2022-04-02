using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model
{
    public static class Localizer
    {
        public static Boolean MoveTo(this IEnumerable<GameElement> elements, IAdoptingLocation location)
        {
            if (location == null)
                return false;

            if (!elements.All(item => location.MayAdopt(item)))
                return false;

            foreach (var item in elements.ToList())
            {
                var oldLocation = item.CurrentLocation;
                if (oldLocation == null || oldLocation.Abandon(item))
                {
                    item.CurrentLocation = location;
                }
                else
                {
                    throw new InvalidOperationException("Item " + item + " couldn't be abandoned from " + oldLocation);
                }
            }

            location.AdoptOrphans();

            return true;
        }

        public static void AdoptOrphans(this IAdoptingLocation location)
        {
            foreach (var orphan in location.Game.Base.GatherOrphansOf(location))
            {
                if (!location.Adopt(orphan))
                {
                    throw new InvalidOperationException("Orphan item " + orphan + " couldn't be adopted by " + location);
                }
            }
        }

        public static Boolean MoveTo(this GameElement item, IAdoptingLocation location)
        {
            return MoveTo(new[] { item }, location);
        }
    }
}