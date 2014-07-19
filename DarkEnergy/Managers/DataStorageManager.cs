using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace DarkEnergy
{
    static class DataStorageManager
    {
        private static IsolatedStorageSettings data { get { return IsolatedStorageSettings.ApplicationSettings; } }

        public static string RootDirectory { get { return "Character" + GameManager.SelectedCharacter.ToString() + "_"; } }

        /// <summary>
        /// Loads the specified data belonging to the specified character. If no character
        /// is specified, the method defaults to GameManager.SelectedCharacter.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="id">The id of the data.</param>
        /// <param name="characterIndex">The id of the character.</param>
        /// <returns></returns>
        public static T Load<T>(string id, int characterIndex = 0)
        {
            id = (characterIndex > 0)? "Character" + characterIndex.ToString() + "_" + id: RootDirectory + id;
            if (data.Contains(id))
                return (T)data[id];
            return default(T);
        }

        public static void Save(object value, string id, int characterIndex = 0)
        {
            id = (characterIndex > 0) ? "Character" + characterIndex.ToString() + "_" + id : RootDirectory + id;
            if (data.Contains(id))
                data[id] = value;
            else data.Add(id, value);
        }

        public static void Delete(string id, int characterIndex = 0)
        {
            id = (characterIndex > 0) ? "Character" + characterIndex.ToString() + "_" + id : RootDirectory + id;
            if (data.Contains(id))
                data.Remove(id);
        }

        /// <summary>
        /// Writes all data to the disk.
        /// </summary>
        public static void Flush()
        {
            SaveGameNotification.Show();
            data.Save();
            SaveGameNotification.Hide();
        }

        public static void DeleteCharacter(int characterIndex)
        {
            var idList = new List<string>();
            var characterTag = "Character" + characterIndex.ToString();
            
            foreach (var key in data.Keys)
            {
                var id = key.ToString();
                var thisCharacterTag = id.Substring(0, id.IndexOf('_'));
                
                if (thisCharacterTag == characterTag)
                {
                    idList.Add(id);
                }
            }

            foreach (var id in idList)
            {
                data.Remove(id);
            }
        }

        /// <summary>
        /// Saves the specified scene. The game will load to this point when it is restarted.
        /// </summary>
        /// <typeparam name="Scene">The scene.</typeparam>
        public static void SaveCharacterLocation<Scene>()
        {
            // Saves the name (type) of the scene.
            Save(typeof(Scene).AssemblyQualifiedName, "Scene");
        }

        /// <summary>
        /// Loads the scene
        /// </summary>
        /// <returns></returns>
        public static GameSystem LoadScene()
        {
            // Loads the name (type) of the scene.
            string sceneName = Load<string>("Scene");

            if (sceneName != null)
            {
                // Parses the string into a type.
                var scene = Type.GetType(sceneName);

                // Constructs an instance of the scene and returns it.
                return (GameSystem)Activator.CreateInstance(scene);
            }

            return null;
        }
    }
}
