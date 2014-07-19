using System;
using System.Collections.Generic;
using DarkEnergy.Characters;
using DarkEnergy.Characters.Hero;
using DarkEnergy.Combat;
using DarkEnergy.Scenes.World;

namespace DarkEnergy
{
    static class CombatSceneManager
    {
        public static bool InBattle
        {
            get
            {
                return (SceneManager.Current is Battle);
            }
        }

        public static Battle GetBattle()
        {
            if (InBattle) return (SceneManager.Current as Battle);
            else return null;
        }

        public static void Engage(WorldScene currentScene, int locationId, List<Character> enemies)
        {
            var groupA = new List<Character>() { new Hero() };
            groupA.AddRange(GameManager.Party.Members);

            var groupB = enemies;

            LoadingScreenManager.Show(() => { SceneManager.Play(new Battle(currentScene, locationId, groupA, groupB)); }, Resources.Strings.LoadingBattle);
        }

        public static void ExitBattle()
        {
            if (InBattle)
            {
                SceneManager.Play(new Scenes.BattleEndScreen(GetBattle()));
            }
            else
            {
                ExceptionManager.Log("Cannot exit battle because the character is not engaged in one.");
            }
        }
    }
}
