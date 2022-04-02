using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroicFlux.Model
{
    public abstract class GameElement
    {
        private static Int32 _indexer = 0;

        public GameElement(Game game)
        {
            Game = game;
            Id = _indexer++;
        }
        public virtual IAdoptingLocation CurrentLocation { get; set; }

        public virtual Game Game { get; protected set; }

        public virtual Int32 Id { get; protected set; }
    }
}
