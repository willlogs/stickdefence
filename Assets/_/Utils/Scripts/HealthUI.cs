using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Utils
{
    public class HealthUI : MonoBehaviour
    {
        public void SetHealth(Health h){
            h.OnChange += SetValue;
            _health = h;
            _hasHealth = true;
        }

        [SerializeField] private Image _sliderIMG, _sliderBG;
        [SerializeField] private Health _health;
        [SerializeField] private float _showTime = 2f;

        private bool _hasHealth = false, _isVis = false;
        private Tweener _disappearTweener, _disappearTweenerBG;
        private float _waitTime = 0;

        private void SetValue(float value){
            _sliderIMG.fillAmount = value / _health.capacity;

            _isVis = true;
            _sliderIMG.color = new Color(_sliderIMG.color.r, _sliderIMG.color.g, _sliderIMG.color.b, 1f);
            _sliderBG.color = new Color(_sliderBG.color.r, _sliderBG.color.g, _sliderBG.color.b, 1f);

            _waitTime = _showTime;
            StartCoroutine(WaitAndDisappear());
        }

        private IEnumerator WaitAndDisappear()
        {
            while(_waitTime >= 0)
            {
                _waitTime -= Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
            }

            // TODO: don't compare to null. have a bool to track
            if (_disappearTweener != null)
            {
                _disappearTweener.Kill();
            }
            if (_disappearTweenerBG != null)
            {
                _disappearTweenerBG.Kill();
            }

            _disappearTweener = _sliderIMG.DOColor(
                new Color(_sliderIMG.color.r, _sliderIMG.color.g, _sliderIMG.color.b, 0f),
                1f
            ).OnComplete(() => { _isVis = false; });
            _disappearTweenerBG = _sliderBG.DOColor(
                new Color(_sliderBG.color.r, _sliderBG.color.g, _sliderBG.color.b, 0f),
                1f
            );
        }

        private void Start()
        {
            _sliderIMG.color = new Color(_sliderIMG.color.r, _sliderIMG.color.g, _sliderIMG.color.b, 0);
            _sliderBG.color = new Color(_sliderBG.color.r, _sliderBG.color.g, _sliderBG.color.b, 0);
        }

        private void Update()
        {
            if (_isVis)
            {
                _sliderBG.transform.rotation = Quaternion.identity;
                _sliderIMG.transform.rotation = Quaternion.identity;
            }
        }
    }
}