using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZAM.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        // Setup Variables \\
        BaseStats playerStats;
        Experience playerExperience;
        Text experiencePoints;

        // Base Methods - Unity \\
        private void Awake()
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            playerExperience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            experiencePoints = GetComponent<Text>();
        }

        private void OnEnable()
        {
            playerExperience.onExperienceGain += UpdateExperienceDisplay;
            playerStats.onLevelUp += UpdateExperienceDisplay;
        }

        private void OnDisable()
        {
            playerExperience.onExperienceGain -= UpdateExperienceDisplay;
            playerStats.onLevelUp -= UpdateExperienceDisplay;
        }

        private void Start()
        {
            UpdateExperienceDisplay();
        }

        // Delegate Methods \\
        private void UpdateExperienceDisplay()
        {
            experiencePoints.text = String.Format("Exp: {0:0}/{1:0}", playerExperience.GetCurrentExperience(), playerExperience.GetExpNextLevel());
        }
    }
}
