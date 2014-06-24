using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Saving
{
    public static class SaveManager
    {
        public static int NumberOfLevels { get; set; }

        private static Save _CurrentSave;

        static SaveManager()
        {
            NumberOfLevels = 1;
        }

        public static void LoadGame()
        {
        }

        public static void NewGame()
        {
            CurrentSave = new Save();
        }

        public static Save CurrentSave
        {
            get
            {
                string save = PlayerPrefs.GetString("Save");
                if (!string.IsNullOrEmpty(save))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(save));
                    _CurrentSave = (Save)binaryFormatter.Deserialize(memoryStream);
                    return _CurrentSave;
                }
                return null;
            }
            set
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                binaryFormatter.Serialize(memoryStream, value);
                PlayerPrefs.SetString("Save", Convert.ToBase64String(memoryStream.GetBuffer()));
            }
        }
    }
}