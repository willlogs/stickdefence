using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DB.War.Stickman;
using DB.Utils;

namespace DB.War.Weapons
{
    public class AutoShooter : MonoBehaviour
    {
        public void Enter(Collider other)
        {
            if (other.isTrigger)
                return;

            targets.Add(other.transform);

            Stickman.Stickman stick = other.gameObject.GetComponent<Stickman.Stickman>();
            if(stick != null)
            {
                stick.OnKilledByGun += OneGotKilled;
            }
            else
            {
                BuildingStage bs = other.gameObject.GetComponent<BuildingStage>();
                if(bs != null)
                {
                    bs.OnDestroyed += OnOneExploded;
                }
            }
        }

        public void OnOneExploded(BuildingStage bs)
        {
            targets.Remove(bs.transform);
        }

        public void OneGotKilled(Stickman.Stickman stick)
        {
            print("killed");
            targets.Remove(stick.transform);
            stick.OnKilledByGun -= OneGotKilled;
        }

        public void Exit(Collider other)
        {
            if (other.isTrigger)
                return;

            targets.Remove(other.transform);

            Stickman.Stickman stick = other.gameObject.GetComponent<Stickman.Stickman>();
            if (stick != null)
            {
                stick.OnKilledByGun -= OneGotKilled;
            }
        }

        [SerializeField] private List<Transform> targets;
        [SerializeField] private GunBase gun;
        [SerializeField] private Transform turret, aimT;
        [SerializeField] private bool turretRotation, resetTurret, shootLevel;
        [SerializeField] private BoolCondition hasTargetCondition;
        [SerializeField] private int forwMultiplier = 1;

        private Quaternion defTurretRot;

        private void Awake()
        {
            defTurretRot = turret.localRotation;
        }

        private void Update()
        {
            if (targets.Count > 0)
            {
                Transform t = targets[targets.Count - 1];
                if(t == null)
                {
                    targets.RemoveAt(targets.Count - 1);
                    return;
                }

                hasTargetCondition.value = true;
                if (turretRotation)
                {
                    Vector3 forw = forwMultiplier * (t.position - turret.position);
                    forw.y = 0;
                    turret.forward = forw;
                }

                aimT.position = new Vector3(t.position.x, shootLevel? aimT.position.y : t.position.y, t.position.z);
                gun.Shoot();
            }
            else
            {
                hasTargetCondition.value = false;
                if (turretRotation && resetTurret)
                {
                    turret.localRotation = Quaternion.Slerp(
                        turret.localRotation, defTurretRot, Time.deltaTime
                    );
                }
            }
        }
    }
}