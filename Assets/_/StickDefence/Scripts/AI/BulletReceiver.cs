using System.Collections;
using System.Collections.Generic;
using DB.StickDefence.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace DB.StickDefence.AI
{
    public class BulletReceiver : MonoBehaviour
    {
        public UnityEvent<float> OnCollision;
        public UnityEvent<float, SquadManager> OnCollisionS;

        [SerializeField] private LayerMask _layerMask;

        private void OnCollisionEnter(Collision c){
            if(((1 << c.gameObject.layer) & _layerMask.value) > 0){
                Bullet b = c.gameObject.GetComponent<Bullet>();
                if(b._hasSquad){
                    OnCollisionS?.Invoke(b.damage, b._squadManager);
                }
                else{
                    OnCollision?.Invoke(b.damage);
                }
            }
        }
    }
}