using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Inventory;

namespace DarkEnergy.Characters.Hero
{
    public sealed class Hero : Character
    {
        public Features Features { get; set; }
        public Equipment Equipment { get; set; }
        
        public Vector2 GenderBodyScale
        {
            get { return Features.Gender == Gender.Male ? Vector2.One : new Vector2(0.7f, 1.0f);  }
        }

        public override Attributes Total
        {
            get { return Base + Equipment.Attributes; }
        }

        public override Vector2 Scale
        {
            get { return base.Scale * GenderBodyScale; }
            set { base.Scale = value; }
        }

        public Hero(Features features = null, Equipment equipment = null) : base(216, 350)
        {
            Equipment = (equipment == null) ? GameManager.Hero.Equipment : equipment;
            Features = (features == null) ? GameManager.Hero.Features : features;
            AbilitySets = GameManager.Hero.AbilitySets;
            Base = GameManager.Hero.Base;
            Level = GameManager.Hero.Level;
            Name = GameManager.Hero.Name;

            OffensiveElement = (Equipment.Weapon == null) ? Element.None : Equipment.Weapon.Element;
            DefensiveElement = (Equipment.Relic == null) ? Element.None : Equipment.Relic.Element;

            Equipment.Parent = this;
            Features.Parent = this;

            Loaded += AdjustObjects;
            PositionChanged += AdjustObjects;
            SizeChanged += AdjustObjects;
        }

        public bool IsHairVisible
        {
            get
            {
                return (Equipment.Head == null || Equipment.Head.HairVisible == true);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            Features.Initialize();
            Equipment.Initialize();
            State = CharacterState.Stand;

            if (CombatSceneManager.InBattle)
            {
                Scale = new Vector2(0.8f);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Path = @"Characters\Hero\Skin" + Features.Skin.ToString() + ".dds";
            Features.LoadContent(contentManager);
            Equipment.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Draw(Renderer renderer)
        {
            // Draw the back armor, if there is any.
            Equipment.StartDrawing(renderer);

            // Draw the body.
            base.Draw(renderer);

            // Draw its features.
            Features.FaceTexture.Draw(renderer);

            // Draw the equipment.
            Equipment.Draw(renderer);

            // Draw the overlapping features.
            if (IsHairVisible) Features.HairTexture.Draw(renderer);
            Features.HandOverWeaponTexture.Draw(renderer);

            // Finish drawing the equipment (like drawing the
            // hand armor above the hand drawn above the weapon).
            Equipment.FinishDrawing(renderer);
        }

        public override void Update(GameTime gameTime)
        {
            Equipment.Update(gameTime);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            Features.UnloadContent(contentManager);
            Equipment.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        private void AdjustObjects(object sender, EventArgs e)
        {
            Features.SetState(this);
            Equipment.SetState(this);
        }
    }
}
