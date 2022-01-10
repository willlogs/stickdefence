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
        public event Action<Stickman> OnKilledByGun;

        public bool isTank, isChopper;
        public Transform mainGoalT, goalT;

        public void Damage(int damage)
        {
            health.Amount -= damage;
            if(health.Amount <= 0)
            {
                health.Amount = 0;
                Die();
            }
        }

        public void Die()
        {
            isDead = true;
            if (hasAnim)
                animator.SetTrigger("Die");
            renderer_.material = deathMat;
            foreach(Component c in removeOnDeath)
            {
                Destroy(c);
            }
            OnKilledByGun?.Invoke(this);
            Destroy(gameObject, 2);
        }

        [SerializeField] private Material deathMat;
        [SerializeField] private Renderer renderer_;
        [SerializeField] private Component[] removeOnDeath;

        [SerializeField] private Animator animator;
        [SerializeField] private Transform targetT; // move to goal and look at target
        [SerializeField] private float runningSpeed, goalDistanceThreshold, lookRotationSpeed = 5, pathfindingInterval = 0.1f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private StickPathTraveler pathTraveler;
        [SerializeField] private BoolCondition lookForwardCondition;

        [SerializeField] private Health health;
        [SerializeField] private HealthUI healthUI;

        private bool _isRunning = false, isDead = false, hasAnim;

        private void Awake()
        {
            if(healthUI != null)
                healthUI.SetHealth(health);

            hasAnim = animator != null;
            rb = GetComponent<Rigidbody>();
            goalT.parent = null;
            mainGoalT.parent = null;
            pathfindingInterval += UnityEngine.Random.Range(0f, 0.5f);
        }

        private void Update()
        {
            if (!isDead)
            {
                targetT.position = goalT.position;
                MoveStickman();
            }
        }

        private void MoveStickman()
        {
            Vector3 goalDiff = goalT.position - transform.position;
            goalDiff.y = 0;
            Vector3 targetDiff = targetT.position - transform.position;
            targetDiff.y = 0;

            float goalTargetDot = Vector3.Dot(goalDiff, targetDiff);

            if(hasAnim)
                animator.SetFloat("WalkSpeed", Mathf.Sign(goalTargetDot));

            if (goalDiff.magnitude > goalDistanceThreshold)
            {
                if (!_isRunning)
                {
                    _isRunning = true;
                    if (hasAnim)
                        animator.SetBool("Run", true);
                }
                rb.velocity = goalDiff.normalized * runningSpeed;

                if (!lookForwardCondition.value)
                {
                    transform.forward = Vector3.Slerp(transform.forward, targetDiff, Time.deltaTime * lookRotationSpeed);
                }
            }
            else
            {
                if (_isRunning)
                {
                    _isRunning = false;
                    if (hasAnim)
                        animator.SetBool("Run", false);
                    OnGoalReached?.Invoke();
                }
            }
        }
    }
}