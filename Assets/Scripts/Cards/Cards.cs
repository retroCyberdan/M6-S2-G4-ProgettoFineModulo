using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cards : MonoBehaviour
{
    [SerializeField] private int _value;
    private bool _hasTriggered;

    private UI_CardsManager _cardsManager;

    private void Start()
    {
        _cardsManager = UI_CardsManager.cardsManager;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            _cardsManager.AddCards(_value);
            Destroy(gameObject);
        }
    }
}
