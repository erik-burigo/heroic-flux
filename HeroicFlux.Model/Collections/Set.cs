﻿using HeroicFlux.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Collections
{
    public class Set<T> : IAdoptingLocation, IEnumerable<T> where T : GameElement
    {
        public Set(Game game)
        {
            Game = game;
        }

        public IEnumerable<GameElement> AdoptedItems
        {
            get { return _content; }
        }

        public Int32 Count { get { return _content.Count; } }

        public virtual Game Game { get; protected set; }

        public Boolean IsEmpty { get { return _content.Count == 0; } }

        public virtual String LocationName { get { return "Set of " + typeof(T).Name; } }

        public Action<T> OnAdoption { get; set; }

        public bool Abandon(GameElement item)
        {
            _content.Remove(item as T);
            var stillPresent = _content.Contains(item as T);
            return !stillPresent;
        }

        public bool Adopt(GameElement item)
        {
            T t = item as T;
            if (t == null)
                return false;
            return Introduce(t);
        }

        public virtual void AfterRefill()
        {
            _content.Shuffle();
        }

        public virtual List<T> Draw(Int32 targetNumber, IAdoptingLocation target, IAdoptingLocation discard)
        {
            var list = new List<T>();
            while (targetNumber > 0)
            {
                var next = Draw(target, discard);
                if (next == null)
                    break;
                list.Add(next);
                targetNumber--;
            }
            return list;
        }

        public virtual T Draw(IAdoptingLocation target, IAdoptingLocation discard)
        {
            var firstTentative = DrawWithoutRefill(target, discard);
            if (firstTentative != null)
                return firstTentative;

            discard.AdoptedItems.MoveTo(this);
            AfterRefill();

            return DrawWithoutRefill(target, discard);
        }

        public virtual T DrawWithoutRefill(IAdoptingLocation target, IAdoptingLocation discard)
        {
            T item;

            Boolean mayAdopt;
            var safeGuard = Count;

            do
            {
                item = ReachForNext();
                if (item == null)
                    return null;
                safeGuard--;
                mayAdopt = target.MayAdopt(item);
                if (!mayAdopt)
                    item.MoveTo(discard);
            } while (!mayAdopt && safeGuard > 0);

            if (safeGuard <= 0)
                return null;

            item.MoveTo(target);
            return item;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        public virtual Boolean Introduce(T item)
        {
            if (MayAdopt(item))
            {
                _content.Add(item);
                if (OnAdoption != null)
                    OnAdoption(item);
                return true;
            }
            return false;
        }

        public virtual Boolean Introduce(IEnumerable<T> items)
        {
            if (items.Any(i => !MayAdopt(i)))
                return false;
            return items.MoveTo(this);
        }

        public virtual bool MayAdopt(GameElement item)
        {
            return true;
        }

        public virtual void Sort()
        {
            _content.Sort();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _content.GetEnumerator();
        }

        protected virtual T ReachForNext()
        {
            return IsEmpty ? null : _content[0];
        }

        protected List<T> _content = new List<T>();
    }
}