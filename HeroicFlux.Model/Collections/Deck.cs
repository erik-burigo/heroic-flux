using HeroicFlux.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Collections
{
    public class Deck<T>: Set<T> where T: GameElement
    {
        public Deck(Game game)
            : base(game)
        {
            Game = game;
        }

        public override String LocationName { get { return "Deck of "+typeof(T).Name; } }

        public T Top
        {
            get { return Peek(1).FirstOrDefault(); }
        }

        public IEnumerable<T> Peek(Int32 upTo)
        {
            var i = 0;
            while (i<upTo && i<_content.Count)
            {
                yield return _content[i++];
            }
        }

        public void Shuffle()
        {
            _content.Shuffle();
        }
    }
}