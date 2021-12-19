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
        public UnityEvent OnGatherBox;

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
                return false;
            }

            if (score == 1)
            {
                firstAmmoBox.gameObject.SetActive(false);
                score--;
                return true;
            }
            else
            {
                stack[score - 2].gameObject.SetActive(false);
                score--;
                return true;
            }
        }

        [Button]
        public void AddAmmoBox()
        {
            print(score);
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
        }

        [SerializeField] private AmmoBox firstAmmoBox;
        [SerializeField] private List<AmmoBox> stack;
        [SerializeField] private int score = 0;
        [SerializeField] private int warmup = 50;
        [SerializeField] private GameObject ammoBoxPrefab;

        private Transform dummyParent;

        private void Awake()
        {
            dummyParent = new GameObject().transform;
            for(int i = 0; i < warmup; i++)
            {
                AddAmmoBox();
            }

            for (int i = 0; i < warmup; i++)
            {
                GetAmmoBox();
            }
            GetAmmoBox();
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