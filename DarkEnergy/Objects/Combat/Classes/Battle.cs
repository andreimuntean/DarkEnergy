using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using DarkEnergy.Abilities;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Combat.Interface;
using DarkEnergy.Scenes.World;

namespace DarkEnergy.Combat
{
    public enum BattleState { Acting, Waiting, Ending }

    public class Battle : GameSystem, IScene
    {
        private bool paused;
        private TexturedElement background;
        private MessageBox exitMenu;
        private WorldScene worldScene;

        public event EventHandler RoundStarted;
        public event EventHandler RoundEnded;

        public BattleState State { get; protected set; }
        public bool Victory { get; protected set; }
        public int Rounds { get; protected set; }

        public AbilityManager AbilityManager { get; protected set; }
        public AnimationManager AnimationManager { get; protected set; }
        public AreaEffects AreaEffects { get; protected set; }
        public RewardManager RewardManager { get; protected set; }
        public TacticalMenu TacticalMenu { get; protected set; }
        public Units Units { get; protected set; }

        public Battle(WorldScene worldScene, int locationId, List<Character> groupA, List<Character> groupB)
        {
            this.worldScene = worldScene;
            
            background = new TexturedElement(1280, 720) { Parent = this, Path = @"World\Scenes\Combat\" + locationId.ToString() + ".dds" };
            exitMenu = new MessageBox() { Parent = this };

            Units = new Units(this, groupA, groupB);
            AbilityManager = new AbilityManager(this);
            AnimationManager = new AnimationManager(this);
            AreaEffects = new AreaEffects(this);
            RewardManager = new RewardManager(this);
            TacticalMenu = new TacticalMenu(this);
        }

        public WorldScene GetWorldScene()
        {
            return worldScene;
        }

        public void BeginRound()
        {
            State = BattleState.Acting;

            HideMenu();
            Units.CanSelectUnits = false;
            Units.ComputeAttackOrder();
            AbilityManager.Next();
            AnimationManager.Next();
            GameManager.Combat.Next();
            
            OnRoundStarted(this, EventArgs.Empty);
        }

        public void EndRound()
        {
            State = BattleState.Waiting;

            if (Units.CountAlive(Units.GroupA) == 0)
            {
                Victory = false;
                State = BattleState.Ending;
            }
            else if (Units.CountAlive(Units.GroupB) == 0)
            {
                Victory = true;
                State = BattleState.Ending;
            }
            else
            {
                AbilityManager.Tick();
                Units.CanSelectUnits = true;
                Units.InitializeSelections();
                Rounds++;
                ShowMenu();
            }

            OnRoundEnded(this, EventArgs.Empty);
        }

        public void HideMenu()
        {
            TacticalMenu.AbilityFrames.Visible = false;
        }

        public void ShowMenu()
        {
            TacticalMenu.AbilityFrames.Visible = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            paused = false;
            Rounds = 1;
            State = BattleState.Waiting;

            background.Initialize();
            exitMenu.Initialize();
            AreaEffects.Initialize();
            TacticalMenu.Initialize();
            Units.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            background.LoadContent(contentManager);
            exitMenu.LoadContent(contentManager);
            AreaEffects.LoadContent(contentManager);
            TacticalMenu.LoadContent(contentManager);
            Units.LoadContent(contentManager);
            base.LoadContent(contentManager);
        }

        public override void Update(GameTime gameTime)
        {
            exitMenu.Update(gameTime);

            if (exitMenu.Result == Resources.Strings.Yes)
            {
                exitMenu.Hide();
                CombatSceneManager.ExitBattle();
            }
            else if (exitMenu.Result == Resources.Strings.No)
            {
                exitMenu.Hide();
                paused = false;
            }

            if (!paused)
            {
                AbilityManager.Update(gameTime);
                AnimationManager.Update(gameTime);
                AreaEffects.Update(gameTime);
                TacticalMenu.Update(gameTime);
                Units.Update(gameTime);

                if (State == BattleState.Waiting)
                {
                    if (Units.Actions.Count == Units.CountAlive(Units.CurrentGroup))
                    {
                        // If every unit belonging to the player selected an ability ...
                        BeginRound();
                    }
                }
                else if (State == BattleState.Acting)
                {
                    // If the current actor finished casting and animating ...
                    if (AbilityManager.CastCompleted && AnimationManager.UnitAnimationEnded)
                    {
                        Units.Actions.RemoveAt(0);

                        // Actions involving fallen units are irrelevant, therefore they are removed.
                        // "while an action exists AND (character is dead OR (cannot target dead people AND target is dead))"
                        while (Units.Actions.Count > 0 && (!Units.Actions[0].Current.Alive || (!Units.Actions[0].Ability.TargetRestrictions.Dead && !Units.Actions[0].Target.Alive)))
                        {
                            Units.Actions.RemoveAt(0);
                        }

                        if (Units.Actions.Count > 0)
                        {
                            AbilityManager.Next();
                            AnimationManager.Next();
                            GameManager.Combat.Next();
                        }
                        else EndRound();
                    }
                }
                else if (State == BattleState.Ending)
                {
                    if (Victory)
                    {
                        RewardManager.Assign(Units.GroupA);
                    }

                    CombatSceneManager.ExitBattle();
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            background.Draw(renderer);
            AreaEffects.DrawBackLayer(renderer);
            Units.Draw(renderer);
            AreaEffects.DrawFrontLayer(renderer);
            TacticalMenu.Draw(renderer);
            exitMenu.Draw(renderer);
        }

        public override void UnloadContent(ContentManager contentManager)
        {
            background.UnloadContent(contentManager);
            exitMenu.UnloadContent(contentManager);
            AreaEffects.UnloadContent(contentManager);
            TacticalMenu.UnloadContent(contentManager);
            Units.UnloadContent(contentManager);
            base.UnloadContent(contentManager);
        }

        #region Events
        protected void OnRoundStarted(object sender, EventArgs e)
        {
            if (RoundStarted != null)
            {
                RoundStarted(this, e);
            }
        }

        protected void OnRoundEnded(object sender, EventArgs e)
        {
            if (RoundEnded != null)
            {
                RoundEnded(this, e);
            }
        }

        public void OnBackKeyPress()
        {
            if (State == BattleState.Waiting)
            {
                if (Units.Actions.Count > 0)
                {
                    // Remove the last action.
                    Units.Actions.RemoveAt(Units.Actions.Count - 1);

                    // Return to the previous character.
                    Units.SelectPreviousCharacter();
                    
                    // Stop exiting.
                    return;
                }
            }
            else paused = !paused;

            if (exitMenu.Visible)
            {
                exitMenu.Hide();
            }
            else
            {
                exitMenu.Show(Resources.Strings.CombatExitConfirmationLine1 + "\n" + Resources.Strings.CombatExitConfirmationLine2, new string[] { Resources.Strings.Yes, Resources.Strings.No });
            }
        }
        #endregion
    }
}
