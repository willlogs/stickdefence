using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Utils
{
    public class Stackee : MonoBehaviour
    {
        public Transform _nextPos;
        public UnityEvent _passedEvent;

        public void RegisterNext(Stackee n)
        {
            _next = n;
        }

        public void RegisterParent(Stackee p)
        {
            _parent = p;
            _hasParent = true;
        }

        public void ActivateEvent()
        {
            _passedEvent?.Invoke();
            if (!gameObject.activeSelf)
                return;

            StartCoroutine(PassEvent());
        }

        public void Snap()
        {
            if (_hasParent)
            {
                transform.position = _parent._nextPos.position;
            }
        }

        [SerializeField] private float _speed = 30f, _eventPassWait = 0.2f;

        [SerializeField] private Stackee _parent;
        [SerializeField] private Stackee _next;

        private bool _hasParent = false;

        private IEnumerator PassEvent()
        {
            if(_next != null)
            {
                yield return new WaitForSeconds(_eventPassWait);
                _next.ActivateEvent();
            }
        }

        private void Update()
        {
            if (_hasParent)
            {
                transform.rotation = _parent.transform.rotation;
                transform.position = Vector3.Lerp(
                    transform.position,
                    _parent._nextPos.position,
                    Time.deltaTime * _speed
                );
            }
        }
    }
}