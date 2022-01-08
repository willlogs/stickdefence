using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace DB.War.Stack
{
    public class Stacker : MonoBehaviour
    {
        public int targetEvent = 10;
        public UnityEvent OnGatherBox, OnReduceBox, OnReachTarget, OnReachZero;

        public void GatherBox(Collider box)
        {
            box.enabled = false;
            box.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(BringBoxHome(box.transform));
        }

        // get a position and toss a dummy ammox towards that position
        [Button]
        public bool GetAmmoBox()
        {
            if(score <= 0)
            {
                score = 0;
                OnReachZero?.Invoke();
                return false;
            }

            if (score == 1)
            {
                firstAmmoBox.gameObject.SetActive(false);
                score--;
                OnReduceBox?.Invoke();
                return true;
            }
            else
            {
                stack[score - 2].gameObject.SetActive(false);
                score--;
                OnReduceBox?.Invoke();
                return true;
            }
        }

        public bool GetAmmoBox(Transform target)
        {
            bool res = GetAmmoBox();
            if (res)
            {
                GameObject nextDummy = dummies[0];
                foreach(GameObject go in dummies)
                {
                    if (!go.activeSelf)
                    {
                        nextDummy = go;
                    }
                }

                nextDummy.SetActive(true);
                nextDummy.transform.position = transform.position;
                nextDummy.transform.DOMove(target.position, 0.5f).OnComplete(() =>
                {
                    nextDummy.SetActive(false);
                });
            }

            return res;
        }

        [Button]
        public void AddAmmoBox(bool init = false)
        {
            if(score == 0)
            {
                firstAmmoBox.gameObject.SetActive(true);
                score++;
                return;
            }

            if(score - 1 < stack.Count)
            {
                stack[score - 1].gameObject.SetActive(true);
                GameObject go = stack[score - 1].gameObject;
                AmmoBox lastAmmoBox = score > 1 ? stack[score - 2] : firstAmmoBox;
                go.transform.position = lastAmmoBox.transform.position;
                go.transform.rotation = lastAmmoBox.transform.rotation;
            }
            else
            {
                GameObject go = Instantiate(ammoBoxPrefab);
                
                AmmoBox lastAmmoBox = score > 1 ? stack[stack.Count - 1] : firstAmmoBox;

                go.transform.position = lastAmmoBox.transform.position;
                go.transform.rotation = lastAmmoBox.transform.rotation;
                go.transform.parent = dummyParent;

                AmmoBox newBox = go.GetComponent<AmmoBox>();
                stack.Add(newBox);
                newBox.SetParent(lastAmmoBox);
            }
            score++;
            OnGatherBox?.Invoke();

            if(score == targetEvent && !init)
            {
                OnReachTarget?.Invoke();
            }
        }

        [SerializeField] private AmmoBox firstAmmoBox;
        [SerializeField] private List<AmmoBox> stack;
        [SerializeField] private List<GameObject> dummies;
        [SerializeField] public int score = 0;
        [SerializeField] private int warmup = 50, dummyWarmup = 50;
        [SerializeField] private GameObject ammoBoxPrefab, dummyBoxPrefab;

        private Transform dummyParent;

        private void Awake()
        {
            dummyParent = new GameObject().transform;
            for(int i = 0; i < warmup; i++)
            {
                AddAmmoBox(true);
            }

            for(int i = 0; i < dummyWarmup; i++)
            {
                GameObject go = Instantiate(dummyBoxPrefab);
                dummies.Add(go);
                go.transform.parent = dummyParent;
                go.SetActive(false);
            }

            for (int i = 0; i < warmup; i++)
            {
                GetAmmoBox();
            }
            GetAmmoBox();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = 0; i < warmup; i++)
                {
                    AddAmmoBox();
                }
            }
        }

        private IEnumerator BringBoxHome(Transform boxT)
        {
            float mag = (boxT.position - transform.position).magnitude;
            float t = 0;
            while(mag > 1f)
            {
                boxT.position = Vector3.Lerp(
                    boxT.position,
                    transform.position,
                    t
                );
                t += Time.deltaTime * 2;
                mag = (boxT.position - transform.position).magnitude;
                yield return new WaitForEndOfFrame();
            }
            Destroy(boxT.gameObject);
            AddAmmoBox();
        }
    }
}