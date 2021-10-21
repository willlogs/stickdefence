using System.Collections;
using System.Collections.Generic;
using DB.Utils;
using UnityEngine;

namespace DB.StickDefence
{
    public class EnemyBuilding : MonoBehaviour
    {
        public Health health;

        public void Damage(float damage)
        {
            health.Amount = health.Amount - damage;
            if (health.Amount <= 0)
            {
                Explode();
            }
        }

        public void Damage(float damage, SquadManager squad)
        {
            Damage(damage);
            // give reward to squad
            if (_isDead)
            {

            }
        }

        public void Explode()
        {
            if (!_isDead)
            {
                _isDead = true;
                Destroy(gameObject);
            }
        }

        [SerializeField] private SpawnPoint[] _spawnPoints;
        [SerializeField] private int _soldierCapacity;
        [SerializeField] private float _betweenSpawns, _healthAmount = 500f;
        [SerializeField] private GameObject _SoldierPrefab;
        [SerializeField] private HealthUI _healthUI;
        [SerializeField] private List<GameObject> _soldiers;
        private int _activeSoldiersCount = 0;
        private bool _isDead = false, _canSpawn = false;

        private void Start()
        {
            health.capacity = _healthAmount;
            health.Amount = _healthAmount;
            StartCoroutine(Tick());
            _healthUI.SetHealth(health);
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                Check();
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator ActivateSpawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(_betweenSpawns);
                _canSpawn = true;
            }
        }

        private void Check()
        {
            if (_activeSoldiersCount < _soldierCapacity)
            {
                _soldiers.Add(
                    _spawnPoints[Random.Range(0, _spawnPoints.Length)].Spawn()
                );
                _activeSoldiersCount++;
                _canSpawn = false;
            }
        }
    }
}