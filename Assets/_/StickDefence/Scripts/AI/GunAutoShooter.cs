using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DB.StickDefence;
using DB.StickDefence.Weapons;

namespace DB.StickDefence.AI
{
    public class GunAutoShooter : MonoBehaviour
    {
        public void SetSquad(SquadManager squad){
            _squad = squad;
            _hasSquad = true;
        }

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Gun _gun;
        [SerializeField] private List<Transform> _targets = new List<Transform>();
        [SerializeField] private SquadManager _squad;
        [SerializeField] private bool _hasSquad = false;

        private bool _listLock = false;

        private void OnTriggerEnter(Collider other){
            if(((1 << other.gameObject.layer) & _layerMask.value) > 0){
                _targets.Add(other.transform);
            }
        }

        private void OnTriggerExit(Collider other){
            if(((1 << other.gameObject.layer) & _layerMask.value) > 0){
                if(_targets.Contains(other.transform)){
                    _targets.Remove(other.transform);
                }
            }
        }

        private void Update(){
            if(_targets.Count > 0){
                for(int i = _targets.Count - 1; i >= 0; i--){
                    if(_targets[i] == null || ((1 << _targets[i].gameObject.layer) & _layerMask.value) == 0){
                        // went out
                        _targets.RemoveAt(i);
                    }
                }
            }

            if(_targets.Count > 0){
                // shoot the first one!
                Transform target = _targets[0];
                
                if(_hasSquad){
                    _gun.Shoot(target.position, _squad);
                }
                else{
                    _gun.Shoot(target.position);
                }
            }
        }
    }
}