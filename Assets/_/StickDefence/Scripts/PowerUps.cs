using DB.War.Stack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{
    public bool canRainBomb;

    public void Bomb()
    {
        for(int i = 0; i < 100; i++)
        {
            stacker.GetAmmoBox();
        }

        Instantiate(bombPrefab);
    }

    public void OnAddBox()
    {
        float amount = Mathf.Clamp01(((float)stacker.score) / 100f);
        canRainBomb = amount == 1f;
        ui.fillAmount = amount;
        uiBtn.interactable = canRainBomb;
        shine.enabled = canRainBomb;
    }

    [SerializeField] private Stacker stacker;
    [SerializeField] private Image ui, shine;
    [SerializeField] private Button uiBtn;
    [SerializeField] private GameObject bombPrefab;

    private void Start()
    {
        OnAddBox();
    }
}
