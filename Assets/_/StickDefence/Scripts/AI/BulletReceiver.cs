using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.StickDefence.AI
{
    public class BulletReceiver : MonoBehaviour
    {
        public UnityEvent OnCollision;
        [SerializeField] private LayerMask _layerMask;

        private void OnCollisionEnter(Collision c){
            if(((1 << c.gameObject.layer) & _layerMask.value) > 0){
                OnCollision?.Invoke();
            }
        }
    }
}