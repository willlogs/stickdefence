using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.StickDefence.Weapons
{
    public class Bullet : MonoBehaviour
    {
        public SquadManager _squadManager;
        public float damage = 10f;
        public bool _hasSquad = false;
        
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _shootingSpeed = 20f;

        public void GetShot(Vector3 dir, SquadManager squad, float d = -1){
            _hasSquad = true;
            _squadManager = squad;
            GetShot(dir, d);
        }

        public void GetShot(Vector3 dir, float d = -1){
            if(d != -1)
                damage = d;
            
            _rb.velocity = dir.normalized * _shootingSpeed;
        }

        private void OnCollisionEnter(Collision c){
            Destroy(gameObject);
        }
    }
}