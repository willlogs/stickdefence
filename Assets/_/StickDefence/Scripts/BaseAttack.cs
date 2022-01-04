using DB.War.Stickman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    int i = 0;
    public void Enter(Collider other)
    {
        StickController stick = other.gameObject.GetComponent<StickController>();
        if (stick != null)
        {
            if (i++ == 0)
                return;

            Attack(other.transform);
        }
    }

    private void Attack(Transform target)
    {
        foreach(Enemy e in FindObjectsOfType<Enemy>())
        {
            e.SetTarget(target);
        }
    }
}
