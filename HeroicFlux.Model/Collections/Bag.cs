using HeroicFlux.Model;
using System;

namespace HeroicFlux.Model.Collections
{
    public class Bag<T> : Set<T> where T : GameElement
    {
        public Bag(Game game)
            : base(game)
        {
            Game = game;
        }

        public override String LocationName => "Bag of "+typeof(T).Name;

        public override void AfterRefill()
        {
            // Bags have randomization on draw, rather than on fill.
        }

        protected override T ReachForNext()
        {
            if (IsEmpty)
                return null;

            var item = Content[Game.Alea.Next(Content.Count)];
            return item;
        }
    }
}