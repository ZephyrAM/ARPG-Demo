using System;
using UnityEngine;
using UnityEngine.Events;

namespace ZAM.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        public void OnHit()
        {
            if (gameObject != null) { onHit.Invoke(); }
        }
    }
}
