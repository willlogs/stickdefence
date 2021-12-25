using DB.Utils;
using DG.Tweening;
using PT.Utils;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Stack
{
    public class Upgradable : MonoBehaviour
    {
        public UnityEvent OnFullyUpgraded;
        public event Action<Upgradable> OnFullyUpgradedE;

        public void Unlock()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].gameObject.SetActive(true);
            }

            whole.SetActive(true);
            whole.transform.parent = null;
            Destroy(gameObject);
        }

        public void Upgrade()
        {
            if (level < parts.Count)
            {
                GameObject part = parts[level++];
                part.SetActive(true);

                Quaternion rot = part.transform.rotation;
                Vector3 pos = part.transform.position;

                part.transform.position = new Vector3(UnityEngine.Random.Range(-1f, 1f), 1, UnityEngine.Random.Range(0, 1f)).normalized * 20 + pos;
                Vector3 eulers = new Vector3(UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-90f, 90f), UnityEngine.Random.Range(-90f, 90f));
                part.transform.rotation = Quaternion.Euler(eulers);

                part.transform.DOMove(pos, 0.5f);
                part.transform.DORotateQuaternion(rot, 0.5f);

                if (level >= parts.Count)
                {
                    Finish();
                }
                else
                {
                    TimeManager.Instance.DoWithDelay(upgradeDelay, () =>
                    {
                        condition.value = true;
                    });
                }                
            }
        }

        [Button]
        public void Damage(int damage)
        {
            health.Amount -= damage;
            if(health.Amount <= 0)
            {
                health.Amount = 0;
                Explode();    
            }
        }

        public void Explode()
        {
            whole.SetActive(false);
            partsPar.SetActive(true);
            partsPar.transform.parent = null;
            TimeManager.Instance.DoWithDelay(5, () =>
            {
                partsPar.transform.DOScale(0, 1).OnComplete(() =>
                {
                    Destroy(partsPar);
                });
            });

            foreach(Transform t in partsPar.transform)
            {
                t.gameObject.AddComponent<MeshCollider>().convex = true;
                t.gameObject.AddComponent<Rigidbody>();
            }

            Destroy(gameObject);
        }

        private void Finish()
        {
            OnFullyUpgraded?.Invoke();
            OnFullyUpgradedE?.Invoke(this);
            unstacker.SetActive(false);

            whole.SetActive(true);
            partsPar.SetActive(false);
        }

        [SerializeField] private Health health;
        [SerializeField] private HealthUI healthUI;

        [SerializeField] List<GameObject> parts;
        [SerializeField] private GameObject whole, partsPar, unstacker;
        [SerializeField] private int level = 0;
        [SerializeField] private float upgradeDelay = 0.1f;
        [SerializeField] private BoolCondition condition;

        private void Awake()
        {
            healthUI.SetHealth(health);

            foreach(Transform child in partsPar.transform)
            {
                parts.Add(child.gameObject);
            }

            for(int i = level; i < parts.Count; i++)
            {
                parts[i].gameObject.SetActive(false);
            }
        }
    }
}