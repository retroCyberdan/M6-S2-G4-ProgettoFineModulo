using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CardsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardsCounter;
    public static UI_CardsManager cardsManager;
    public int _cards;

    private void Awake()
    {
        if (!cardsManager) cardsManager = this;
    }

    private void OnGUI() => _cardsCounter.text = _cards.ToString();

    public void AddCards(int amount)
    {
        _cards += amount;
    }
}
