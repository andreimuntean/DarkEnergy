using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities.Visual;
using DarkEnergy.Combat.Interface;

namespace DarkEnergy.Combat
{
    public class AreaEffects : GameSystem
    {
        private Battle battle;
        private TexturedElement selfAura;
        private TexturedElement targetAura;
        private List<CombatText> floatingCombatText;

        public AbilityVisual AbilityVisual { get; set; }

        public AreaEffects(Battle battle)
        {
            Parent = this.battle = battle;
            selfAura = new TexturedElement(250, 100) { Parent = this, Path = @"Interface\World\Combat\AuraSelf.dds" };
            targetAura = new TexturedElement(250, 100) { Parent = this, Path = @"Interface\World\Combat\AuraTarget.dds" };
            floatingCombatText = new List<CombatText>();

            battle.RoundStarted += UpdateAuras;
            battle.RoundEnded += UpdateAuras;
            battle.Units.Loaded += UpdateAuras;
            battle.Units.PositionChanged += UpdateAuras;
            battle.Units.CurrentChanged += UpdateAuras;
            battle.Units.TargetChanged += UpdateAuras;
        }

        public void DisplayCombatText(List<CharacterCombatData> turnData)
        {
            turnData.ForEach(data =>
            {
                var combatText = new CombatText(data);
                combatText.Initialize();
                combatText.LoadContent(App.Game.Content);
                floatingCombatText.Add(combatText);
            });
        }

        public void DrawBackLayer(Renderer renderer)
        {
            selfAura.Draw(renderer);
            targetAura.Draw(renderer);
        }

        public void DrawFrontLayer(Renderer renderer)
        {
            if (floatingCombatText != null)
            {
                var drawOrder = floatingCombatText.OrderBy(combatText => combatText.Position.Y).ToList();
                drawOrder.ForEach(combatText => combatText.Draw(renderer));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            selfAura.Initialize();
            targetAura.Initialize();
            selfAura.Visible = false;
            targetAura.Visible = false;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            selfAura.LoadContent(contentManager);
            targetAura.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            if (AbilityVisual != null) AbilityVisual.Update(gameTime);

            if (floatingCombatText != null)
            {
                for (var i = 0; i < floatingCombatText.Count; ++i)
                {
                    floatingCombatText[i].Update(gameTime);
                    if (floatingCombatText[i].Visible == false)
                    {
                        floatingCombatText.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Calls DrawFrontLayer.
        /// </summary>
        /// <param name="renderer">The graphics renderer.</param>
        public override void Draw(Renderer renderer)
        {
            DrawFrontLayer(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            selfAura.UnloadContent(contentManager);
            targetAura.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void UpdateAuras(object sender, System.EventArgs e)
        {
            var current = battle.Units.Current;
            var target = battle.Units.Target;
            var canSelectUnits = battle.Units.CanSelectUnits;

            if (canSelectUnits && current != null && target != null)
            {
                var currentGroupCount = battle.Units.CurrentGroup.Count;
                var targetGroupCount = battle.Units.TargetGroup.Count;

                selfAura.Scale = new Vector2(current.Width / 250, 1);
                var x = current.X + (current.Width - selfAura.Width) / 2;
                var y = current.Y + current.Height - 65;
                selfAura.Position = new Vector2(x, y);
                selfAura.Visible = (currentGroupCount > 1 && current != target);

                targetAura.Scale = new Vector2(target.Width / 250, 1);
                x = target.X + (target.Width - targetAura.Width) / 2;
                y = target.Y + target.Height - 65;
                targetAura.Position = new Vector2(x, y);
                targetAura.Visible = (currentGroupCount > 1 || targetGroupCount > 1);
            }
            else
            {
                selfAura.Visible = false;
                targetAura.Visible = false;
            }
        }
        #endregion
    }
}
