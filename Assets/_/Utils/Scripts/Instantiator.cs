using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class Instantiator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int count = 1;
        [SerializeField] private float randomPosRange = 0;

        public void Instantiate_()
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(_prefab);
                go.transform.parent = null;
                go.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * randomPosRange;
            }
        }
    }
}