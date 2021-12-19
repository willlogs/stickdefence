using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class Destroyer : MonoBehaviour
    {
        public void DoIt()
        {
            for(int i = 0; i < _objects.Length; i++)
            {
                Destroy(_objects[i]);
            }
        }

        [SerializeField] private GameObject[] _objects;
    }
}