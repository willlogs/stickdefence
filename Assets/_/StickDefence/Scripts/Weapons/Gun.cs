using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DB.StickDefence.Weapons
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletpoint;
        [SerializeField] private float _betweenShots;

        private bool _canShoot = false;

        [Button]
        public void Shoot(Vector3 target)
        {
            if (_canShoot)
            {
                target.y = _bulletpoint.position.y;
                GameObject bulletGO = Instantiate(_bulletPrefab);
                bulletGO.transform.position = _bulletpoint.position;

                Bullet b = bulletGO.GetComponent<Bullet>();
                b.GetShot(target - bulletGO.transform.position);
                StartCoroutine(Rest());
            }
        }

        private void Start()
        {
            StartCoroutine(Rest());
        }

        private IEnumerator Rest()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_betweenShots);
            _canShoot = true;
        }
    }
}