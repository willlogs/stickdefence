using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Weapons
{
    public class Damager : MonoBehaviour
    {
        public UnityEvent<int> OnDamage;

        public void Damage(int damage)
        {
            OnDamage?.Invoke(damage);
        }

        public void Enter(Collision c)
        {
            BulletBase b = c.gameObject.GetComponent<BulletBase>();
            if(b != null)
            {
                Damage(b.damage);
            }
        }
    }
}