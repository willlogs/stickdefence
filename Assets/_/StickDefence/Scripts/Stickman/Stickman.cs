using DB.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War.Stickman
{
    public class Stickman : MonoBehaviour
    {
        public event Action OnGoalReached;
        public event Action OnKilledByGun;

        public Transform mainGoalT;

        [SerializeField] private Animator animator;
        [SerializeField] private Transform goalT, targetT; // move to goal and look at target
        [SerializeField] private float runningSpeed, goalDistanceThreshold, lookRotationSpeed = 5, pathfindingInterval = 0.1f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private StickPathTraveler pathTraveler;
        [SerializeField] private BoolCondition lookForwardCondition;

        private bool _isRunning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            goalT.parent = null;
            mainGoalT.parent = null;
            pathfindingInterval += UnityEngine.Random.Range(0f, 0.5f);
        }

        private void FixedUpdate()
        {
            targetT.position = goalT.position;
            MoveStickman();
        }

        private void MoveStickman()
        {
            Vector3 goalDiff = goalT.position - transform.position;
            goalDiff.y = 0;
            Vector3 targetDiff = targetT.position - transform.position;
            targetDiff.y = 0;

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

                if (!lookForwardCondition.value)
                {
                    transform.forward = Vector3.Slerp(transform.forward, targetDiff, Time.fixedDeltaTime * lookRotationSpeed);
                }
            }
            else
            {
                if (_isRunning)
                {
                    _isRunning = false;
                    animator.SetBool("Run", false);
                    OnGoalReached?.Invoke();
                }
            }
        }
    }
}