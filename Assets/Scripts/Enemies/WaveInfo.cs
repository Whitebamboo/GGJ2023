
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "WaveInfo", menuName = "ScriptableObjects/WaveInfo", order = 2)]
    public class WaveInfo : ScriptableObject
    {
        public List<SpawnInfo> spawnInfos;
        public float time;
    }
