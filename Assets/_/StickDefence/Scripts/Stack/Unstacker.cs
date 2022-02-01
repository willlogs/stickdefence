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

        public void ForceExit()
        {
            hasStacker = false;
        }

        public void Exit(Collider other)
        {
            if (other.isTrigger)
                return;

            hasStacker = false;
        }

        [SerializeField] private Stacker stacker;
        [SerializeField] private BoolCondition condition;
        [SerializeField] private ParticleSystem particle;
        private bool hasStacker = false;

        private void Start()
        {
            Stacker st = FindObjectOfType<Stacker>();
            st.OnMoreThanOneE += Activate;
            st.OnReachZeroE += Deactivate;
            if(st.score > 0)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            particle.Play();
        }

        private void Deactivate()
        {
            particle.Stop();
        }

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