using DB.War.Stack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public bool canRainBomb;

    public void OnAddBox()
    {
        float amount = Mathf.Clamp01(((float)stacker.score) / 50f);
        canRainBomb = amount == 1f;
        ui.fillAmount = amount;
        uiBtn.interactable = canRainBomb;
    }

    [SerializeField] private Stacker stacker;
    [SerializeField] private Image ui;
    [SerializeField] private Button uiBtn;

    private void Start()
    {
        OnAddBox();
    }
}
