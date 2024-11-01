using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using BaseAssets;

public class GameManager : MonoBehaviour
{
    public static Action OnPanelActivated;

    public static GameManager Instance { get; private set; }

    public static float VehicleSpeed => _vehicleSpeed;

    public static int NumberOfMoves;

    public int CompletedGroupCount
    {
        get
        {
            return _completedGroupCount;
        }
        set
        {
            if (value > 0)
            {
                _completedGroupCount = value;

                if (_completedGroupCount == _truckGroupCount)
                {
                    OnPanelActivated?.Invoke();

                    _gamePanel.SetActive(false);
                    ActivatePanel(_completedPanel);
                }
            }
        }
    }
    
    [Header("Texts")]
    [SerializeField] TMP_Text _moveCountText;
    [SerializeField] TMP_Text _levelText;

    [Header("Panels")]
    [SerializeField] GameObject _gamePanel;
    [SerializeField] GameObject _completedPanel;
    [SerializeField] GameObject _failedPanel;

    [Header("Values")]
    [OnValueChanged("ChangeMoveCount")]
    [SerializeField] int _numberOfMoves;

    [SerializeField] int _targetFrameRate = 60;

    const float _vehicleSpeed = 10f;
    
    static int _coin;
    static bool _isLevelsCompleted;
    static int _currentLevelIndex;

    int _completedGroupCount = 0;
    int _truckGroupCount = 0;
    int _releasedGroupCount = 0;
    int _sceneIndex;

    TruckGroupQueue _queue;

    private void Awake()
    {
        Instance = this;
        _queue = FindAnyObjectByType<TruckGroupQueue>();

        _sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if(_isLevelsCompleted) return;
        _currentLevelIndex = _sceneIndex;
    }

    private void Start()
    {
        ChangeFPSRate();      
        ChangeMoveCount();

        if (_isLevelsCompleted)
        {
            _currentLevelIndex++;
            _levelText.text = "Level " + _currentLevelIndex.ToString();
        }
        else
        {
            _levelText.text = "Level " + _sceneIndex.ToString();
        }
    }

    public void DecreaseMoveCount(int decreaseCount)
    {
        if (_queue.IsOnLevelDesign) return;

        NumberOfMoves -= decreaseCount;
        _moveCountText.text = "MOVES: " + NumberOfMoves.ToString();

        if (NumberOfMoves <= 0)
        {
            StartCoroutine(CheckFailedWithDelay());
        }

        IEnumerator CheckFailedWithDelay()
        {
            yield return new WaitForSeconds(1f);

            if (_releasedGroupCount != _truckGroupCount)
            {
                OnPanelActivated?.Invoke();

                _gamePanel.SetActive(false);
                ActivatePanel(_failedPanel);
            }
        }
    }

    public void RestartScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (SceneManager.sceneCountInBuildSettings == sceneIndex + 1)
        {
            _isLevelsCompleted = true;
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex + 1);
        }
    }

    [Button]
    void ChangeFPSRate()
    {
        Application.targetFrameRate = _targetFrameRate;
    }

    public void IncreaseTruckGroupCount(int increaseRate = 1)
    {
        _truckGroupCount += increaseRate;
    }

    void ChangeMoveCount()
    {
        NumberOfMoves = _numberOfMoves;
        _moveCountText.text = "MOVES: " + NumberOfMoves.ToString();
    }

    void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<CoinPanel>().ChangeCoinText(_coin);
    }

    public void IncreaseCoin()
    {
        Increase(250);

        static void Increase(int increaseCount)
        {
            _coin += increaseCount;
        }
    }

    public void IncreaseReleasedGroupCount(int increaseRate = 1)
    {
        _releasedGroupCount += increaseRate;
    }
}
