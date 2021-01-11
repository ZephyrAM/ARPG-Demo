using System.Collections;
using UnityEngine;

using ZAM.Attributes;

namespace ZAM.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        // Assigned Variables \\
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] private float _hideTimer = 5f;
        [SerializeField] private float _healthToRestore = 0;

        // Adjustable Variables \\
        // private bool didPickup = false;

        private void Awake()
        {
            // if (didPickup == true) { Destroy(gameObject); }
        }

        // Interactions \\
        private void OnTriggerEnter(Collider other)
        {
            if (weapon != null)
            {
                CheckPickup(other.GetComponent<Fight>());
            }
            else if (_healthToRestore > 0)
            {
                CheckItemPickup(other.GetComponent<Health>());
            }
            
        }

        private void CheckPickup(Fight fight)
        {
            if (fight == null || fight.tag != "Player") { return; }

            // didPickup = true;
            fight.EquipWeapon(weapon);
            fight.SetAttackTimer(Mathf.Infinity);
            StartCoroutine(HideForRespawn(_hideTimer));
        }

        private void CheckItemPickup(Health health)
        {
            health.TakeHealing(RestoreHealth());
            StartCoroutine(HideForRespawn(_hideTimer));
        }

        private IEnumerator HideForRespawn(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        // public bool HandleRaycast()
        // {
        //     return true;
        // }

        public WeaponConfig GetWeaponPickup()
        {
            return this.weapon;
        }

        public float RestoreHealth()
        {
            return _healthToRestore;
        }

        // Save Component \\
        // [System.Serializable]
        // struct PickupSaveData
        // {
        //     public bool saveTrigger;
        // }

        // public object CaptureState()
        // {
        //     PickupSaveData data = new PickupSaveData();
        //     data.saveTrigger = didPickup;

        //     return data;
        // }

        // public void RestoreState(object state)
        // {
        //     PickupSaveData data = (PickupSaveData)state;
        //     didPickup = data.saveTrigger;
        // }
    }
}

