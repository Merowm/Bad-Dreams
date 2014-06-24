using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Saving
{
    [Serializable]
    public class LevelData
    {
        public bool Locked { get; set; }
        public bool Completed { get; set; }

        private int _Collectibles { get; set; }

        public LevelData()
        {
            Locked = true;
            Completed = false;
            Collectibles = 0;
        }

        public int Collectibles
        {
            get { return _Collectibles; }
            set { _Collectibles = Mathf.Clamp(value, 0, 3); }
        }
    }
}
