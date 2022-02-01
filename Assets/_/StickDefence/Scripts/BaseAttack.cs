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
            if (i++ < 4)
                return;

            Attack(other.transform);
        }
    }

    public void ShowWarning()
    {
        GameObject go = Instantiate(attackInformerPrefab, canvas.transform);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        go.transform.localScale = Vector3.one;
    }

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject attackInformerPrefab;

    private void Attack(Transform target)
    {
        ShowWarning();
        foreach(Enemy e in FindObjectsOfType<Enemy>())
        {
            e.SetTarget(target);
        }
    }
}
