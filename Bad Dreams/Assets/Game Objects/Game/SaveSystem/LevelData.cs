﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SaveSystem
{
    [Serializable]
    public class LevelData
    {
        public bool Locked { get; set; }
        public bool Completed { get; set; }
        public List<bool> Collectibles { get; set; }
        public int BestTime { get; set; }
        public int DropsCollected { get; set; }
        public int TotalDrops { get; set; }

        public LevelData()
        {
            Locked = true;
            Completed = false;
            Collectibles = new List<bool>();
            for (int i = 0; i < 3; ++i)
                Collectibles.Add(false);

            BestTime = 0;
            DropsCollected = 0;
            TotalDrops = 0;
        }
    }
}
