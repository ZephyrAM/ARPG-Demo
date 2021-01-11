using System;
using UnityEngine;

namespace ZAM.Stats
{
    public class BaseStats : MonoBehaviour
    {
        // Assigned Variables \\
        [Range(1, 99)]
        [SerializeField] private int experienceLevel = 1;
        [SerializeField] CharacterClass charClass;
        [SerializeField] Progression progression = null;
        [SerializeField] bool shouldUseAddModifiers = false;
        [SerializeField] bool shouldUseBonusModifiers = false;

        // Setup Variables \\
        Experience experience;

        // Delegate Events \\
        public event Action onLevelUp; 

        // Stat Methods \\
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercetageModifier(stat));
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetBaseStat(stat, charClass, experienceLevel);
        }

        public int GetCurrentLevel()
        {
            return experienceLevel;
        }

        public void SetCurrentLevel(int newLevel)
        {
            experienceLevel = newLevel;
        }

        public void LevelUp()
        {
            experienceLevel++;
            onLevelUp();
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseAddModifiers) { return 0; }
            float total = 0;
            foreach (IModifierLookup source in GetComponents<IModifierLookup>())
            {
                foreach (float modifier in source.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercetageModifier(Stat stat)
        {
            if (!shouldUseBonusModifiers) { return 0; }
            float total = 0;
            foreach (IModifierLookup source in GetComponents<IModifierLookup>())
            {
                foreach (float modifier in source.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return (total / 100);
        }
    }
}
