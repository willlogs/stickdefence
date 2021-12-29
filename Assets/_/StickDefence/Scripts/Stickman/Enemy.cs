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
            pathTraveler.SetAutoTravel(target, 10f);
        }

        public void Damage(int damage)
        {
            health.Amount = health.Amount - damage;
            if(health.Amount <= 0)
            {
                health.Amount = 0;
                Die();
                Destroy(gameObject, 3);
            }
        }

        public void Die()
        {
            for(int i = 0; i < rewardCount; i++)
            {
                GameObject go = Instantiate(ammoxPrefab);
                go.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            }

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
        [SerializeField] private GameObject ammoxPrefab;
        [SerializeField] private int rewardCount = 10;

        private Transform target;
        private bool hasTarget = false;

        private void Start()
        {
            healthUI.SetHealth(health);
        }
    }
}