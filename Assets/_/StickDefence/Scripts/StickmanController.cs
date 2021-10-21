using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DB.Utils;

namespace DB.StickDefence
{
    public class StickmanController : MonoBehaviour
    {
        [SerializeField] private TouchStick _stick;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _changeSpeed = 1, _lerpSpeed = 1, _movementSpeed = 2f;
        [SerializeField] private Animator _animator;

        private bool _isMoving = false;

        private void OnStart(){
            _isMoving = true;
            _animator.SetBool("Run", true);
        }

        private void OnEnd(){
            _isMoving = false;
            _animator.SetBool("Run", false);
        }

        private void Awake(){
            _stick.OnBegin += OnStart;
            _stick.OnEnd += OnEnd;
        }

        private void OnDestroy(){
            _stick.OnBegin -= OnStart;
            _stick.OnEnd -= OnEnd;
        }

        private void Update(){
            if(_isMoving){
                Vector3 _posChange = new Vector3(
                    _stick._diff.normalized.x,
                    0,
                    _stick._diff.normalized.y
                );

                // transform.position = Vector3.Lerp(
                //     transform.position,
                //     transform.position + _posChange * _changeSpeed,
                //     Time.unscaledDeltaTime * _lerpSpeed
                // );
                
                _rb.velocity = _posChange.normalized * _movementSpeed;
                transform.forward = _posChange.normalized;
            }
        }
    }
}