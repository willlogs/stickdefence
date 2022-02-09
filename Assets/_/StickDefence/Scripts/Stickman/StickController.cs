using DB.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stickman
{
    public class StickController : MonoBehaviour
    {
        [SerializeField] private TouchStick joystick;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float velCap = 10f;
        [SerializeField] private float acceleration = 2f;

        private bool isDragging = false;

        private void Awake()
        {
            joystick.OnBegin += OnStartDrag;
            joystick.OnEnd += OnStopDrag;
        }

        private void OnStartDrag()
        {
            isDragging = true;
        }

        private void OnStopDrag()
        {
            isDragging = false;
        }

        private void Update()
        {
            if (Missile.canControl)
                return;

            if (isDragging)
            {
                Vector3 diff = new Vector3(joystick._diff.x, 0, joystick._diff.y) * Time.deltaTime * acceleration;
                rb.velocity += diff;

                if(rb.velocity.magnitude > velCap)
                {
                    rb.velocity = velCap * rb.velocity.normalized;
                }
            }
            else
            {
                Vector3 diff = Vector3.zero;
            }
        }
    }
}