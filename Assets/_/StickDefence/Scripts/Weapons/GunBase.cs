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
                GameObject go;
                BulletBase b;
                MakeBullet(out go, out b);
                go.transform.position = bulletPointT.position;
                go.SetActive(true);

                b.GetShot(gunTargetT.position - go.transform.position);
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
                GameObject go;
                BulletBase b;
                MakeBullet(out go, out b);
                go.transform.position = bulletPointT.position;
                go.SetActive(true);

                b.GetShot(
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
        [SerializeField] private int poolSize = 20, poolIndex = 0;
        [SerializeField] private List<BulletBase> bulletPool;

        bool canShoot = true;

        private void Start()
        {
            for(int i = 0; i < poolSize; i++)
            {
                GameObject go = Instantiate(bulletPrefab);
                go.transform.position = bulletPointT.position;
                BulletBase b = go.GetComponent<BulletBase>();
                go.SetActive(false);
                bulletPool.Add(b);
            }
        }

        private void MakeBullet(out GameObject go, out BulletBase b)
        {
            b = bulletPool[poolIndex];
            go = b.gameObject;
            poolIndex = (poolIndex + 1) % poolSize;
        }

        private void OnDestroy()
        {
            foreach(BulletBase b in bulletPool)
            {
                try
                {
                    Destroy(b.gameObject);
                }
                catch { }
            }
        }
    }
}