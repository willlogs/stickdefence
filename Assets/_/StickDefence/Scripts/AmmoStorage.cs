using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DB.StickDefence
{
    public class AmmoStorage : MonoBehaviour
    {
        public Transform _base;
        public List<AmmoBox> _ammoBoxes = new List<AmmoBox>();
        public int score = 0;

        public void AddBox(AmmoBox ammox)
        {
            GameObject par = ammox.transform.parent.gameObject;
            ammox.transform.parent = null;
            ammox.Get();
            Destroy(par);
            ammox.GetStored(StoreBox, _base);            
        }

        private void StoreBox(AmmoBox ammox){
            Transform nextpos = _base;
            if (_ammoBoxes.Count > 0)
            {
                nextpos = _ammoBoxes[_ammoBoxes.Count - 1].nextPosition;
            }
            _ammoBoxes.Add(ammox);
            ammox.transform.position = nextpos.position;
            ammox.transform.rotation = _base.transform.rotation;
            ammox.transform.parent = _base;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 11)
            {
                AmmoBox ammox = other.gameObject.GetComponent<AmmoBox>();
                if (ammox != null)
                {
                    AddBox(ammox);
                }
            }
        }
    }
}