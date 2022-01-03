using DB.Utils;
using DB.War.Weapons;
using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public void SwitchCamBack()
    {
        canControl = false;
        camFollower.BackToNormal();

        Collider[] cs = Physics.OverlapSphere(transform.position, 15, layerMask);
        foreach(Collider c in cs)
        {
            if (c.isTrigger)
                continue;

            Damager d = c.GetComponent<Damager>();
            if(d != null)
            {
                d.Damage(10000);
            }
        }

        Destroy(gameObject);
    }

    [SerializeField] private Transform camPos;
    [SerializeField] private float controlPower = 10f;
    [SerializeField] private LayerMask layerMask;

    TouchStick stick;
    CameraFollowerXZ camFollower;
    Rigidbody rb;

    void Start()
    {
        stick = FindObjectOfType<TouchStick>();
        camFollower = FindObjectOfType<CameraFollowerXZ>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        camFollower.SwitchTarget(camPos, () =>
        {
            rb.isKinematic = false;
            canControl = true;
        });
    }

    public static bool canControl = false;
    private void Update()
    {
        if (canControl)
        {
            Vector3 control = stick._diff.normalized * controlPower * Time.deltaTime;
            control.z = control.y;
            control.y = 0;
            rb.velocity += control;
        }
    }
}
