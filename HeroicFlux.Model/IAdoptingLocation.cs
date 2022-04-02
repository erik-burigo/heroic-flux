using System;
using System.Collections.Generic;

namespace HeroicFlux.Model
{
    public interface IAdoptingLocation
    {
        Game Game { get; }

        IEnumerable<GameElement> AdoptedItems { get; }

        Boolean Abandon(GameElement item);

        Boolean Adopt(GameElement item);

        Boolean MayAdopt(GameElement item);
    }
}