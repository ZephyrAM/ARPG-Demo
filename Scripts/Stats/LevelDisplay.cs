using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZAM.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        // Setup Variables \\
        BaseStats playerLevel;
        Text levelText;

        // Base Methods - Unity \\
        private void Awake()
        {
            playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            levelText = GetComponent<Text>();
        }

        private void OnEnable()
        {
            playerLevel.onLevelUp += UpdateLevelDisplay;
        }

        private void OnDisable()
        {
            playerLevel.onLevelUp -= UpdateLevelDisplay;
        }

        private void Start()
        {
            UpdateLevelDisplay();
        }

        // Delegate Methods \\
        public void UpdateLevelDisplay()
        {
            levelText.text = String.Format("Level: {0:0}", playerLevel.GetCurrentLevel());
        }
    }
}