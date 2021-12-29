using DB.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War
{
    public class BuildingStage : MonoBehaviour
    {
        public event Action<BuildingStage> OnDestroyed;
        public bool isActive;
        public bool isDead = false;

        public UnityEvent OnDestroy;

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;

            foreach(Stickman.Stickman s in crowd)
            {
                s.OnKilledByGun -= OnOneDied;
            }
        }

        public void Tick()
        {
            if (isDead)
                return;

            if(crowd.Count < capacity)
            {
                AddTroop();
            }
        }

        public void Die()
        {
            isDead = true;
            OnDestroy?.Invoke();
            OnDestroyed?.Invoke(this);
        }

        public void Damage(int damage)
        {
            health.Amount -= damage;
            if(health.Amount <= 0)
            {
                health.Amount = 0;
                Die();
            }
        }

        [SerializeField] private Health health;
        [SerializeField] private HealthUI healthUI;
        [SerializeField] private List<Stickman.Stickman> crowd;
        [SerializeField] private int capacity = 3;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private void AddTroop()
        {
            GameObject go = Instantiate(enemyPrefab);
            Vector3 pos = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
            pos = new Vector3(pos.x, go.transform.position.y, pos.z);
            go.transform.position = pos;
            Stickman.Stickman s = go.GetComponent<Stickman.Stickman>();
            s.mainGoalT.transform.position = pos;
            s.goalT.position = pos;
            s.OnKilledByGun += OnOneDied;
            crowd.Add(s);
        }

        private void OnOneDied(Stickman.Stickman s)
        {
            crowd.Remove(s);
        }

        private void Awake()
        {
            healthUI.SetHealth(health);
        }
    }
}