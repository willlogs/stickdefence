using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT.Utils
{
    public class CameraFollowerXZ : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _delay = 0.5f;
        private Vector3 _offset;

        private void Awake(){
            _offset = transform.position - _target.position;
        }
        
        private void Update(){
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(
                    _target.position.x + _offset.x,
                    transform.position.y,
                    _target.position.z + _offset.z
                ),
                Time.deltaTime / _delay
            );
        }
    }
}