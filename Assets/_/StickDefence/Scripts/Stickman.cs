using System.Collections;
using System.Collections.Generic;
using DB.StickDefence.AI;
using DB.StickDefence.Weapons;
using UnityEngine;

namespace DB.StickDefence
{
    public class Stickman : MonoBehaviour
    {
        [SerializeField] public Transform _movementGoal;
        [SerializeField] public Gun _gun;

        public void Die()
        {
            _animator.SetTrigger("Die");
            _isDead = true;
        }

        public void SetSquad(SquadManager squad)
        {
            _gun.gameObject.SetActive(true);
            _squad = squad;
            _hasSquad = true;
            _renderer.material = squad.squadMat;
            gameObject.layer = LayerMask.NameToLayer("Player");
            _animator.SetBool("Pray", false);

            try
            {
                GetComponentInChildren<GunAutoShooter>().SetSquad(squad);
            }
            catch { }
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private float _movementSpeed = 1f;
        [SerializeField] private SquadManager _squad;
        [SerializeField] private bool _hasSquad;
        [SerializeField] private Renderer _renderer;

        private bool _isMoving, _shouldMove, _isDead;

        private void Start()
        {

        }

        private void Update()
        {
            if (_isDead)
                return;

            Vector3 diff = _movementGoal.position - transform.position;
            diff.y = 0;
            _shouldMove = diff.magnitude > 0.1f;

            if (_shouldMove && !_isMoving)
            {
                // start moving
                _animator.SetBool("Run", true);
                _isMoving = true;
            }

            if (!_shouldMove && _isMoving)
            {
                // stop moving
                _animator.SetBool("Run", false);
                _isMoving = false;
            }

            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + diff,
                    Time.unscaledDeltaTime * _movementSpeed
                );

                transform.forward = diff;
            }
        }
    }
}