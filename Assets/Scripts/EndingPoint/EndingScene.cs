using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour
{
    [SerializeField] private int _requiredCards = 3;
    [SerializeField] private UI_CardsManager _cardsCollector;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && _cardsCollector._cards == _requiredCards)
        {
            FindObjectOfType<GameManager>().WinLevel();
        }
    }
}