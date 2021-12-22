using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Weapons
{
    public class BulletBase : MonoBehaviour
    {
        public UnityEvent OnContact;

        public void GetShot(Vector3 dir, Transform tar)
        {
            target = tar;
            hasTarget = true;
            GetShot(dir);
        }

        public void GetShot(Vector3 dir)
        {
            rb.velocity = dir.normalized * speed;
            transform.forward = dir.normalized;
            gotShot = true;
        }

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;

        private Transform target;
        private bool hasTarget = false;
        private bool gotShot = false;

        private void Update()
        {
            if (gotShot)
            {
                Vector3 dir = rb.velocity.normalized;
                if (hasTarget)
                {
                    dir = target.position - transform.position;
                    dir = dir.normalized;
                }

                rb.velocity = dir * speed;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnContact?.Invoke();
            Destroy(gameObject);
        }
    }
}