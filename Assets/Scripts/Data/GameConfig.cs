using System;
using UnityEngine;

namespace CirclesWar.Data
{
    [Serializable]
    public class GameConfig
    {
        public float gameAreaWidth;
        public float gameAreaHeight;
        public int numUnitsToSpawn;
        public float unitSpawnDelay;
        public float unitSpawnMinRadius;
        public float unitSpawnMaxRadius;
        public float unitSpawnMinSpeed;
        public float unitSpawnMaxSpeed;
        public float unitDestroyRadius;
    }
}
