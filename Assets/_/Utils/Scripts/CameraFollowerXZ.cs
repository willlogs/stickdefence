using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT.Utils
{
    public class CameraFollowerXZ : MonoBehaviour
    {
        [Button]
        public void ScaleOffset(float scaleX, float scaleY)
        {
            _offset.y *= scaleY;
            _offset.z *= scaleX;
        }

        Vector3 beforePos;
        Quaternion beforeRot;

        public void SwitchTarget(Transform target, Action doOnDone)
        {
            update = false;
            beforePos = transform.position;
            beforeRot = transform.rotation;

            transform.parent = target;
            transform.DOLocalMove(Vector3.zero, 1).OnComplete(() => { doOnDone(); });
            transform.DOLocalRotateQuaternion(Quaternion.identity, 1);
        }

        public void BackToNormal()
        {
            transform.parent = null;
            transform.DOMove(beforePos, 1);
            transform.DORotateQuaternion(beforeRot, 1).OnComplete(() => { update = true; });
        }

        [SerializeField] private Transform _target;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private bool _lookAtIt = false;
        [SerializeField] private Vector3 _offset;

        bool update = true;

        private void Awake(){
            _offset = transform.position - _target.position;
        }
        
        private void Update(){
            if (update)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(
                        _target.position.x + _offset.x,
                        _target.position.y + _offset.y,
                        _target.position.z + _offset.z
                    ),
                    Time.deltaTime / _delay
                );

                if (_lookAtIt)
                {
                    Vector3 frw = _target.position - transform.position;
                    transform.forward = Vector3.Lerp(transform.forward, frw, Time.deltaTime);
                }
            }
        }
    }
}
