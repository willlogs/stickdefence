using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DB.StickDefence
{
    [Serializable]
    public class Stage
    {
        public int need;
        public GameObject stagePrefab;
    }

    public class StageUnlocker : MonoBehaviour
    {
        public void ActivateUnlocker()
        {
            _canUnlock = true;
            _unlockButton.SetActive(true);
            _counterButton.SetActive(false);
        }

        public void UnlockStage()
        {
            if (_canUnlock)
            {
                _curStage = stages[_stageIndex++];
                _curStage.stagePrefab.SetActive(true);
                _canUnlock = false;
                _supply = 0;
                _unlockButton.SetActive(false);
                _counterButton.SetActive(true);
            }
        }

        public bool AddAmmox()
        {
            // add supply and check
            if (_supply < _curStage.need)
            {
                _supply++;
                return true;
            }
            else
            {
                ActivateUnlocker();
                return false;
            }
        }

        [SerializeField] private GameObject _unlockButton, _counterButton;
        [SerializeField] private TextElement _counter;

        [SerializeField] private int _supply;
        [SerializeField] private Stage[] stages;
        [SerializeField] private int _stageIndex = 0;
        [SerializeField] private Stage _curStage;
        [SerializeField] private bool _canUnlock = true;

        private void Start(){
            _curStage = stages[_stageIndex];
            _stageIndex++;
            _unlockButton.SetActive(false);
            _counterButton.SetActive(true);
        }

        private void UpdateCounter(){
            _counter.text = (_curStage.need - _supply) + " ";
        }
    }
}