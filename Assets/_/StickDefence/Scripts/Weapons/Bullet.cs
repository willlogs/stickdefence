using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.StickDefence.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _shootingSpeed = 20f;

        public void GetShot(Vector3 dir){
            _rb.velocity = dir.normalized * _shootingSpeed;
        }
    }
}