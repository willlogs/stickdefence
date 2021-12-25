using DB.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Stack
{
    public class Unstacker : MonoBehaviour
    {
        public UnityEvent OnUpgrade;

        public void Enter(Collider other)
        {
            if (other.isTrigger)
                return;

            stacker = other.GetComponent<Stacker>();
            hasStacker = stacker != null;
        }

        public void Exit(Collider other)
        {
            if (other.isTrigger)
                return;

            hasStacker = false;
        }

        [SerializeField] private Stacker stacker;
        [SerializeField] private BoolCondition condition;
        private bool hasStacker = false;

        private void Update()
        {
            if (hasStacker && condition.value && stacker.GetAmmoBox(transform))
            {
                condition.value = false;
                OnUpgrade?.Invoke();
            }
        }
    }
}