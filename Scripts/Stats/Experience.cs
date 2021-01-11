using System;
using UnityEngine;

using ZAM.Saving;

namespace ZAM.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        // Assigned Variables \\
        [SerializeField] GameObject levelUpVFX = null;

        // Setup Variables \\
        BaseStats expBaseStats;

        // Adjustable Variables \\
        private float _experience = 0;

        // Base Methods - Unity \\
        private void Awake()
        {
            expBaseStats = GetComponent<BaseStats>();
        }

        // Delegate Events \\
        public event Action onExperienceGain;

        // Experience Methods \\
        public void AddExperience(float value)
        {
            _experience += value;
            onExperienceGain();
        }

        public float GetCurrentExperience()
        {
            return _experience;
        }

        public float GetExpNextLevel()
        {
            return expBaseStats.GetStat(Stat.ExperienceToLevel);
        }

        public float GetKillExperience()
        {
            return expBaseStats.GetStat(Stat.ExperienceWorth);
        }

        public void AwardKillExperience(GameObject target)
        {
            Experience targetExperience = target.GetComponent<Experience>();

            if (this.GetComponent<Experience>() == null || targetExperience == null) { return; }

            AddExperience(targetExperience.GetKillExperience());
        }

        // Level Up Methods \\
        public bool CheckLevelUp()
        {
            float loopTimes = 0;
            float expToNextLevel = expBaseStats.GetStat(Stat.ExperienceToLevel);

            if (GetCurrentExperience() < expToNextLevel) { return false; }

            while (GetCurrentExperience() >= expToNextLevel)
            {
                loopTimes++;
                DoLevelUp();
                expToNextLevel = expBaseStats.GetStat(Stat.ExperienceToLevel);
                if (loopTimes >= 99) { throw new Exception("Infinite LevelUp loop."); }
            }
            return true;
        }

        private void DoLevelUp()
        {
            expBaseStats.LevelUp();
            LevelUpVFX();
            Debug.Log("You have " + GetCurrentExperience() + " experience.");
            Debug.Log("You increased to level " + expBaseStats.GetCurrentLevel() + "!");
            Debug.Log("Exp to next level = " + expBaseStats.GetStat(Stat.ExperienceToLevel));
        }

        private void LevelUpVFX()
        {
            Instantiate(levelUpVFX, transform);
        }

        // Save Component \\
        [System.Serializable]
        struct ExperienceSaveData
        {
            public float _experience;
            public int _level;
        }

        public object CaptureState()
        {
            ExperienceSaveData data = new ExperienceSaveData();
            data._experience = _experience;
            data._level = expBaseStats.GetCurrentLevel();
            return data;
        }

        public void RestoreState(object state)
        {
            ExperienceSaveData data = (ExperienceSaveData)state;
            _experience = data._experience;
            expBaseStats.SetCurrentLevel(data._level);
        }
    }
}
