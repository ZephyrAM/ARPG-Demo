using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZAM.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        // Setup Variables \\
        Health playerHealth;
        Text healthPoints;

        // Base Methods - Unity \\
        private void Awake()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthPoints = GetComponent<Text>();
        }

        private void OnEnable()
        {
            playerHealth.onHealthChange += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            playerHealth.onHealthChange -= UpdateHealthDisplay;
        }

        private void Start()
        {
            UpdateHealthDisplay();
        }

        // Delegate Methods \\
        private void UpdateHealthDisplay()
        {
            if (playerHealth.GetCurrentHP() == -99) 
            {
                healthPoints.text = String.Format("Health: {0:0}/{1:0}", playerHealth.GetMaxHP(), playerHealth.GetMaxHP());
            }
            else 
            {
                healthPoints.text = String.Format("Health: {0:0}/{1:0}", playerHealth.GetCurrentHP(), playerHealth.GetMaxHP());
            }
        }
    }
}
