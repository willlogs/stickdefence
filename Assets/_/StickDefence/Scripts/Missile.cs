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

        stick.OnBegin -= OnStartTouch;
        stick.OnEnd -= OnStopTouch;
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
        stick.OnBegin += OnStartTouch;
        stick.OnEnd += OnStopTouch;

        camFollower.SwitchTarget(camPos, () =>
        {
            rb.isKinematic = false;
            canControl = true;
        });
    }

    bool touching = false;
    private void OnStartTouch()
    {
        touching = true;
    }

    private void OnStopTouch()
    {
        touching = false;
    }

    public static bool canControl = false;
    private void Update()
    {
        if (canControl)
        {
            if (touching)
            {
                Vector3 control = stick._diff.normalized * controlPower * Time.deltaTime;
                control.z = control.y;
                control.y = 0;
                rb.velocity += control;
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x * 0.8f, rb.velocity.y, rb.velocity.z * 0.8f);
            }
        }
    }
}
