using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.StickDefence
{
    public class SpawnPoint : MonoBehaviour
    {
        public GameObject Spawn(){
            _particle.Stop();
            _particle.Play();

            GameObject go = Instantiate(_prefab);
            go.transform.position = transform.position;
            return go;
        }

        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private GameObject _prefab;
    }
}