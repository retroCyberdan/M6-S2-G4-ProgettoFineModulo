using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    private bool gameEnded = false;

    void Start()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }

    public void WinLevel()
    {
        if (gameEnded) return;
        gameEnded = true;
        _winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoseLevel()
    {
        if (gameEnded) return;
        gameEnded = true;
        _losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
