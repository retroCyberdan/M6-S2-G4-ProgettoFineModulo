using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    private bool _gameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        InitializePanels();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // reset dello stato del gioco quando viene caricata una nuova scena
        _gameEnded = false;
        Time.timeScale = 1f;

        // trova e assegna i nuovi pannelli nella scena caricata
        FindPanelsInScene();
        InitializePanels();
    }

    private void FindPanelsInScene() // <- cerca i pannelli nella nuova scena se non sono già assegnati
    {
        if (_winPanel == null)
        {
            GameObject winPanelObj = GameObject.Find("WinPanel");
            if (winPanelObj != null) _winPanel = winPanelObj;
        }

        if (_losePanel == null)
        {
            GameObject losePanelObj = GameObject.Find("LosePanel");
            if (losePanelObj != null) _losePanel = losePanelObj;
        }
    }

    private void InitializePanels()
    {
        if (_winPanel != null) _winPanel.SetActive(false);

        if (_losePanel != null) _losePanel.SetActive(false);
    }

    public void WinLevel()
    {
        if (_gameEnded) return;
        _gameEnded = true;

        if (_winPanel != null)
        {
            _winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void LoseLevel()
    {
        if (_gameEnded) return;
        _gameEnded = true;

        if (_losePanel != null)
        {
            _losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
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