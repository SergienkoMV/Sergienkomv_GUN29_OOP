using GamePrototype.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePrototype.Utils
{
    internal class Dragon : Unit
    {
        public Dragon(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {
        }

        public override uint GetUnitDamage() => BaseDamage;

        public override void HandleCombatComplete()
        {
            Health = MaxHealth;
        }

        protected override uint CalculateAppliedDamage(uint damage) => damage;
    }
}
