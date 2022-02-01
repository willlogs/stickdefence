using DB.War.Stickman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War
{
    public class WaveManager : MonoBehaviour
    {
        public UnityEvent OnWave;

        public void DoWave()
        {
            if (!done)
            {
                done = true;
                OnWave?.Invoke();
                StartCoroutine(DoWaveWithDelay());
            }
        }

        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int spawns = 6;
        [SerializeField] private float betweenSpawns = 1f;
        [SerializeField] private StateManager stateManager;
        [SerializeField] private Transform[] spawnPoints;

        public bool done;

        private void Awake()
        {
            stateManager.OnHaveTowers += DoWave;
        }

        private IEnumerator DoWaveWithDelay()
        {
            for(int i = 0; i < spawns; i++)
            {
                GameObject go = Instantiate(enemyPrefab);
                Enemy e = go.GetComponent<Enemy>();

                Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                go.transform.position = new Vector3(pos.x, go.transform.position.y, pos.z);
                e.SetTarget(stateManager.towers[0].transform);

                yield return new WaitForSeconds(betweenSpawns);
            }
        }
    }
}