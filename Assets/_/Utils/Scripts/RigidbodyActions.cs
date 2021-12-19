using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class RigidbodyActions : MonoBehaviour
    {
        public Vector3 vel;
        public Vector3 avel;

        public void SetVel()
        {
            vel = vel.x * transform.right + vel.y * transform.up + vel.z * transform.forward;
            _rb.velocity = vel;
        }

        public void SetAV()
        {
            _rb.angularVelocity = avel;
        }

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}