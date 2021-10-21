using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DB.StickDefence
{
    public class SquadManager : MonoBehaviour
    {
        public Material squadMat;

        // TODO: add mutual parent to all squad members
        [Button]
        public void AddPeople(int number)
        {
            for (int i = 0; i < number; i++)
            {
                GameObject newGuy = Instantiate(_stickmanPrefab);
                newGuy.transform.position = transform.position;
                Stickman stickman = newGuy.GetComponent<Stickman>();
                stickman.SetSquad(this);
                _crowd.Add(stickman);
            }

            PlaceCrowd();
        }

        public void AddPerson(Stickman person){
            person.SetSquad(this);
            _crowd.Add(person);
            PlaceCrowd();
        }

        [SerializeField] private GameObject _stickmanPrefab;
        [SerializeField] private List<Stickman> _crowd = new List<Stickman>();
        [SerializeField] private Transform _leader;
        [SerializeField] private AmmoStorage _ammoStorage;

        [FoldoutGroup("Placor")]
        [SerializeField] private float _radiusIncrease = 0.3f;
        [FoldoutGroup("Placor")]
        [SerializeField] private int _density = 4;

        [Button]
        private void PlaceCrowd()
        {
            int i = 0;
            for(int rimIndex = 1; i < _crowd.Count; rimIndex++){
                int density = _density * rimIndex;
                int inrimIndex = 0;

                while(true){
                    // set values
                    int rimOffset = inrimIndex++ % density;
                    float time = (float)rimOffset / (float)density;
                    // calculate offset
                    Quaternion rotator = Quaternion.Euler(0, time * 360f, 0);
                    Vector3 offset = rotator * (transform.forward * _radiusIncrease * rimIndex);
                    // place them
                    _crowd[i]._movementGoal.parent = transform;
                    _crowd[i]._movementGoal.position = offset + transform.position;
                    // continue loop
                    i++;
                    if(inrimIndex >= density || i >= _crowd.Count){
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            transform.parent = null;
        }

        private void Update()
        {
            transform.position = _leader.position;
        }

        private void OnTriggerEnter(Collider other){
            if(other.gameObject.layer == LayerMask.NameToLayer("Hostage")){
                Stickman stickman = other.gameObject.GetComponent<Stickman>();
                if(stickman != null){
                    AddPerson(stickman);
                }
            }
        }
    }
}