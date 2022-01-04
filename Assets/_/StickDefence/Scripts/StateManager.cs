using DB.Utils;
using DB.War.Stack;
using DB.War.Stickman;
using PT.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Serializable]
    public class BaseState
    {
        public BaseStage[] stages;
    }

    [Serializable]
    public class BuildinStageState
    {
        public bool isActive;
        public bool isDead;
    }

    [Serializable]
    public class BuildingState
    {
        public bool active;
        public BuildinStageState[] stages;
        public int stageIndex;
    }

    [Serializable]
    public class Buildings
    {
        public BuildingState[] buildings;
    }

    public class StateManager : MonoBehaviour
    {
        public List<Upgradable> towers;
        public event Action OnHaveTowers;
        public UnityEvent OnStageUnlock;

        public void Save()
        {
            print("saving");

            // buildings
            List<BuildingState> states = new List<BuildingState>();
            foreach (Building b in buildings)
            {
                BuildingState bs = new BuildingState();
                bs.active = b.active;
                bs.stageIndex = b.stageIndex;

                List<BuildinStageState> stages = new List<BuildinStageState>();
                foreach (BuildingStage b2 in b.stageGOs)
                {
                    BuildinStageState bss = new BuildinStageState();
                    bss.isActive = b2.isActive;
                    bss.isDead = b2.isDead;
                    stages.Add(bss);
                }
                bs.stages = stages.ToArray();

                states.Add(bs);
            }
            BuildingState[] buildingsArr = states.ToArray();
            Buildings allB = new Buildings();
            allB.buildings = buildingsArr;

            string stringState = JsonUtility.ToJson(allB);
            PlayerPrefs.SetString("save_building", stringState);

            // base
            BaseState baseState = new BaseState();
            baseState.stages = stages;
            string base_state = JsonUtility.ToJson(baseState);
            PlayerPrefs.SetString("save_base", base_state);

            // wave
            PlayerPrefs.SetInt("wave", waveManager.done?1:0);
        }

        public void Load()
        {
            print("loading");

            // buildings
            string stringState = PlayerPrefs.GetString("save_building");

            if (stringState.Length > 0)
            {
                Buildings allB = JsonUtility.FromJson<Buildings>(stringState);

                int buildingIndex = 0;
                foreach (Building b in buildings)
                {
                    BuildingState buildingState = allB.buildings[buildingIndex];
                    b.active = buildingState.active;
                    b.stageIndex = buildingState.stageIndex;

                    int stageIndex = 0;
                    foreach (BuildingStage b2 in b.stageGOs)
                    {
                        BuildinStageState bss = buildingState.stages[stageIndex];
                        b2.isActive = bss.isActive;
                        b2.isDead = bss.isDead;

                        stageIndex++;
                    }
                    buildingIndex++;
                }
            }

            // base
            string base_state = PlayerPrefs.GetString("save_base");
            if (base_state.Length > 0)
            {
                BaseState baseState = JsonUtility.FromJson<BaseState>(base_state);
                int i = 0;
                foreach(BaseStage s in stages)
                {
                    s.need = baseState.stages[i].need;
                    s.supply = baseState.stages[i].supply;
                    s.isUnlocked = baseState.stages[i].isUnlocked;

                    int j = 0;
                    foreach(StagePart sp in s.children)
                    {
                        sp.isUnlocked = baseState.stages[i].children[j].isUnlocked;

                        j++;
                    }

                    i++;
                }
            }

            // wave
            waveManager.done = PlayerPrefs.GetInt("wave") > 0;
        }

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

                txt.text = stages[index].supply + "/" + stages[index].need;
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
                txt.transform.parent.gameObject.SetActive(true);
                txt.text = stages[index].supply + "/" + stages[index].need;
                unstackCondition.value = true;
            }
        }

        [SerializeField] private BaseStage[] stages;
        [SerializeField] private int index = 0;
        [SerializeField] private float upgradeDelay = 0.1f;
        [SerializeField] private BoolCondition unstackCondition;
        [SerializeField] private GameObject unstackerGO;
        [SerializeField] private WaveManager waveManager;
        [SerializeField] private TMPro.TextMeshPro txt;

        [SerializeField] private Transform[] spawnPoints, tankSpawnPoints, chopperSpawnPoints;
        [SerializeField] private SquadManager squadManager;
        [SerializeField] private CameraFollowerXZ cameraFollower;

        [SerializeField] private Building[] buildings;

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
                    Vector3 place = new Vector3(pos.x, go.transform.position.y, pos.z);
                    go.transform.position = place;
                    person.goalT.position = place;
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

            unstackerGO.GetComponent<Unstacker>().ForceExit();
            unstackerGO.SetActive(false);
            txt.transform.parent.gameObject.SetActive(false);

            CheckIndex();
            OnStageUnlock?.Invoke();
        }

        private void CheckIndex()
        {
            hasNext = index < stages.Length;
        }

        int buildingCap = 4, curActive = 0;
        private IEnumerator Tick()
        {
            while (true)
            {
                AddBuilding();
                Save();

                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            }
        }

        private void AddBuilding()
        {
            bool hasCandidate = true;
            while (curActive < buildingCap && hasCandidate)
            {
                Building min = buildings[0];
                hasCandidate = !min.active;

                foreach (Building b in buildings)
                {
                    if (b.stageIndex < min.stageIndex)
                    {
                        min = b;
                        hasCandidate = !min.active;
                    }
                }

                if (hasCandidate)
                {
                    if (min.GoToNextStage())
                    {
                        min.curStage.OnDestroyed += OnBuildingDestroyed;
                        curActive++;
                    }
                    else
                    {
                        hasCandidate = false;
                    }
                }
            }
        }

        private void OnBuildingDestroyed(BuildingStage stage)
        {
            stage.OnDestroyed -= OnBuildingDestroyed;
            curActive--;
        }

        private void Start()
        {
            // load the saves as JSON
            // TODO
            Load();

            // apply the save
            for (int i = 0; i < stages.Length; i++)
            {
                index = i;
                if (stages[i].isUnlocked)
                {
                    UnlockStage();

                    foreach (StagePart sp in stages[i].children)
                    {
                        if (sp.isUnlocked)
                        {
                            print("unlocked!");
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

            foreach(Building b in buildings)
            {
                b.SetUp();
                if (b.active)
                {
                    curActive++;
                    b.curStage.OnDestroyed += OnBuildingDestroyed;
                }
            }

            TimeManager.Instance.DoWithDelay(1f, () =>
            {
                AddBuilding();
            });

            StartCoroutine(Tick());
        }
    }
}