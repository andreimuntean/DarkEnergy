using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using DarkEnergy.Characters;

namespace DarkEnergy.Combat.Impact_Animations
{
    public class TintedUnit
    {
        protected readonly float Seconds = 0.4f;
        protected bool IsTinting;

        public bool Completed { get; protected set; }
        public Character Unit { get; protected set; }
        public Color3 Tint { get; protected set; }
        public Color3 CurrentTint { get { return Unit.ColorIntensity; } }
        public Color3 TargetTint { get { return Unit.Alive ? Color3.White : Color3.Black; } }

        public TintedUnit(Character unit, Color3 tint)
        {
            Unit = unit;
            Tint = tint;
            IsTinting = false;
            Completed = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Completed)
            {
                var modifier = gameTime.ElapsedGameTime.TotalSeconds / Seconds;

                if (IsTinting)
                {
                    if (Unit.ColorIntensity != TargetTint)
                    {
                        var red = CurrentTint.Red == TargetTint.Red ? CurrentTint.Red : (float)(CurrentTint.Red + (TargetTint.Red > CurrentTint.Red ? modifier : -modifier));
                        var green = CurrentTint.Green == TargetTint.Green ? CurrentTint.Green : (float)(CurrentTint.Green + (TargetTint.Green >= CurrentTint.Green ? modifier : -modifier));
                        var blue = CurrentTint.Blue == TargetTint.Blue ? CurrentTint.Blue : (float)(CurrentTint.Blue + (TargetTint.Blue >= CurrentTint.Blue ? modifier : -modifier));

                        Unit.ColorIntensity = new Color3(red, green, blue);
                    }
                    else Completed = true;
                }
                else
                {
                    Unit.ColorIntensity = Tint;
                    IsTinting = true;
                }
            }
        }
    }

    public class ShakedUnit
    {
        protected readonly Vector2 Shake = new Vector2(15, 0);
        protected readonly float Seconds = 0.2f;
        protected int Direction;
        protected bool IsShaking;

        public bool Completed { get; protected set; }
        public Character Unit { get; protected set; }
        public Vector2 InitialPosition { get; protected set; }

        public ShakedUnit(Character unit)
        {
            Unit = unit;
            InitialPosition = unit.Position;
            IsShaking = false;
            Completed = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Completed)
            {
                var modifier = new Vector2((float)(Shake.X * 3.0f / Seconds * gameTime.ElapsedGameTime.TotalSeconds), 0);

                if (IsShaking)
                {
                    if (Direction == -1)
                    {
                        var targetX = (InitialPosition - Shake).X;

                        if ((Unit.Position - modifier).X < targetX)
                        {
                            Unit.Position = InitialPosition - Shake;
                            Direction = 1;
                        }
                        else Unit.Position -= modifier;
                    }
                    else if (Direction == 1)
                    {
                        var targetX = InitialPosition.X;

                        if ((Unit.Position + modifier).X > targetX)
                        {
                            Unit.Position = InitialPosition;
                            Completed = true;
                        }
                        else Unit.Position += modifier;
                    }
                }
                else
                {
                    Unit.Position += Shake;
                    IsShaking = true;
                    Direction = -1;
                }
            }
        }
    }
}

namespace DarkEnergy.Combat
{
    using ShakedUnit = Impact_Animations.ShakedUnit;
    using TintedUnit = Impact_Animations.TintedUnit;

    public class ImpactAnimations
    {
        private Battle battle;

        protected List<ShakedUnit> ShakedUnits;
        protected List<TintedUnit> TintedUnits;

        public bool Completed { get; protected set; }

        public ImpactAnimations(Battle battle)
        {
            this.battle = battle;
            Completed = false;
        }

        public void Initialize(CombatAction action)
        {
            Completed = false;
            ShakedUnits = new List<ShakedUnit>();
            TintedUnits = new List<TintedUnit>();

            foreach (var data in GameManager.Combat.GetTurnData())
            {
                var unit = data.Character;

                if (data.AttackStatus == AttackStatus.Successful)
                {
                    Color3 tint = (data.TotalDamageReceived > data.TotalHealingReceived) ? new Color3(1, 0.3f, 0.3f) : new Color3(0.3f, 1, 0.3f);

                    TintedUnits.Add(new TintedUnit(unit, tint));

                    if (data.TotalDamageReceived > data.TotalHealingReceived)
                    {
                        ShakedUnits.Add(new ShakedUnit(unit));
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Completed)
            {
                var completed = true;

                if (ShakedUnits != null)
                {
                    ShakedUnits.ForEach(unit =>
                    {
                        unit.Update(gameTime);
                        if (completed && !unit.Completed) completed = false;
                    });
                }
                else completed = false;

                if (TintedUnits != null)
                {
                    TintedUnits.ForEach(unit =>
                    {
                        unit.Update(gameTime);
                        if (completed && !unit.Completed) completed = false;
                    });
                }
                else completed = false;

                Completed = completed;

                if (Completed)
                {
                    ShakedUnits = null;
                    TintedUnits = null;
                }
            }
        }
    }
}
