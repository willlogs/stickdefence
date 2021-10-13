using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT.Utils
{
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager _instance;
        [SerializeField] private List<float> _layers = new List<float>();

        public static TimeManager Instance{
            get
            {
                if(_instance == null)
                {
                    _instance = new GameObject().AddComponent<TimeManager>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this;
        }

        private IEnumerator DelayJobExecute(float delay, Action job)
        {
            yield return new WaitForSecondsRealtime(delay);
            job();
        }

        private bool _isSlow = false;

        public void DoWithDelay(float delay, Action job)
        {
            StartCoroutine(DelayJobExecute(delay, job));
        }

        public void AddLayer(float tweenerDuration, float factor = 0.3f)
        {
            _isSlow = true;
            _layers.Add(factor);
            if (tweenerDuration == 0)
            {
                SetSpeed(factor);
            }
            else
            {
                SetSpeedTweener(tweenerDuration, factor);
            }
        }

        public void GoBackALayer(float tweenerDuration = 0)
        {
            float _lastLevel = 1f;
            if(_layers.Count > 1)
            {
                _layers.RemoveAt(_layers.Count - 1);
                _lastLevel = _layers[_layers.Count - 1];
            }
            else
            {
                GoNormal();
                return;
            }

            if (tweenerDuration == 0)
            {
                SetSpeed(_lastLevel);
            }
            else
            {
                SetSpeedTweener(tweenerDuration, _lastLevel);
            }
        }

        public void GoNormal(float tweenerDuration = 0)
        {
            _isSlow = false;
            _layers = new List<float>();
            if (tweenerDuration == 0)
            {
                SetSpeed(1f);
            }
            else
            {
                SetSpeedTweener(tweenerDuration, 1f);
            }
        }

        private Tweener _scaleTweener, _fixedDTTweener;

        private void SetSpeedTweener(float duration, float ts)
        {
            try
            {
                _scaleTweener.Kill();
                _fixedDTTweener.Kill();
            }
            catch { }

            _scaleTweener = DOTween.To(() => Time.timeScale, (x) => { Time.timeScale = x; }, ts, duration).SetUpdate(true);
            _fixedDTTweener = DOTween.To(() => Time.fixedDeltaTime, (x) => { Time.fixedDeltaTime = x; }, ts * 0.01f, duration).SetUpdate(true);
        }

        private void SetSpeed(float ts)
        {
            Time.timeScale = ts;
            Time.fixedDeltaTime = 0.01f * ts;
        }
    }
}