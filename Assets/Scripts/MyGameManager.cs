using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MyGameManager : MonoBehaviour {
    public Button[] cells;
    public Button playAgainBtn;
    public Button mainMenuBtn;
    public Text levelText;
    public Text timeText;
    public Text scoreText;
    public AudioSource shortTimeAudioSource;
    public GameObject gameOver;
    public GameObject gamePanel;
    
    private bool _isGameStarted;
    private float _startTime;
    private float _remainingTime;
    private int _indexToAnimate;
    private List<int> _randomPathList;
    private Queue<int> _pathQueue;
    private UnityAction[] _actions;
    private float _timePerCell;
    private bool _isShortTime;
    private bool _isGameOver;
    private int _loseTimes;
    
    private const float PlayerTime = 10f;

    public void RewardedExtraTime() {
        _remainingTime += 5f;
    }
    
    private void ShortTimeLeftEvent() {
        if (_isShortTime) {
            return;
        }
        
        shortTimeAudioSource.Play();
        _isShortTime = true;
        timeText.color = Color.red;
    }
    
    private void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = $"Timer: {minutes:00}:{seconds:00}";
    }
    
    private void DisplayLevel(int levelToDisplay) {
        levelText.text = string.Format("Level: " + levelToDisplay);
    }
    
    public void AnimateCell() {
        if (_indexToAnimate < _randomPathList.Count) { // animate
            var index = _randomPathList[_indexToAnimate];
            cells[index].GetComponent<Animator>().Play("Path", -1, 0f);
            cells[index].GetComponent<AudioSource>().Play();
            _indexToAnimate++;
        }
        else { // enable buttons after showing the full path
            ActiveButtons();
            _indexToAnimate = 0;
        }
    }
    
    private IEnumerator NextLevel() {
        DisableButtons();
        _isShortTime = false;
        
        var randomCell = Random.Range(0, cells.Length - 1);
        _randomPathList.Add(randomCell);
        
        var pathLength = _randomPathList.Count;
        _remainingTime = PlayerTime + pathLength * _timePerCell;

        if (pathLength % 5 == 0) {
            _timePerCell -= 0.2f / (1 + (int)Math.Log10(pathLength));
        }
        
        shortTimeAudioSource.Stop();
        timeText.color = Color.white;
        DisplayLevel(pathLength);
        
        _randomPathList.ForEach(item => {
            _pathQueue.Enqueue(item);
        });

        yield return new WaitForSeconds(1);
        AnimateCell();
    }

    private void GameOver() {
        _isGameOver = true;
        _isShortTime = false;
        DisableButtons();
        timeText.color = Color.white;
        DisplayTime(0);
        DisplayLevel(1);
        scoreText.text = "Your score: " + (_randomPathList.Count - 1);
        gamePanel.SetActive(false);
        gameOver.SetActive(true);
        _randomPathList = new List<int>();
        _pathQueue = new Queue<int>();
        _startTime = Time.time;
        _remainingTime = PlayerTime;
        _indexToAnimate = 0;
        _loseTimes += 1;
        if (_loseTimes % 3 == 0) {
            _loseTimes = 0;
            GetComponent<Interstitial>().ShowInterstitial();
        }
    }

    private void MainMenu() {
        gameOver.SetActive(false);
        gamePanel.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void PlayAgain() {
        _isGameOver = false;
        _isGameStarted = false;
        _startTime = Time.time;
        gameOver.SetActive(false);
        gamePanel.SetActive(true);
    }
    
    private void InitializeButtons() {
        for (var i = 0; i < cells.Length; i++) {
            var index = i;
            _actions[i] += () => OnClick(index);
            cells[i].GetComponentInChildren<Text>().text = index.ToString();
        }
    }

    private void DisableButtons() {
        for (var i = 0; i < cells.Length; i++) {
            cells[i].onClick.RemoveListener(_actions[i]);
        }
    }

    private void ActiveButtons() {
        for (var i = 0; i < cells.Length; i++) {
            cells[i].onClick.AddListener(_actions[i]);
        }
    }
    
    private void OnClick(int index) {
        if (_pathQueue.Count != 0 && index == _pathQueue.Peek()) {
            cells[index].GetComponent<AudioSource>().Play();
            _pathQueue.Dequeue();
            if (_pathQueue.Count == 0) {
                StartCoroutine(NextLevel());
            }
        }                
        else {
            GameOver();
        }
    }

    /*public IEnumerator WatchToContinue() {
        yield return new WaitForSeconds(5);
    }*/

    public void Awake() {
        _randomPathList = new List<int>();
        _pathQueue = new Queue<int>();
        _actions = new UnityAction[cells.Length];
        gameOver.SetActive(false);
    }

    public void Start() {
        _isGameOver = false;
        _startTime = Time.time;
        _remainingTime = PlayerTime;
        _indexToAnimate = 0;
        _isGameStarted = false;
        _timePerCell = 1.5f;
        _loseTimes = 0;
        InitializeButtons();
        playAgainBtn.onClick.AddListener(PlayAgain);
        mainMenuBtn.onClick.AddListener(MainMenu);
    }

    public void Update() {
        if (_isGameOver) {
            return;
        }
        
        if (!_isGameStarted && Time.time - _startTime > 3) {
            _isGameStarted = true;
            StartCoroutine(NextLevel());
        }
        
        if (!_isGameStarted) {
            return;
        }

        _remainingTime -= Time.deltaTime;
        if (_remainingTime < 1f) {
            GameOver();
        }
        else {
            if (_remainingTime < 6) {
                ShortTimeLeftEvent();    
            }
            
            DisplayTime(_remainingTime);
        }
    }
}
