using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    [Serializable] public class Health
    {
        // TODO: Change OnChange to 2 types: On reduce, On Increase
        public event Action<float> OnChange;
        public float capacity = 100f;

        [SerializeField] private float amount = 100f;

        public float Amount{
            get{
                return amount;
            }

            set{
                amount = value;
                OnChange?.Invoke(amount);
            }
        }
    }
}