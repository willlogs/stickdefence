using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class TransformActions : MonoBehaviour
    {
        public void SetParentToNull()
        {
            transform.parent = null;
        }

        public void CopyPosition(Transform _ref)
        {
            transform.position = _ref.position;
        }

        public void CopyRotation(Transform _ref)
        {
            transform.rotation = _ref.rotation;
        }

        [SerializeField] private Transform _reference;
        [SerializeField] private bool _copyPositionInUpdate, _copyRotationInUpdate;

        private void Update()
        {
            if (_copyPositionInUpdate)
                CopyPosition(_reference);

            if(_copyRotationInUpdate)
                CopyRotation(_reference);
        }
    }
}