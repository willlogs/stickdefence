using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Utils
{
    public class HealthUI : MonoBehaviour
    {
        public void SetHealth(Health h){
            h.OnChange += SetValue;
            _health = h;
            _hasHealth = true;
        }

        [SerializeField] private Image _sliderIMG;
        [SerializeField] private Health _health;
        private bool _hasHealth = false;

        private void SetValue(float value){
            _sliderIMG.fillAmount = value / _health.capacity;
        }
    }
}