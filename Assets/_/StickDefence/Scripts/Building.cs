using DB.Utils;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War
{
    public class Building : MonoBehaviour
    {
        public bool active;

        public bool GoToNextStage()
        {
            if (!active && stageIndex < stageGOs.Length)
            {
                curStage = stageGOs[stageIndex++];
                curStage.gameObject.SetActive(true);
                curStage.Activate();
                curStage.OnDestroyed += OnCurrentDied;

                active = true;
                return true;
            }

            return false;
        }

        public void SetUp()
        {
            active = false;
            foreach (BuildingStage bs in stageGOs)
            {
                if (!bs.isActive)
                {
                    bs.Deactivate();
                    bs.gameObject.SetActive(false);
                }
                else
                {
                    bs.gameObject.SetActive(true);
                    bs.Activate();
                    curStage = bs;
                    active = true;
                    bs.OnDestroyed += OnCurrentDied;
                    break;
                }
            }

            StartCoroutine(Tick());
        }

        [SerializeField] private StateManager stateManager;
        [SerializeField] public BuildingStage[] stageGOs;
        [SerializeField] public BuildingStage curStage;
        [SerializeField] public int stageIndex;

        private void OnCurrentDied(BuildingStage bs)
        {
            active = false;
            curStage.OnDestroyed -= OnCurrentDied;

            curStage.transform.DOScale(0, 1).OnComplete(() =>
            {
                curStage.gameObject.SetActive(false);
            });
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                if (active)
                    curStage.Tick();

                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 5f));
            }
        }
    }
}