using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifeTime = 3f;

    private Coroutine _deactivationCoroutine;

    void Start()
    {
        Destroy(gameObject, _lifeTime); // <- distrugge il bullet dopo un certo tempo 
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            LifeController life = collider.GetComponent<LifeController>();
            life.AddHp(-_damage);
            //Destroy(gameObject);
            gameObject.SetActive(false); // <- disattiva il bullet anzichè distruggerlo
        }
    }

    void OnEnable()
    {
        _deactivationCoroutine = StartCoroutine(DeactivateAfterTime()); // <- quando il bullet viene attivato, parte il timer per disattivarlo
    }

    void OnDisable()
    {
         if (_deactivationCoroutine != null) StopCoroutine(_deactivationCoroutine); // <- ferma la coroutine se il bullet viene disattivato manualmente
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
}