using DB.War.Stickman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    public void Enter(Collider other)
    {
        StickController stick = other.gameObject.GetComponent<StickController>();
        if (stick != null)
        {
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
