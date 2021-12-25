using DB.Utils;
using DB.War.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stickman
{
    public class Enemy : MonoBehaviour
    {
        public void Enter(Collider c)
        {
            if (c.isTrigger)
                return;

            SetTarget(c.transform);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
            hasTarget = false;
            pathTraveler.SetAutoTravel(target, 5f);
        }

        public void Damage(int damage)
        {
            health.Amount = health.Amount - damage;
            if(health.Amount <= 0)
            {
                health.Amount = 0;
                Die();
            }
        }

        public void Die()
        {
            stickman.Die();
            gameObject.layer = 7;
        }
        // receive bullets
        // die
        // follow player or attack objective

        [SerializeField] private Stickman stickman;
        [SerializeField] private StickPathTraveler pathTraveler;
        [SerializeField] private Health health;
        [SerializeField] private HealthUI healthUI;

        private Transform target;
        private bool hasTarget = false;

        private void Start()
        {
            healthUI.SetHealth(health);
        }
    }
}