using DB.Utils;
using DB.War.Stack;
using DB.War.Stickman;
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
        public bool isUnlocked, isTower;
    }

    [Serializable]
    public class BaseStage
    {
        public int need;
        public int supply;
        public bool isUnlocked;

        public GameObject rewardPrefab;
        public int rewardNumber;
        public float scaleX = 1, scaleY = 1;

        public GameObject go;
        [SerializeField] public List<StagePart> children;
    }

    public class StateManager : MonoBehaviour
    {
        public List<Upgradable> towers;
        public event Action OnHaveTowers;

        // get one ammo box
        public void Upgrade()
        {
            if (stages[index].supply < stages[index].need)
            {
                stages[index].supply++;

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
            StagePart newUnlock = new StagePart();
            bool flag = false;
            foreach (StagePart sp in stage.children)
            {
                if (sp.upgradable == u)
                {
                    sp.isUnlocked = true;
                    flag = true;
                    newUnlock = sp;
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

            if (flag && newUnlock.isTower)
            {
                towers.Add(newUnlock.upgradable);
                OnHaveTowers?.Invoke();
            }

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
        [SerializeField] private WaveManager waveManager;

        [SerializeField] private Transform[] spawnPoints, tankSpawnPoints, chopperSpawnPoints;
        [SerializeField] private SquadManager squadManager;
        [SerializeField] private CameraFollowerXZ cameraFollower;

        private bool hasNext = true, waiting = false;

        private void UnlockStage()
        {
            stages[index].isUnlocked = true;
            stages[index].go.SetActive(true);
            BaseStage stage = stages[index];
            cameraFollower.ScaleOffset(stage.scaleX, stage.scaleY);
            if (stage.rewardPrefab != null)
            {
                for(int i = 0; i < stage.rewardNumber; i++)
                {
                    GameObject go = Instantiate(stage.rewardPrefab);
                    Stickman.Stickman person = go.GetComponent<Stickman.Stickman>();
                    Vector3 pos;
                    if (person.isTank)
                    {
                        pos = tankSpawnPoints[UnityEngine.Random.Range(0, tankSpawnPoints.Length)].position;
                    }else if (person.isChopper)
                    {
                        pos = chopperSpawnPoints[UnityEngine.Random.Range(0, chopperSpawnPoints.Length)].position;
                    }
                    else
                    {
                        pos = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
                    }
                    go.transform.position = new Vector3(pos.x, go.transform.position.y, pos.z);
                    squadManager.AddPerson(person);
                }
            }

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
            unstackerGO.GetComponent<Unstacker>().ForceExit();

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