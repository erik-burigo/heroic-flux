using HeroicFlux.Model.Collections;
using HeroicFlux.Model.Essence;
using HeroicFlux.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model.Equipment
{
    public class PropertyCollection: Set<PropertyToken>, IEnumerable<PropertyToken>
    {
        public PropertyCollection(Game game)
            : base(game)
        {
        }

        public EssencePool EssencePool
        {
            get
            {
                var pool = new EssencePool();
                foreach (var token in Content)
                {
                    pool.Add(token.EssencePool);
                }
                return pool;
            }
        }

        public Boolean FillUpTo(Int32 count)
        {
            var ok = true;
            var temporary = new Set<PropertyToken>(Game);

            while (Count<count)
            {
                var token = Game.PropertyTokensBag.Draw(this, temporary);
                if (token == null)
                {
                    // { property bag has been emptied } { all unusable tokens have been put into
                    // "temporary" }
                    ok = false;
                    break;
                }
                CheckAndPurgeTo(temporary);
            }

            temporary.MoveTo(Game.PropertyTokensBag);
            Sort();

            return ok;
        }

        public override bool MayAdopt(GameElement item)
        {
            if (!base.MayAdopt(item))
                return false;

            var token = item as PropertyToken;
            if (token == null)
                return false;

            var categoryMatches = !Category.HasValue || token.SupportedCategories.Contains(Category.Value);

            return categoryMatches;
        }

        public override void Sort()
        {
            Content.Sort();
        }

        private void CheckAndPurgeTo(IAdoptingLocation discardLocation)
        {
            var mustContinue = true;

            while (mustContinue)
            {
                mustContinue = false;
                var toRemove = new List<PropertyToken>();
                var homonimousGroups = from t in this group t by t.Name into g where g.Count()>1 select g;

                foreach (var group in homonimousGroups)
                {
                    var first = group.First();
                    var second = group.Skip(1).First();
                    var third = group.Skip(2).FirstOrDefault();

                    if (first.Type==PropertyType.Static && !first.IsFlipped)
                    {
                        first.IsFlipped=true;
                        mustContinue=true;
                    }
                    toRemove.Add(second);
                    if (third!=null)
                        mustContinue=true;
                }

                toRemove.MoveTo(discardLocation);
            }
        }
        public PropertyItemCategory? Category;
    }
}