using UnityEngine;

namespace ZAM.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        // Assigned Variables \\
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            instance.SetValue(damageAmount);
        }
    }
}
