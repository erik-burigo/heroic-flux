using HeroicFlux.Model.Equipment;
using HeroicFlux.Model.Heroes;
using HeroicFlux.Model.Monsters;
using System.Collections.Generic;
using System.Linq;

namespace HeroicFlux.Model
{
    public class GamePropsBase: IEnumerable<GameElement>
    {
        public GoogleSpreadsheetAmbassador Ambassador => _ambassador??(_ambassador = new GoogleSpreadsheetAmbassador("HeroicFluxData"));

        public Game Game { get; set; }

        public IEnumerable<GameElement> GatherOrphansOf(IAdoptingLocation location)
        {
            return this.Except(location.AdoptedItems).Where(t => t.CurrentLocation==location).ToList();
        }

        public IEnumerator<GameElement> GetEnumerator()
        {
            foreach (var x in ItemProperties)
                yield return x;
            foreach (var x in BaseItems)
                yield return x;
            foreach (var x in WeaponTypes)
                yield return x;
            foreach (var x in Progressions)
                yield return x;
            foreach (var x in Monsters)
                yield return x;
            foreach (var x in MonsterTraits)
                yield return x;
            foreach (var x in MonsterTactics)
                yield return x;
            foreach (var x in HeroPassiveSkills)
                yield return x;
            foreach (var x in HeroActions)
                yield return x;
        }

        public void Initialize()
        {
            ItemProperties = PropertyToken.Populate(Game);
            BaseItems = BaseItemCard.Populate(Game);
            WeaponTypes = WeaponTypeCard.Populate(Game);
            Progressions = ProgressionCard.Populate(Game);
            Monsters = MonsterCard.Populate(Game);
            MonsterTraits = MonsterTraitCard.Populate(Game);
            MonsterTactics = MonsterTacticCard.Populate(Game);
            HeroPassiveSkills = HeroPassiveSkillCard.Populate(Game);
            HeroActions = HeroActionCard.Populate(Game);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<BaseItemCard> BaseItems = new List<BaseItemCard>();
        public List<PropertyToken> ItemProperties = new List<PropertyToken>();
        public List<WeaponTypeCard> WeaponTypes = new List<WeaponTypeCard>();
        public List<ProgressionCard> Progressions = new List<ProgressionCard>();
        public List<MonsterCard> Monsters = new List<MonsterCard>();
        public List<MonsterTraitCard> MonsterTraits = new List<MonsterTraitCard>();
        public List<MonsterTacticCard> MonsterTactics = new List<MonsterTacticCard>();
        public List<HeroPassiveSkillCard> HeroPassiveSkills = new List<HeroPassiveSkillCard>();
        public List<HeroActionCard> HeroActions = new List<HeroActionCard>();

        private GoogleSpreadsheetAmbassador _ambassador;
    }
}