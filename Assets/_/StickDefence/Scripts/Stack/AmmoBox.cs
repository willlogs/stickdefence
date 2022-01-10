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
        [SerializeField] private float lerpSpeed = 20f, rotLerpSpeed = 5f;
        private bool hasParent = false;
        private Vector3 lastPos;

        private void Start()
        {
            lastPos = parent.topT.position;
        }

        private void FixedUpdate()
        {
            if (hasParent)
            {
                Vector3 tp = (parent.topT.position + lastPos)/2;
                lastPos = parent.topT.position;
                transform.position = Vector3.Lerp(
                    transform.position,
                    parent.topT.position,
                    Time.fixedDeltaTime * lerpSpeed
                );

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    parent.transform.rotation,
                    Time.fixedDeltaTime * rotLerpSpeed
                );
            }
        }
    }
}