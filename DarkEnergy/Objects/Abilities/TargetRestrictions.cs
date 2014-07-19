using DarkEnergy.Characters;

namespace DarkEnergy.Abilities
{
    public struct TargetRestrictions
    {
        public bool Self { get; private set; }
        public bool Friendly;
        public bool Hostile;
        public bool Alive;
        public bool Dead;

        public TargetRestrictions(bool self, bool friendly, bool hostile, bool alive = true, bool dead = false) : this()
        {
            Self = self;
            Friendly = friendly;
            Hostile = hostile;
            Alive = alive;
            Dead = dead;
        }

        public bool Validate(Character current, Character target)
        {
            if (!current.Alive)
                return false;

            if (!Alive && target.Alive)
                return false;

            if (!Dead && !target.Alive)
                return false;

            if (!Self && current == target)
                return false;

            if (!Friendly)
            {
                if (current != target)
                {
                    if (!(current is Enemy) && !(target is Enemy))
                        return false;
                    if (current is Enemy && target is Enemy)
                        return false;
                }
            }

            if (!Hostile)
            {
                if (!(current is Enemy) && target is Enemy)
                    return false;
                if (current is Enemy && !(target is Enemy))
                    return false;
            }

            return true;
        }
    }
}
