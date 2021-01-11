using UnityEngine;

namespace ZAM.Combat
{    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] Projectile projectile = null;

        [SerializeField] private float _weaponRange = 1f;
        [SerializeField] private float _weaponDelay = 1f;
        [SerializeField] private float _weaponDamage = 3f;
        [SerializeField] private float _percentBonus = 0;
        [SerializeField] private bool _isRightHanded = true;

        //[SerializeField] AudioSource attackSound = null;

        const string weaponLabel = "Weapon";
        GameObject weaponWielder = null;

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon newWeapon = null;
            if (weaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                newWeapon = Instantiate(weaponPrefab, handTransform);
                newWeapon.gameObject.name = weaponLabel;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponOverride != null)
            {
                animator.runtimeAnimatorController = weaponOverride;
            } else if (overrideController != null)
            {
                Debug.LogWarning("Error - Missing animator override.");
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return newWeapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldRightWeapon = rightHand.Find(weaponLabel);
            Transform oldLeftWeapon = leftHand.Find(weaponLabel);
            
            if (oldRightWeapon != null) 
            { 
                oldRightWeapon.name = "DESTROYING";
                Destroy(oldRightWeapon.gameObject); 
            }
            if (oldLeftWeapon != null) 
            {
                oldLeftWeapon.name = "DESTROYING";
                Destroy(oldLeftWeapon.gameObject); 
            }
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return _isRightHanded ? rightHand : leftHand;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Vector3 aimTarget, GameObject wielder)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetAimDirection(aimTarget);
            projectileInstance.SetWielder(wielder);
        }

        // public void PlayAttackSound()
        // {
        //     attackSound.Play();
        // }

        // Get Attributes \\
        public string GetWeaponName()
        {
            return this.name;
        }
        public float GetWeaponRange()
        {
            return _weaponRange;
        }

        public float GetWeaponDelay()
        {
            return _weaponDelay;
        }

        public float GetWeaponDamage()
        {
            return _weaponDamage;
        }

        public float GetPercentBonus()
        {
            return _percentBonus;
        }

        public bool HasProjectile()
        {
            return projectile;
        }

        public Projectile GetProjectile()
        {
            return projectile;
        }

        public GameObject GetWielder()
        {
            return weaponWielder;
        }
    }
}