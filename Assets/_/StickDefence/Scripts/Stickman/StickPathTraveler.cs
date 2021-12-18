using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DB.War.Stickman
{
    public class StickPathTraveler : MonoBehaviour
    {
        [Button]
        public void GoToLocation(Vector3 location)
        {
            path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, location, NavMesh.AllAreas, path);
            if(path.corners.Length > 1)
            {
                cornerIdx = 1;
                isWaitingForNextCorner = true;
                GoToNextCorner();
            }
        }

        [SerializeField] private Transform goalT;
        [SerializeField] private Stickman stickman;

        private bool isWaitingForNextCorner;
        private int cornerIdx = 0;
        private NavMeshPath path;

        private void Awake()
        {
            stickman.OnGoalReached += OnGoalReached;
        }

        private void OnGoalReached()
        {
            if (isWaitingForNextCorner)
            {
                GoToNextCorner();
            }
        }

        private void GoToNextCorner()
        {
            if (cornerIdx < path.corners.Length)
            {
                goalT.position = path.corners[cornerIdx];
                cornerIdx++;
            }
            else
            {
                isWaitingForNextCorner = false;
            }
        }
    }
}