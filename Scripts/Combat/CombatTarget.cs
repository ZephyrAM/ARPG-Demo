using UnityEngine;

using ZAM.Attributes;

namespace ZAM.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        private Health health;

        public void Awake()
        {
            health = GetComponent<Health>();
        }
        
        public bool IsDead()
        {
            if (health.IsDead()) { return true; }
            else return false;
        }
    }
}

