using UnityEngine;

namespace ZAM.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        // Setup Variables \\
        Health health = null;
        Canvas healthBarImage = null;
        [SerializeField] RectTransform healthBarTransform = null;

        // Text healthText;

        // Base Methods - Unity \\
        private void Awake()
        {
            health = GetComponentInParent<Health>();
            healthBarImage = GetComponentInChildren<Canvas>();

            // healthText = GetComponent<Text>();
        }

        private void OnEnable()
        {
            health.onHealthChange += UpdateHealthDisplay;
        }

        private void OnDisable()
        {
            health.onHealthChange -= UpdateHealthDisplay;
        }

        private void Start()
        {
            UpdateHealthDisplay();
        }

        private void Update()
        {
            healthBarImage.enabled = IsBarVisible();
        }

        // Delegate Methods \\
        private void UpdateHealthDisplay()
        {
            if (health.GetCurrentHP() == -99) { health.SetCurrentHP(health.GetMaxHP()); }
            healthBarTransform.localScale = new Vector3(health.GetHealthPercentage(), 1, 1);

            // healthText.text = String.Format("{0:0}/{1:0}", health.GetCurrentHP(), health.GetMaxHP());
        }            

        // State Checks \\
        public bool IsBarVisible()
        {
            if (health.GetTimeSinceDamaged() < 10 && !health.IsDead()) { return true; }
            else { return false; }
        }
    }
}
