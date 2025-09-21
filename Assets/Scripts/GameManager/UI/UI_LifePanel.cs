using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifePanel : MonoBehaviour
{
    [SerializeField] private Image _fillableLifeBar;
    [SerializeField] private TMP_Text _lifeText;

    public void UpdateGraphics(int currentHp, int maxHp)
    {
       _lifeText.text = "HP " + currentHp + "/" + maxHp;
       _fillableLifeBar.fillAmount = (float)currentHp / maxHp;
    }    
}