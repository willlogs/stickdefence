using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Utils
{
    public class RendererActions : MonoBehaviour
    {
        public void SetMaterialToDest()
        {
            // TODO: enable for multiple mats on single renderer
            _renderer.material = _destinationMat;
        }

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _destinationMat;
    }
}