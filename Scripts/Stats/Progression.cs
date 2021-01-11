using System.Collections.Generic;
using UnityEngine;

namespace ZAM.Stats
{    
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/Build New Progression", order = 0)]
    public class Progression : ScriptableObject 
    {
        // Assigned Variables \\
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        // Setup Variables \\
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> progressionTable = null;

        private void BuildProgressionTable() 
        {
            if (progressionTable != null) return;

            progressionTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statTable[progressionStat.stat] = progressionStat.levels;
                }

                progressionTable[progressionClass.characterClass] = statTable;
            }
        }

        public float GetBaseStat(Stat stat, CharacterClass charClass, int level)
        {
            BuildProgressionTable();

            float[] levels = progressionTable[charClass][stat];

            if (levels.Length < level) { return 0; }

            return levels[level - 1];
        }

        // Save Component \\
        [System.Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
