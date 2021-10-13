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
        [SerializeField] private Stickman _stickman;

        [FoldoutGroup("rederer")]
        [SerializeField] private Renderer _characterRenderer;
        [FoldoutGroup("rederer")]
        [SerializeField] private Material _deathMaterial;

        public void GetDamaged(){
            _health.Amount -= 100f;
            if(_health.Amount <= 0){
                Die();
            }
        }

        public void Die(){
            _characterRenderer.material = _deathMaterial;
            _stickman.Die();
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            Destroy(gameObject, 5f);
        }
    }
}