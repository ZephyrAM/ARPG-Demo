using System;
using System.Collections.Generic;
using UnityEngine;

using ZAM.Core;
using ZAM.Movement;
using ZAM.Attributes;
using ZAM.Saving;
using ZAM.Stats;
using Random = UnityEngine.Random;

namespace ZAM.Combat
{
    public class Fight : MonoBehaviour, IAction, ISaveable, IModifierLookup
    {
        // Resource Folder \\
        private string weaponFolder = "Weapons/";

        // Assigned Values \\
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        [SerializeField] private float basePercentVariance = .10f;

        // Setup Variables\\
        CombatTarget self;
        Animator animator;
        Mover mover;

        BaseStats baseStats;
        Experience experience;
        Experience playerExperience;
        Health health;

        HealthBar healthDisplay;
        CombatTarget target = null;
        WeaponConfig currentWeaponConfig = null;
        Weapon currentWeapon = null;

        // Adjustable Variables \\
        private bool _attackComplete = false;
        private float _attackTimer = Mathf.Infinity;
        private bool _justAttack = false;

        private bool _clickTarget = false;
        private Vector3 _clickPosition;

        private bool _shouldAttack = false;

        // Set Variables \\
        private const string playerTag = "Player";

        // Base Methods - Unity \\
        private void Awake()
        {
            self = GetComponent<CombatTarget>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();

            baseStats = GetComponent<BaseStats>();
            experience = GetComponent<Experience>();
            health = GetComponent<Health>();
            playerExperience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();

            healthDisplay = GetComponentInChildren<HealthBar>();
        }

        private void OnEnable()
        {
            experience.onExperienceGain += GainedExperience;
        }

        private void OnDisable()
        {
            experience.onExperienceGain -= GainedExperience;
        }

        private void Start() 
        {
            if (currentWeapon == null) { EquipWeapon(defaultWeapon); }
        }

