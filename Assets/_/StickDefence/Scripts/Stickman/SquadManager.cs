using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DB.War.Stickman
{
    public class SquadManager : MonoBehaviour
    {
        [Button]
        public void AddPerson(Stickman person)
        {
            _crowd.Add(person);
            PlaceCrowd();
        }

        [SerializeField] private List<Stickman> _crowd = new List<Stickman>();
        [SerializeField] private Transform _leader;

        [FoldoutGroup("Placor")]
        [SerializeField] private float _radiusIncrease = 0.3f;
        [FoldoutGroup("Placor")]
        [SerializeField] private int _density = 4;

        [Button]
        private void PlaceCrowd()
        {
            int i = 0;
            for (int rimIndex = 1; i < _crowd.Count; rimIndex++)
            {
                int density = _density * rimIndex;
                int inrimIndex = 0;

                while (true)
                {
                    // set values
                    int rimOffset = inrimIndex++ % density;
                    float time = (float)rimOffset / (float)density;
                    // calculate offset
                    Quaternion rotator = Quaternion.Euler(0, time * 360f, 0);
                    Vector3 offset = rotator * (transform.forward * _radiusIncrease * rimIndex);

                    // place them
                    _crowd[i].mainGoalT.parent = transform;
                    _crowd[i].mainGoalT.position = offset + transform.position;

                    // continue loop
                    i++;
                    if (inrimIndex >= density || i >= _crowd.Count)
                    {
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
    }
}