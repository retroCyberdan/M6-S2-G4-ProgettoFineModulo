using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _currentHp = 20;
    [SerializeField] private int _maxHp = 20;
    [SerializeField] private bool _fullHpOnAwake = true;

    [SerializeField] private UnityEvent<int, int> _onHpChanged;
    [SerializeField] private UnityEvent _onDeath;

    public int GetHp() => _currentHp;
    public int GetMaxHp() => _maxHp;

    private void Awake()
    {
        if(_fullHpOnAwake) SetHp(_maxHp);
    }

    public void SetHp(int hp)
    {
        int oldHp = _currentHp;

        hp = Mathf.Clamp(hp, 0, _maxHp);

        _currentHp = hp;

        if(oldHp != _currentHp)
        {
            _onHpChanged?.Invoke(_currentHp, _maxHp);
            if (_currentHp == 0) Die();
        }        
    }

    public void AddHp(int amount) => SetHp(_currentHp + amount);

    private void Die()
    {
        Debug.Log($"Il personaggio {gameObject.name} è morto!");
        _onDeath?.Invoke();
        FindObjectOfType<GameManager>().LoseLevel();
    }
}