        private void Update()
        {
            GetComponent<Collider>().enabled = !self.IsDead();
            if (DoNotFight()) { return; }

            if (_justAttack) { return; }
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 0.8f);
            }
            else
            {
                FaceTarget();
                GetComponent<ActionScheduler>().StartAction(this);
            }
        }

        private void FixedUpdate()
        {
            AttackTimerZero();
            if (_attackTimer <= currentWeaponConfig.GetWeaponDelay()) { _attackTimer += Time.deltaTime; }
        }

        // Delegate Methods \\
        private void GainedExperience()
        {
            if (gameObject.tag == playerTag)
            {
                if (experience.CheckLevelUp()) { health.RestoreMaxHP(); }
            }
        }

        // Combat Configs \\
        public void EquipWeapon(WeaponConfig equipWeapon)
        {
            currentWeaponConfig = equipWeapon;
            currentWeapon = AttachWeapon(equipWeapon);
        }

        private Weapon AttachWeapon(WeaponConfig newWeapon)
        {
            return newWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public string GetWeaponResource(string nameInput)
        {
            return weaponFolder + nameInput;
        }

        public void SetAttackPoint(Vector3 mousePosition)
        {
            _clickPosition = mousePosition;
        }

        private Vector3 GetTargetPosition()
        {
            Vector3 targetPos;
            if (this.tag == playerTag)
            {
                targetPos = _clickPosition;
                targetPos.y = Terrain.activeTerrain.SampleHeight(_clickPosition);
            }
            else
            {
                targetPos = target.transform.position;
                targetPos.y = Terrain.activeTerrain.SampleHeight(target.transform.position);
            }

            return targetPos;
        }

        private void FaceTarget()
        {
            transform.LookAt(target.transform.position);
        }

        public void AttackAnyRange(bool justStop)
        {
            _justAttack = justStop;
        }

        public void SetClickTarget(bool justClick)
        {
            _clickTarget = justClick;
        }

        public void SetAttackTimer(float value)
        {
            _attackTimer = value;
        }

        private float GetTotalDamage()
        {
            int damageBase = (int)Mathf.Round(baseStats.GetStat(Stat.BaseDamage));
            int damageVariance = (int)Mathf.Max(1, Mathf.Round(damageBase * basePercentVariance));

            float finalDamage = Random.Range(damageBase - damageVariance, damageBase + damageVariance);
            return finalDamage;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.BaseDamage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.BaseDamage)
            {
                yield return currentWeaponConfig.GetPercentBonus();
            }
        }

        // Combat Checks \\
        public void StartAttack(CombatTarget combatTarget)
        {
            if (!TargetAvailable(combatTarget)) { return; }
            target = combatTarget;

            if (_attackTimer >= currentWeaponConfig.GetWeaponDelay())
            {
                if (IsInRange() || _justAttack)
                {
                    DoAttack();
                }
            }
        }

        public void StartAttack(Vector3 aimTarget)
        {
            _clickPosition = aimTarget;
            if (_attackTimer >= currentWeaponConfig.GetWeaponDelay()) { DoAttack(); }
        }

        private bool TargetAvailable(CombatTarget combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetHealth = combatTarget.GetComponent<Health>();
            return (targetHealth != null && !targetHealth.IsDead());
        }

        private bool DoNotFight()
        {
            if (this == null) { return true; }
            if (self.IsDead()) { return true; }
            if (target == null) { return true; }
            if (target.IsDead()) { return true; }

            return false;
        }

        // Combat Events \\
        private void DoAttack()
        {
            SetAttackTrigger();
            SetClickTarget(false);
        }

        public void Cancel()
        {
            SetStopAttackTrigger();
            target = null;
            _clickTarget = false;
        }

        private void AttackTimerZero()
        {
            if (_attackComplete)
            {
                animator.ResetTrigger("Attack");
                _attackTimer = 0f;
                _attackComplete = false;
            }
        }

        private void AttackComplete()
        {
            _attackComplete = true;
        }

        private void CombatLog()
        {
            if (target.IsDead()) { return; }

            float killExp = 0;
            if (target.tag != playerTag) { killExp = target.GetComponent<Experience>().GetKillExperience(); }

            if (!target.GetComponent<Fight>().HasTarget()) { target.GetComponent<Fight>().SetRetaliate(true); }
            target.GetComponent<Health>().TakeDamage(GetTotalDamage());

            Debug.Log(target.name + " takes " + GetTotalDamage() + " damage! " + target.GetComponent<Health>().GetCurrentHP() + " health left!");
            if (target.IsDead()) 
            {
                Debug.Log(target.name + " dies!");
                Debug.Log("~~ " + self.name + " gains " + killExp  + " experience! ~~");

                if (self.tag == playerTag) 
                { 
                    experience.AwardKillExperience(target.gameObject); // Only awards on killing blow. Improve.
                } 
            }
        }

        public void CombatLog(Collider other)
        {
            if (other.GetComponent<CombatTarget>().IsDead()) { return; }

            float killExp = 0;
            if (other.tag != playerTag) { killExp = other.GetComponent<Experience>().GetKillExperience(); }

            if (!other.GetComponent<Fight>().HasTarget()) { other.GetComponent<Fight>().SetRetaliate(true); }
            other.GetComponent<Health>().TakeDamage(GetTotalDamage());
            
            Debug.Log(other.name + " takes " + GetTotalDamage() + " damage! " + other.GetComponent<Health>().GetCurrentHP() + " health left!");
            if (other.GetComponent<CombatTarget>().IsDead()) 
            {
                Debug.Log(other.name + " dies!");
                Debug.Log("~~ " + self.name + " gains " + killExp + " experience! ~~");

                if (self.tag == playerTag) 
                { 
                    experience.AwardKillExperience(other.gameObject); // Only awards on killing blow. Improve.
                } 
            }
        }

        public bool ShouldRetaliate()
        {
            return _shouldAttack;
        }

        public void SetRetaliate(bool attackBool)
        {
            _shouldAttack = attackBool;
        }

        // State Checks \\
        public bool IsInRange()
        {
            if (target == null) { return false; }
            if (target == this) { return false; }

            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetWeaponRange();
        }

        public bool HasTarget()
        {
            return target;
        }

        public bool IsClickTarget()
        {
            return _clickTarget;
        }

        public CombatTarget GetTarget()
        {
            return target;
        }

        // Animation Events \\
        private void Hit()
        {
            if (self.IsDead()) { return; }

            AttackComplete();
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, (GetTargetPosition() + Vector3.up), gameObject);
                return;
            }
            else { if (!IsInRange()) { return; } }

            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (currentWeapon != null) { currentWeapon.OnHit(); }
            CombatLog();
        }

        private void Shoot()
        {
            Hit();
        }

        private void SetAttackTrigger()
        {
            mover.Cancel();
            animator.ResetTrigger("Stop Attack");
            animator.SetTrigger("Attack");
        }

        private void SetStopAttackTrigger()
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Stop Attack");
        }

        // Save Component \\
        // [System.Serializable]
        // struct WeaponSaveData
        // {
        //     public WeaponConfig currentWeaponConfig;
        // }

        public object CaptureState()
        {
            // WeaponSaveData data = new WeaponSaveData();
            // data.currentWeaponConfig = currentWeaponConfig;
            // return data;

            return currentWeaponConfig.name;
        }
        
        public void RestoreState(object state)
        {
            // WeaponSaveData data = (WeaponSaveData)state;
            // currentWeapon = data.currentWeaponConfig;

            string savedWeaponName = (string)state;
            WeaponConfig savedWeapon = UnityEngine.Resources.Load<WeaponConfig>(GetWeaponResource(savedWeaponName));
            EquipWeapon(savedWeapon);
        }
    }
}

