using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public Image img;
    public Sprite activeS, deactiveS;

    public void Activate()
    {
        img.sprite = activeS;
    }

    public void Deactivate()
    {
        img.sprite = deactiveS;
    }
}
