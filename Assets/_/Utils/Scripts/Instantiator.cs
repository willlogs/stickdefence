using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class Instantiator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        public void Instantiate_()
        {
            GameObject go = Instantiate(_prefab);
            go.transform.position = transform.position;
            go.transform.parent = null;
        }
    }
}