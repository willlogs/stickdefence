using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT.Utils
{
    public class TFPS : MonoBehaviour
    {
        void Awake(){
            Application.targetFrameRate = 60;
        }
    }
}