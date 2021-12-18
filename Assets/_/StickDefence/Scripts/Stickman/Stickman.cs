using DB.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stickman
{
    public class Stickman : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform goalT, targetT; // move to goal and look at target
        [SerializeField] private float runningSpeed, goalDistanceThreshold, lookRotationSpeed = 5;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TouchStick joystick;

        private bool _isRunning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            goalT.parent = null;
        }

        private void Update()
        {
            MoveGoal();
        }

        private void FixedUpdate()
        {
            targetT.position = goalT.position;
            MoveStickman();
        }

        private void MoveStickman()
        {
            Vector3 goalDiff = goalT.position - transform.position;
            Vector3 targetDiff = targetT.position - transform.position;

            float goalTargetDot = Vector3.Dot(goalDiff, targetDiff);
            animator.SetFloat("WalkSpeed", Mathf.Sign(goalTargetDot));

            if (goalDiff.magnitude > goalDistanceThreshold)
            {
                if (!_isRunning)
                {
                    _isRunning = true;
                    animator.SetBool("Run", true);
                }
                rb.velocity = goalDiff.normalized * runningSpeed;
                transform.forward = Vector3.Slerp(transform.forward, targetDiff, Time.fixedDeltaTime * lookRotationSpeed);
            }
            else
            {
                if (_isRunning)
                {
                    _isRunning = false;
                    animator.SetBool("Run", false);
                }
            }
        }

        private void MoveGoal()
        {
            Vector3 diff = new Vector3(joystick._diff.x, 0, joystick._diff.y);
            goalT.position += diff;
        }
    }
}