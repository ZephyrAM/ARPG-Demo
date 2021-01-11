using System;
using UnityEngine;
using UnityEngine.Events;

using ZAM.Core;
using ZAM.Stats;
using ZAM.Saving;

namespace ZAM.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    { 
        // Assigned Variables \\
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] AudioSource dieSound = null;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        // Setup Variables \\
        Animator healthAnimator;
        BaseStats healthBaseStats;
        Experience experience;

        // Adjustable Variables \\
        private float _currentHP = -99f;
        private float _timeSinceDamaged = Mathf.Infinity;

        //Basic Methods - Unity \\
        private void Awake()
        {
            healthAnimator = GetComponent<Animator>();
            healthBaseStats = GetComponent<BaseStats>();
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            if (GetCurrentHP() == -99f) { SetCurrentHP(GetMaxHP()); }
        }

        private void FixedUpdate()
        {
            _timeSinceDamaged += Time.deltaTime;
        }

        // Delegate Events \\
        public event Action onHealthChange;

        // Health Methods \\
        public float GetCurrentHP()
        {
            return _currentHP;
        }

        public float GetMaxHP()
        {
            return healthBaseStats.GetStat(Stat.MaxHealth);
        }

        public float GetHealthPercentage()
        {
            if (_currentHP == 0) { return 0; }
            return (_currentHP / GetMaxHP());
        }

        public void SetCurrentHP(float newHP)
        {
            _currentHP = newHP;
            onHealthChange();
        }

        public void RestoreMaxHP()
        {
            SetCurrentHP(GetMaxHP());
        }

        // public float PercentOfMaxHealth(float healthPercent)
        // {
        //     healthPercent /= 100;
        //     return (GetMaxHP() * healthPercent);
        // }

        public void TakeDamage(float value)
        {
            _timeSinceDamaged = 0;
            SetCurrentHP(Mathf.Max(_currentHP - value, 0));
            if (_currentHP == 0)
            {
                dieSound.Play();
                Die();
            }
            else 
            { 
                takeDamage.Invoke(value); 
            }
        }

        public float GetTimeSinceDamaged()
        {
            return _timeSinceDamaged;
        }
        
        private void Die()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            SetDeathTrigger();
        }

        public void TakeHealing(float value)
        {
            SetCurrentHP(Mathf.Min(_currentHP + value, GetMaxHP()));
        }

        public bool IsDead()
        {
            if (GetCurrentHP() == 0) 
            {
                return true; 
            }
            else return false;
        }
        
        public void Revive()
        {
            if (!IsDead())
            {
                SetReviveTrigger();
            }
        }

        // Animator Triggers \\
        private void SetDeathTrigger()
        {
            healthAnimator.ResetTrigger("Revive");
            healthAnimator.SetTrigger("Die");
        }

        private void SetReviveTrigger()
        {
            healthAnimator.ResetTrigger("Die");
            healthAnimator.SetTrigger("Revive");
        }

        // Save Component \\
        [System.Serializable]
        struct HealthSaveData
        {
            public float _currentHP;
        }

        public object CaptureState()
        {
            HealthSaveData data = new HealthSaveData();
            data._currentHP = _currentHP;
            return data;
        }

        public void RestoreState(object state)
        {
            HealthSaveData data = (HealthSaveData)state;
            _currentHP = data._currentHP;

            if (IsDead()) { Die(); }
            else { Revive(); }
        }
    }   
}

