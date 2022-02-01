using DB.War.Stickman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DB.Utils
{
    public class Indicator : MonoBehaviour
    {
        public void Activate(int colorIdx = 0)
        {
            isActive = true;
            img.color = colors[colorIdx];
        }

        public void Deactivate()
        {
            isActive = false;
        }

        public void SetTarget(Transform t)
        {
            target = t;
        }

        [SerializeField] private Transform target, pT;
        [SerializeField] private bool isActive;
        [SerializeField] private Image img;
        [SerializeField] private Sprite offScreen, onScreen;
        [SerializeField] private Color[] colors;
        
        Camera mc;

        private void Start()
        {
            mc = Camera.main;
            pT = FindObjectOfType<StickController>().transform;
        }

        private void Update()
        {
            if (isActive)
            {
                Indicate();
            }
        }

        private void Indicate()
        {
            Vector3 tp = target.position;
            tp.y = 7;
            Vector3 vp = mc.WorldToViewportPoint(tp);
            Vector3 clipped = new Vector3(
                Mathf.Clamp(vp.x, 0.1f, 0.9f),
                Mathf.Clamp(vp.y, 0.1f, 0.9f),
                0
            );

            vp.z = 0;
            bool isClipped = clipped.x != vp.x || clipped.y != vp.y;
            if (isClipped)
            {
                img.transform.right = -vp.normalized;
                img.sprite = offScreen;
                tp = (tp - pT.position).normalized * Screen.width * 0.8f;
                tp.y = tp.z;
                tp.z = 0;
                img.rectTransform.anchoredPosition = tp;
            }
            else
            {
                img.transform.right = Vector3.right;
                img.sprite = onScreen;
                img.rectTransform.position = mc.WorldToScreenPoint(tp);
            }
        }
    }
}