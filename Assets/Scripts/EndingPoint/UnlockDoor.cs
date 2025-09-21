using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [Header("Door Parameters")]
    [SerializeField] private int _requiredCards = 3;
    [SerializeField] private UI_CardsManager _cardsCollector;
    [SerializeField] private GameObject _doorObject;

    void Update()
    {
        void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player") && _cardsCollector._cards == _requiredCards)
            {
                Destroy(_doorObject);
                this.enabled = false;
            }
        }
    }
}