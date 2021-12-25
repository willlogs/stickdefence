using DB.Utils;
using DB.War.Stack;
using PT.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.War
{
    [Serializable]
    public class StagePart
    {
        public GameObject go;
        public Upgradable upgradable;
        public bool isUnlocked;
    }

    [Serializable]
    public class BaseStage
    {
        public int need;
        public int supply;
        public bool isUnlocked;

        public GameObject go;
        [SerializeField] public List<StagePart> children;
    }

    public class StateManager : MonoBehaviour
    {
        // get one ammo box
        public void Upgrade()
        {
            if (stages[index].supply < stages[index].need)
            {
                stages[index].supply++;
                print(stages[index].supply + "/" + stages[index].need);

                if (stages[index].supply >= stages[index].need)
                {
                    UnlockStage();
                }
                else
                {
                    TimeManager.Instance.DoWithDelay(upgradeDelay, () =>
                    {
                        unstackCondition.value = true;
                    });
                }
            }
        }

        public void OneUpgraded(Upgradable u)
        {
            int unlockedCount = 0;
            BaseStage stage = stages[index - 1];
            foreach (StagePart sp in stage.children)
            {
                if (sp.upgradable == u)
                {
                    sp.isUnlocked = true;
                    unlockedCount++;
                    // call enemy wave if needed
                }
                else
                {
                    if (sp.isUnlocked)
                        unlockedCount++;
                }
            }
            u.OnFullyUpgradedE -= OneUpgraded;

            if(unlockedCount >= stage.children.Count)
            {
                StopWaiting();
            }
        }

        public void StopWaiting()
        {
            waiting = false;
            if (hasNext)
            {
                unstackerGO.SetActive(true);
                unstackCondition.value = true;
            }
        }

        [SerializeField] private BaseStage[] stages;
        [SerializeField] private int index = 0;
        [SerializeField] private float upgradeDelay = 0.1f;
        [SerializeField] private BoolCondition unstackCondition;
        [SerializeField] private GameObject unstackerGO;

        private bool hasNext = true, waiting = false;

        private void UnlockStage()
        {
            stages[index].isUnlocked = true;
            stages[index].go.SetActive(true);
            foreach (StagePart sp in stages[index].children)
            {
                if (sp.isUnlocked)
                {
                    sp.upgradable.Unlock();
                }
                sp.upgradable.OnFullyUpgradedE += OneUpgraded;
            }
            index++;
            waiting = true;
            unstackerGO.SetActive(false);
            CheckIndex();
        }

        private void CheckIndex()
        {
            hasNext = index < stages.Length;
        }

        private void Start()
        {
            // load the saves as JSON
            // TODO

            // apply the save
            for (int i = 0; i < stages.Length; i++)
            {
                index = i;
                if (stages[i].isUnlocked)
                {
                    stages[i].go.SetActive(true);
                    foreach (StagePart sp in stages[i].children)
                    {
                        if (sp.isUnlocked)
                        {
                            sp.upgradable.Unlock();
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            CheckIndex();
        }
    }
}