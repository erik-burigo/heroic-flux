using HeroicFlux.Model.Essence;
using System;

namespace HeroicFlux.Model.Equipment
{
    public class ProperyTokenSide: ICloneable
    {
        public String Effect { get; set; }

        public EssencePool EssencePool
        {
            get { return _essencePool ?? (_essencePool = new EssencePool()); }
            set { _essencePool = value; }
        }

        public Boolean IsTriggered { get; set; }

        public String Name { get; set; }

        public PropertyToken Token { get; set; }

        public object Clone()
        {
            return Copy();
        }

        public ProperyTokenSide Copy()
        {
            return new ProperyTokenSide
            {
                Name = Name,
                Effect = Effect,
                IsTriggered = IsTriggered,
                EssencePool = new EssencePool(EssencePool)
            };
        }

        private EssencePool _essencePool;
    }
}