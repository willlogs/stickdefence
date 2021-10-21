using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DB.Utils;
using Sirenix.OdinInspector;

namespace DB.StickDefence.AI
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] public Health _health;
        [SerializeField] private HealthUI _healthUI;
        [SerializeField] private Stickman _stickman;

        [FoldoutGroup("rederer")]
        [SerializeField] private Renderer _characterRenderer;
        [FoldoutGroup("rederer")]
        [SerializeField] private Material _deathMaterial;

        [SerializeField] private Component[] _destroyOnDeath;
        [SerializeField] private GameObject _ammoxPrefab;
        [SerializeField] private int _numberOfDrops = 2;

        private bool _isDead = false;

        private void Start(){
            _healthUI.SetHealth(_health);
        }

        public void GetDamaged(float damage)
        {
            _health.Amount = _health.Amount - damage;
            if (_health.Amount <= 0)
            {
                Die();
            }
        }

        public void GetDamaged(float damage, SquadManager squad)
        {
            _health.Amount = _health.Amount - damage;
            if (_health.Amount <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (!_isDead)
            {
                _isDead = true;
                _characterRenderer.material = _deathMaterial;
                _stickman.Die();
                gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

                for(int i = 0; i < _numberOfDrops; i ++){
                    GameObject go = Instantiate(_ammoxPrefab);
                    go.transform.position = transform.position;
                    go.GetComponentInChildren<AmmoBox>().DoRandomToss();
                }

                for (int i = _destroyOnDeath.Length - 1; i >= 0; i--)
                {
                    Destroy(_destroyOnDeath[i]);
                }
                Destroy(gameObject, 5f);
            }
        }
    }
}