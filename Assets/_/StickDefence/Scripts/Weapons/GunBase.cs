using PT.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DB.War.Weapons
{
    public class GunBase : MonoBehaviour
    {
        public UnityEvent OnShoot;

        public void Shoot()
        {
            if (canShoot)
            {
                GameObject go = Instantiate(bulletPrefab);
                go.transform.position = bulletPointT.position;
                go.GetComponent<BulletBase>().GetShot(gunTargetT.position - go.transform.position);
                OnShoot?.Invoke();
                canShoot = false;
                TimeManager.Instance.DoWithDelay(restBetweenShots, () =>
                {
                    canShoot = true;
                });
            }
        }

        public void Shoot(Transform target)
        {
            if (canShoot)
            {
                GameObject go = Instantiate(bulletPrefab);
                go.transform.position = bulletPointT.position;
                go.GetComponent<BulletBase>().GetShot(
                    gunTargetT.position - go.transform.position,
                    target
                );
                OnShoot?.Invoke();
                canShoot = false;
                TimeManager.Instance.DoWithDelay(restBetweenShots, () =>
                {
                    canShoot = true;
                });
            }
        }

        [SerializeField] private float restBetweenShots = 0.4f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform gunTargetT, bulletPointT;

        bool canShoot = true;
    }
}