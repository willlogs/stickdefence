using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils{
    public class MouseInput : MonoBehaviour
    {
        [TitleGroup("Variables")]
        public Vector2 mouseInput;
        public Vector2 independentInput;
        public float deathRate = 0.99f;

        [NonSerialized]
        public Vector2 _startPos, _endPos;
        public event Action OnClickStart, OnClickEnd;

        private bool _hasLastInput, _firstDiff;

        private Vector2 _lastInput, _input;
        private float widthMultiplier;

        private void Start()
        {
            widthMultiplier = 1080f / (float)Screen.width;
        }

        private void Update(){
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
                OnClickStart?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _endPos = Input.mousePosition;
                OnClickEnd?.Invoke();
            }

            if (Input.GetMouseButton(0))
            {
                _input = Input.mousePosition;
                if (_hasLastInput)
                {
                    Vector2 diff = _input - _lastInput;
                    mouseInput = diff;
                    independentInput = mouseInput * widthMultiplier;
                }
                else
                {
                    _hasLastInput = true;
                }
                _lastInput = _input;
            }
            else
            {
                _hasLastInput = false;
                mouseInput *= deathRate;
            }
        }
    }
}