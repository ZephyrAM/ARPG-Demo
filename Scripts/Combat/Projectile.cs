using UnityEngine;
using UnityEngine.Events;

namespace ZAM.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 25f;
        [SerializeField] private float _waitAfterCollision = 1.5f;
        [SerializeField] private float _waitOutOfRange = 4f;
        [SerializeField] private float _xAimVariance = 0.1f;
        [SerializeField] private float _yAimVariance = 0.1f;

        [SerializeField] AudioSource hitAudioEffect = null;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;

        private Vector3 origin;
        private Vector3 direction;

        private bool hitTarget = false;

        GameObject wielder;

        // Base Methods - Unity \\
        private void Start()
        {
            origin = transform.position;
            transform.LookAt(direction);    
        }

        private void FixedUpdate()
        {
            if (!hitTarget)
            {
                transform.position -= (origin - direction).normalized * _speed * Time.deltaTime;
            }

            if (transform.parent != null)
            {
                if (GetComponentInParent<CombatTarget>() && GetComponentInParent<CombatTarget>().IsDead())
                {
                    Destroy(gameObject);
                }
            }
            
            Destroy(gameObject, _waitOutOfRange);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other != wielder.GetComponent<Collider>()) 
            { 
                hitTarget = true;
                AttachToTarget(other);

                if ((other.tag == "Player" || other.tag == "Enemy") && wielder.tag != other.tag)
                {
                    wielder.GetComponent<Fight>().CombatLog(other);
                }
                
                hitAudioEffect.Play();
                if (hitEffect != null) 
                {
                    Instantiate(hitEffect, transform.position, transform.rotation);
                }
                DestroyOnImpact();
            }

        }

        // Set Properties \\
        public void SetAimDirection(Vector3 aimDirection)
        {
            direction = aimDirection + ProjectileVariance(); 
        }

        private Vector3 ProjectileVariance()
        {
            Vector3 variance = new Vector3(AimVariance(_xAimVariance), AimVariance(_yAimVariance), 0);
            return variance;
        }

        public void SetWielder(GameObject getWielder)
        {
            wielder = getWielder;
        }

        private void AttachToTarget(Collider other)
        {
            transform.parent = other.transform;
            if (GetComponentInChildren<TrailRenderer>()) { GetComponentInChildren<TrailRenderer>().enabled = false; }
        }

        // Value Checks \\
        private float AimVariance(float adjust)
        {
            return Random.Range(-adjust, adjust);
        }

        // Destroy Events \\
        private void DestroyOnImpact()
        {
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, _waitAfterCollision);
        }
    }
}
