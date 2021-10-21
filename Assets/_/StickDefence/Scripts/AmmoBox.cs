using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.StickDefence
{
    public class AmmoBox : MonoBehaviour
    {
        public Transform nextPosition;

        public void Get(){
            foreach(Component c in _deleteOnGet){
                Destroy(c);
            }
        }

        public void GetStored(Action<AmmoBox> doAfter, Transform _targetPos){
            _target = _targetPos;
            _doAfter = doAfter;
            _isGettingStored = true;
            _doingRandomToss = false;
        }

        public void DoRandomToss(){
            _doingRandomToss = true;
            _goalPos = transform.position + new Vector3(
              UnityEngine.Random.Range(-1f, 1f),
              0,
              UnityEngine.Random.Range(-1f, 1f)
            ).normalized * 2f;
        }

        [SerializeField] private Component[] _deleteOnGet;
        private Action<AmmoBox> _doAfter;
        private Transform _target;
        private Vector3 _goalPos;
        private bool _isGettingStored = false, _doingRandomToss = false;

        private void Update(){
            if(_isGettingStored){
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.unscaledDeltaTime * 20);
                Vector3 diff = _target.position - transform.position;
                if(diff.magnitude < 0.1f){
                    _isGettingStored = false;
                    _doAfter(this);
                }
            }else if(_doingRandomToss){
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, _goalPos, Time.unscaledDeltaTime * 10);
                Vector3 diff = _goalPos - transform.parent.position;
                if(diff.magnitude < 0.1f){
                    _doingRandomToss = false;
                }
            }
        }
    }
}