using UnityEngine;
using System.Collections;

namespace Saving
{
    public static class Keys
    {
        public static string Locked { get; private set; }
        public static string Completed { get; private set; }
        public static string Collectibles { get; private set; }

        static Keys()
        {
            Locked = "Locked";
            Completed = "Completed";
            Collectibles = "Collectibles";
        }
    }
}