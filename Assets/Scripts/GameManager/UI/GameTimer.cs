using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float _timerLimit = 60f;
    private float _timer;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Transform _player;
    private Vector3 _startingPosition;

    void Start()
    {
        _timer = _timerLimit;
        _startingPosition = _player.position;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        UpdateTimerUI();

        if (_timer <= 0f)
        {
            FindObjectOfType<GameManager>().LoseLevel();
            //RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateTimerUI()
    {
        if (_timerText != null)
        {
            _timerText.text = "00:00:" + Mathf.Ceil(_timer).ToString();
        }
    }
}