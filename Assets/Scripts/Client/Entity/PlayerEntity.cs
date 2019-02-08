namespace Client.Entity
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity(
            string name, 
            string last_name, 
            short class_id,
            short race_id,
            short gender,
            short level,
            int ac,
            int atk,
            int str,
            int con,
            int dex,
            int agi,
            int _int,
            int wis,
            int chr,
            int mr,
            int pr          
            )
        {
            
        }
        
        
        
        private string Name;
        private string LastName;

        private short Class;
        private short Race;
        private short Gender;

        #region Calculated Attributes

        protected int ac;
        
        protected int atk;

        #endregion

        #region Base Attributes

        /// <summary>
        /// Strength - Melee ATK Bonus, Carrying Capacity
        /// </summary>
        protected int str;
        
        /// <summary>
        /// Constitution - HP Max/Regen, Stamina Max/Regen
        /// </summary>
        protected int con;
        
        /// <summary>
        /// Dexterity - Ranged ATK Bonus, Damange Bonus
        /// </summary>
        protected int dex;
        
        /// <summary>
        /// Agility - AC Bonus, Dodge Chance
        /// </summary>
        protected int agi;
        
        /// <summary>
        /// Intelligence Pure Caster MP Max, Spell Power
        /// </summary>
        protected int _int;
        
        /// <summary>
        /// Wisdom - Hybrid Caster MP Max, Spell Power
        /// </summary>
        protected int wis;
        
        /// <summary>
        /// Charisma - Merchant Buy/Sell Mob, Charm/Crowd Control Power
        /// </summary>
        protected int chr;
        
        /// <summary>
        /// Magic Resist
        /// </summary>
        protected int mr;
        
        /// <summary>
        /// Cold Resist
        /// </summary>
        protected int cr;
        
        /// <summary>
        /// Fire Resist
        /// </summary>
        protected int fr;
        
        /// <summary>
        /// Disease Resist
        /// </summary>
        protected int dr;
        
        /// <summary>
        /// Poison Resist
        /// </summary>
        protected int pr;
        
        #endregion



        protected int abjuration;
        protected int alcohol_tolerance;
        protected int alteration;
        protected int apply_poison;
        protected int archery;
        protected int backstab;
        protected int bash;
        protected int begging;
        protected int bind_wound;
        protected int block;
        protected int brewing;
        protected int shanneling;
        protected int conjuration;
        protected int defense;
        protected int disarm;
        protected int disarm_trap;
        protected int divination;
        protected int dodge;
        protected int double_attack;
        protected int dragon_punch;
        protected int dual_wield;
        protected int eagle_strike;
        protected int evocation;
        protected int feign_death;
        protected int fishing;


        public override bool Process()
        {
            throw new System.NotImplementedException();
        }

        public override bool Save()
        {
            throw new System.NotImplementedException();
        }
    }
}