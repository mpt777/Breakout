using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Breakout.State
{
    public class Persistance
    {
        public bool saving = false;
        public bool loading = false;
        public void SaveGameState(List<GameState> myStates)
        {
            lock (this)
            {
                if (!saving)
                {
                    saving = true;
                    //
                    // Create something to save
                    FinalizeSaveAsync(myStates);
                }
            }
        }

        private async void FinalizeSaveAsync(List<GameState> states)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("HighScores.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(List<GameState>));
                                mySerializer.Serialize(fs, states);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                saving = false;
            });
        }

        /// <summary>
        /// Demonstrates how to deserialize an object from storage device
        /// </summary>
        public void LoadGameState()
        {
            lock (this)
            {
                if (!loading)
                {
                    loading = true;
                    FinalizeLoadAsync();
                }
            }
        }
        public List<GameState> states = new List<GameState>();

        private async void FinalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("HighScores.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("HighScores.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<GameState>));
                                    states = (List<GameState>)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                loading = false;
            });
        }
    }
}

