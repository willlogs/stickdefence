using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stack
{
    public class AmmoBox : MonoBehaviour
    {
        public Transform topT;

        public void SetParent(AmmoBox p)
        {
            parent = p;
            hasParent = true;
        }

        [SerializeField] private AmmoBox parent;
        [SerializeField] private float lerpSpeed = 20f;
        private bool hasParent = false;

        private void FixedUpdate()
        {
            if (hasParent)
            {
                transform.position = Vector3.Slerp(
                    transform.position,
                    parent.topT.position,
                    Time.fixedDeltaTime * lerpSpeed
                );

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    parent.transform.rotation,
                    Time.fixedDeltaTime * lerpSpeed
                );
            }
        }
    }
}