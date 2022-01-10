using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Utils
{
    public class OnTriggerEvent : MonoBehaviour
    {
        public LayerMask layerMask;
        public UnityEvent<Collider> OnEnter, OnExit, OnStay;

        private void OnTriggerEnter(Collider other)
        {
            int layerTest = layerMask.value & (1 << other.gameObject.layer);
            if (layerTest > 0)
            {
                OnEnter?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int layerTest = layerMask.value & (1 << other.gameObject.layer);
            if (layerTest > 0)
            {
                OnExit?.Invoke(other);
            }
        }

        /*private void OnTriggerStay(Collider other)
        {
            int layerTest = layerMask.value & (1 << other.gameObject.layer);
            if (layerTest > 0)
            {
                OnStay?.Invoke(other);
            }
        }*/
    }
}