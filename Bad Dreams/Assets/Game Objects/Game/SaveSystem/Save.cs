using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SaveSystem
{
    [Serializable]
    public class Save
    {
        public List<LevelData> Levels { get; private set; }
        public int Drops { get; set; }
        
        public Save()
        {
            Levels = new List<LevelData>();

            for (int i = 0; i < SaveManager.NumberOfLevels; ++i)
                Levels.Add(new LevelData());

            Levels[0].Locked = false;
            Drops = 0;
        }
    }
}
