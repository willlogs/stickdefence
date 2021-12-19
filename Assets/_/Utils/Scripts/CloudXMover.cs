using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class CloudXMover : MonoBehaviour
    {
        [SerializeField] private Transform _start, _end;
        [SerializeField] private float _speed = 1f;

        private int _direction = 1;

        private void Start()
        {
            float diff = _end.position.x - _start.position.x;
            _direction = diff > 0 ? 1 : -1;
            diff = Mathf.Abs(diff);
            transform.position = new Vector3(
                _start.position.x + Random.Range(0, diff) * _direction,
                transform.position.y,
                transform.position.z
            );
        }

        private void Update()
        {
            transform.position += Vector3.right * Time.deltaTime * _speed * _direction;
            float diff = Mathf.Abs(transform.transform.position.x - _end.position.x);
            if(diff < 0.5f)
            {
                transform.position = new Vector3(
                    _start.position.x,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }
}