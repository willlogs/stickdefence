using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.StickDefence
{
    public class Stickman : MonoBehaviour
    {
        public void Die(){
            _animator.SetTrigger("Die");
            _isDead = true;
        }

        [SerializeField] private Transform _movementGoal;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _movementSpeed = 1f;

        private bool _isMoving, _shouldMove, _isDead;

        private void Start(){
            _movementGoal.parent = null;
        }
        
        private void Update(){
            if(_isDead)
                return;
            
            Vector3 diff = _movementGoal.position - transform.position;
            diff.y = 0;
            _shouldMove = diff.magnitude > 0.1f;

            if(_shouldMove && !_isMoving){
                // start moving
                _animator.SetBool("Run", true);
                _isMoving = true;
            }

            if(!_shouldMove && _isMoving){
                // stop moving
                _animator.SetBool("Run", false);
                _isMoving = false;
            }

            if(_isMoving){
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + diff,
                    Time.unscaledDeltaTime * _movementSpeed
                );

                transform.forward = diff;
            }
        }
    }
}