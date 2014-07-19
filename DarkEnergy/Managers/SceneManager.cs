namespace DarkEnergy
{
    static class SceneManager
    {   
        private static DarkEnergyGame Game { get { return App.Game; } }

        public static GameSystem Current { get { return Game.Scene; } }
        
        public static void Play(GameSystem scene)
        {
            bool onFirstRun = Game.Scene == null;
            
            if (onFirstRun == false)
            {
                Game.Pause();
                Game.Content.Unload();
            }

            Game.Scene = scene;
            scene.Initialize();

            if (onFirstRun == false)
            {
                // The Game loads everything on the first run, which
                // is why loading the scene again is unnecessary.
                Game.NotificationManager.LoadContent(Game.Content);
                scene.LoadContent(Game.Content);

                if (scene is ILoadable)
                {
                    (scene as ILoadable).LoadData();
                }

                Game.Resume();
            }
        }

        public static void GoBack()
        {
            if (Game.Scene != null)
            {
                if (Game.Scene is IScene)
                {
                    (Game.Scene as IScene).OnBackKeyPress();
                }
                else
                {
                    ExceptionManager.Log("The current scene of type \'" + Current.GetType().Name + "\' does not implement the IScene interface.");
                }
            }
        }
    }
}
