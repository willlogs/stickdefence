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
            if (shortCircuit)
            {
                goalT.position = location;
                return;
            }

            location.y = 0;
            location += (transform.position - location).normalized * stopDistance;
            path = new NavMeshPath();
            int ac = 1 << areaCode;
            NavMesh.CalculatePath(transform.position, location, areaCode > 0 ? ac : NavMesh.AllAreas, path);
            if(path.corners.Length > 1)
            {
                cornerIdx = 1;
                isWaitingForNextCorner = true;
                GoToNextCorner();
            }else if(bypassObstacle)
            {
                goalT.position = location;
            }
        }

        public void SetAutoTravel(Transform t, float dis)
        {
            stopDistance = dis;
            target = t;
            hasTarget = true;
            StopAllCoroutines();
            StartCoroutine(PathfindAuto());
        }

        [SerializeField] private float betweenPathFinds = 1f, stopDistance = 0.1f;
        [SerializeField] private bool autoPathfindFromStart = false, shortCircuit, bypassObstacle;
        [SerializeField] private Transform target;

        [SerializeField] private Transform goalT;
        [SerializeField] private Stickman stickman;
        [SerializeField] private int areaCode;

        private bool isWaitingForNextCorner, hasTarget;
        private int cornerIdx = 0;
        private NavMeshPath path;

        private IEnumerator PathfindAuto()
        {
            while (hasTarget)
            {
                yield return new WaitForSeconds(betweenPathFinds);
                if(target != null)
                    GoToLocation(target.position);
            }
        }

        private void Awake()
        {
            goalT.position = transform.position;

            stickman.OnGoalReached += OnGoalReached;
            if (autoPathfindFromStart)
            {
                SetAutoTravel(target, 0.1f);
            }
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