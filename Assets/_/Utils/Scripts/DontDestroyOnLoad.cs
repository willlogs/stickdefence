using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static DontDestroyOnLoad instance;
        [SerializeField] private bool _noOtherVersion;

        private void Start()
        {
            if (_noOtherVersion)
            {
                if (instance == null)
                {
                    instance = this;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            
            DontDestroyOnLoad(this);
        }
    }
}