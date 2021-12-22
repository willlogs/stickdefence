using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Stack
{
    public class Upgradable : MonoBehaviour
    {
        public UnityEvent OnFullyUpgraded;

        public void Enter(Collider other)
        {
            stacker = other.GetComponent<Stacker>();
            hasStacker = stacker != null;
        }

        public void Exit(Collider other)
        {
            hasStacker = false;
        }

        public void Upgrade()
        {
            parts[level++].SetActive(true);
            if(level >= parts.Length)
            {
                OnFullyUpgraded?.Invoke();
            }
        }

        [SerializeField] GameObject[] parts;
        [SerializeField] private int level = 0;
        [SerializeField] private Stacker stacker;
        bool hasStacker = false;

        private void Start()
        {
            for(int i = level; i < parts.Length; i++)
            {
                parts[i].gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (hasStacker && level < parts.Length && stacker.GetAmmoBox())
            {
                Upgrade();
            }
        }
    }
}