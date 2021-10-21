using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DB.StickDefence
{
    public class Upgradable : MonoBehaviour
    {
        public UnityEvent OnFullyUpgraded;

        [Button]
        public void Upgrade()
        {
            // activate a game object everytime it's called
            if (_level < _upgrades.Length)
            {
                _upgrades[_level].SetActive(true);
                GameObject go = Instantiate(_spawnParticle);
                go.transform.position = _upgrades[_level].transform.position;
                Vector3 lastPos = _upgrades[_level].transform.position;
                _upgrades[_level].transform.position = lastPos + Vector3.up * 5;
                _upgrades[_level].transform.DOMove(lastPos, 0.3f);
                _level++;
            }else if(!_isComplete){
                _isComplete = true;
                OnFullyUpgraded?.Invoke();
            }
        }

        // a system to upgrade level by level
        // a variable to indicate health and power
        // game object activator to activate torrets and stuff as they get built
        [SerializeField] private GameObject _spawnParticle;
        [SerializeField] private GameObject[] _upgrades;
        [SerializeField] private int _level = 0;
        private bool _isComplete = false;

        private void Start()
        {
            for (int i = _level; i < _upgrades.Length; i++)
            {
                _upgrades[i].SetActive(false);
            }
        }
    }
}