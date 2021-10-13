using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Utils
{
    public class TouchStick : MonoBehaviour
    {
        public Vector2 _diff;
        public event Action OnBegin, OnEnd;

        [SerializeField] private Image _base, _stick;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _lockRange = 100;

        private bool _active = false;
        private float _halfWidth, _halfHeight;

        private void Awake(){
            _halfWidth = (float)Screen.width / 2f;
            _halfHeight = (float)Screen.height / 2f;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Begin();
            }

            if (Input.GetMouseButtonUp(0))
            {
                End();
            }

            if (_active)
            {
                _stick.rectTransform.anchoredPosition = MousePosToCanvasPos(Input.mousePosition);
                Vector3 diff = _stick.rectTransform.anchoredPosition - _base.rectTransform.anchoredPosition;
                _diff = diff;
                if(diff.magnitude > _lockRange){
                    _stick.rectTransform.anchoredPosition = _base.rectTransform.anchoredPosition + (Vector2)diff.normalized * _lockRange;
                    _diff = _stick.rectTransform.anchoredPosition - _base.rectTransform.anchoredPosition;
                }
            }
        }

        private void Start()
        {
            Activate(false);
        }

        private void Begin()
        {
            Activate(true);
            _base.rectTransform.anchoredPosition = MousePosToCanvasPos(Input.mousePosition);
            OnBegin?.Invoke();
        }

        private void End()
        {
            Activate(false);
            OnEnd?.Invoke();
        }

        private void Activate(bool inpt)
        {
            _base.gameObject.SetActive(inpt);
            _stick.gameObject.SetActive(inpt);
            _active = inpt;
        }
        
        private Vector3 MousePosToCanvasPos(Vector3 input){
            Vector2 _mousePos = input - new Vector3(_halfWidth, _halfHeight);
            _mousePos =  _mousePos / _canvas.transform.lossyScale;
            return _mousePos;
        }
    }
}