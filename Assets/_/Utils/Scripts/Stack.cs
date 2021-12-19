using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.Utils
{
    public class Stack : MonoBehaviour
    {
        [Button]
        public void PlusPlus(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                PlusPlus();
            }
        }

        public void PlusPlus()
        {
            _count++;
            Stackee _newBox;
            if(_count > 1)
            {
                // check if we have it
                if(_count - _stack.Count > 1)
                {
                    // we need a new box
                    _newBox = AddStackee();
                }
                else
                {
                    // we already have a box
                    _newBox = _stack[_count - 2];
                    _newBox.gameObject.SetActive(true);
                    _newBox.Snap();
                }
            }
            else
            {
                // use _base
                _base.gameObject.SetActive(true);
            }

            if (_canTriggerEvent) {
                StartCoroutine(RestForEvent());
                OnPlusPlus?.Invoke();
            }
        }

        [Button]
        public void MinusMinus()
        {
            if (_count > 0)
            {
                if (_count > 1)
                {
                    _stack[_count - 2].gameObject.SetActive(false);
                }
                else
                {
                    _base.gameObject.SetActive(false);
                }
                _count--;
            }
        }

        public bool CanUnload()
        {
            return _count > 0;
        }

        public UnityEvent OnPlusPlus;
        [SerializeField] private Stackee _base;
        [SerializeField] private List<Stackee> _stack;
        [SerializeField] private GameObject _stackePrefab;

        [SerializeField] private int _count = 0;
        [SerializeField] private int initFill = 50;
        [SerializeField] private float _EventRest = 1f;
        private bool _canTriggerEvent = true;

        private IEnumerator RestForEvent()
        {
            _canTriggerEvent = false;
            yield return new WaitForSeconds(_EventRest);
            _canTriggerEvent = true;
        }

        private void Start()
        {
            AddStackees(initFill);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                PlusPlus(60);
            }
        }

        private void AddStackees(int i)
        {
            for(; i >= 0; i--)
            {
                AddStackee();
            }

            foreach(Stackee s in _stack)
            {
                s.gameObject.SetActive(false);
                _base.gameObject.SetActive(false);
            }
        }

        private Stackee AddStackee()
        {
            GameObject go = Instantiate(_stackePrefab);
            Stackee _last = GetLastStackee();
            go.transform.position = _last._nextPos.position;
            go.transform.parent = transform;
            Stackee s = go.GetComponent<Stackee>();
            _stack.Add(s);
            _last.RegisterNext(s);
            s.RegisterParent(_last);
            return s;
        }

        private Stackee GetLastStackee()
        {
            if(_stack.Count > 0)
            {
                return _stack[_stack.Count - 1];
            }

            return _base;
        }
    }
}