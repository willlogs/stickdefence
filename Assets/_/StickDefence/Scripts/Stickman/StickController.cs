using DB.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stickman
{
    public class StickController : MonoBehaviour
    {
        [SerializeField] private TouchStick joystick;
        [SerializeField] private Transform goalT;

        private bool isDragging = false;

        private void Awake()
        {
            goalT.parent = null;
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
                goalT.position = transform.position + new Vector3(joystick._diff.x, 0, joystick._diff.y) * Time.deltaTime * 15;
            }
            else
            {
                goalT.position = transform.position;
            }
        }
    }
}