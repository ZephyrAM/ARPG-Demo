﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZAM.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;

        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}", amount);
        }
    }
}
