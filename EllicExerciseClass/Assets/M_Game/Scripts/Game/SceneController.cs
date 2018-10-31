using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Game_Process_Object{
    // Timer
    public float gameTime;
    float m_time;

    public float TimeLeft {
        get {
            return m_time;
        }
    }

    public Text TimeTextVR;
    public Text WaitTimeTextVR;

    private float m_waitTime = 10.0f;
    public float ReadyWaitTime {
        get {
            return this.m_waitTime;
        }
    }

    private bool is_vr_ready = false;
    private bool is_pc_ready = false;

    // Scene Object
    public Level[] levelList;
    static int s_currentLevel = 0;

    public Level CurrentLevel {
        get { return levelList[s_currentLevel]; }
    }

    public GameObject TouchObj;
    public GameObject LeftTouchObj;
    public GameObject RightTouchObj;
    public GameObject RoamingCameraObj;

    public GameObject ScoreVRObj;
    public GameObject Score2DObj;

    public GameObject VRDifficultyObj;
    static int s_currentDifficulty = 0;

    // UI Menu
    public UI_Controller ui_controller;

    public static SceneController context;

    public static Level Level_Current {
        get {
            return context.levelList[s_currentLevel];
        }
    }

    private void Awake() {
        context = this;
    }



    // Use this for initialization
    void Start () {
        s_currentLevel = NotChanged.context.Level_Current;
        s_currentDifficulty = NotChanged.context.difficulty_level;
        this.LevelChange(0);
        this.DifficultyChange(0);

        if (NotChanged.context.is_auto_start == true) {
            Start_SinglePlayer();
        }
        NotChanged.context.is_auto_start = false;
     }
	
	// Update is called once per frame
	void Update () {
        EventSystem.current.SetSelectedGameObject(null);

        if (this.Current_State == GameState.RUNNING) {
            m_time -= Time.deltaTime;
            if (m_time < 0.0f)
                this.GameEndBuffer();
            else {
                TimeTextVR.text = ((int)m_time).ToString();
            }
        }

        if (this.Current_State == GameState.READY) {
            m_waitTime -= Time.deltaTime;
            if (m_waitTime < 0.0f) {
                this.GameStart();
            }
            else if(m_waitTime < 1.0f) {
                if (WaitTimeTextVR.text != "GO !") {
                    WaitTimeTextVR.transform.Find("Sound2").GetComponent<AudioSource>().Play();
                }
                WaitTimeTextVR.text = "GO !";
            }
            else if (m_waitTime < 4.0f) {
                if (WaitTimeTextVR.text != ((int)m_waitTime).ToString()) {
                    WaitTimeTextVR.transform.Find("Sound1").GetComponent<AudioSource>().Play();
                }
                WaitTimeTextVR.text = ((int)m_waitTime).ToString();
            }
        }
	}

    #region public scene controll function
    public void ResetCamera() {
        UnityEngine.XR.InputTracking.Recenter();
    }

    public void StartLevel() {
        this.levelList[s_currentLevel].Difficulty = (Level.DifficultyLevel)s_currentDifficulty;
        this.Start_SinglePlayer();
    }

    public void Start_Level_Easy() {
        this.levelList[s_currentLevel].Difficulty = Level.DifficultyLevel.EASY;
        this.Start_SinglePlayer();
    }

    public void Start_Level_Medium() {
        this.levelList[s_currentLevel].Difficulty = Level.DifficultyLevel.MEDIUM;
        this.Start_SinglePlayer();
    }

    public void Start_Level_Hard() {
        this.levelList[s_currentLevel].Difficulty = Level.DifficultyLevel.HARD;
        this.Start_SinglePlayer();
    }

    private void Start_SinglePlayer() {
        if (this.Current_State == GameState.NOT_STARTED) {
            s_currentDifficulty = (int)this.levelList[s_currentLevel].Difficulty;
            GameReady();
        }
    }

    public void GameQuit() {
        Application.Quit();
    }

    public void ReStartScene() {
        SceneManager.LoadScene(0);
    }

    public void RestartGame() {
        NotChanged.context.is_auto_start = true;
        ReStartScene();
    }

    public void ShowTutorial() {
        if (m_state == GameState.NOT_STARTED) {
            this.ui_controller.ShowTutorial();
        }
    }

    public void HideTutorial() {
        if (m_state == GameState.NOT_STARTED) {
            this.ui_controller.HideTutorial();
        }
    }

    public void NextLevel() {
        if (m_state == GameState.NOT_STARTED) {
            this.LevelChange(1);
        }
    }

    public void LastLevel() {
        if (m_state == GameState.NOT_STARTED) {
            this.LevelChange(-1);
        }
    }

    public void NextDifficulty() {
        if (m_state == GameState.NOT_STARTED) {
            this.DifficultyChange(1);
        }
    }

    public void LastDifficulty() {
        if (m_state == GameState.NOT_STARTED) {
            this.DifficultyChange(-1);
        }
    }

    public void ShowTouchObj() {
        TouchObj.SetActive(true);
        LeftTouchObj.SetActive(true);
        RightTouchObj.SetActive(true);

        this.levelList[s_currentLevel].HandObj.SetActive(false);
    }

    public void ShowGameHandObj() {
        TouchObj.SetActive(false);
        LeftTouchObj.SetActive(false);
        RightTouchObj.SetActive(false);

        this.levelList[s_currentLevel].HandObj.SetActive(true);
    }

    public void ShowTouchObj(float _time) {
        Invoke("ShowTouchObj", _time);
    }

    #endregion


    #region Game_Process_Interface
    public override void GameReady() {
        base.GameReady();

        m_time = gameTime;
        
        this.ShowGameHandObj();

        this.ui_controller.GameReady();
        RoamingCameraObj.SetActive(false);

        this.levelList[s_currentLevel].GameReady();
    }

    public override void GameStart() {
        base.GameStart();

        TimeTextVR.transform.parent.gameObject.SetActive(true);
        TimeTextVR.text = ((int)m_time).ToString();

        WaitTimeTextVR.transform.parent.gameObject.SetActive(false);

        this.levelList[s_currentLevel].GameStart();
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();

        this.levelList[s_currentLevel].GameEndBuffer();

        this.ui_controller.GameEndBuffer();

        Invoke("GameEnd", this.levelList[s_currentLevel].EndingBufferTime);
    }

    public override void GameEnd() {
        base.GameEnd();

        this.levelList[s_currentLevel].GameEnd();

        this.ShowTouchObj(2.0f);

        this.ui_controller.GameEnd();

        // score
        ScoreCalculate();
        this.GetComponent<AudioSource>().Play();
    }
    #endregion 

    #region private support functions
    void ScoreCalculate() {
        SceneController.Level_Current.GetComponent<ScoreCalculation>().Calculate();
    }

    private void LevelChange(int _offset) {
        int previous_level = s_currentLevel;

        levelList[s_currentLevel].gameObject.SetActive(false);
        s_currentLevel = (s_currentLevel + _offset + levelList.Length) % levelList.Length;
        levelList[s_currentLevel].gameObject.SetActive(true);

        NotChanged.context.Level_Current = s_currentLevel;

        this.ui_controller.LevelChange(levelList[previous_level], levelList[s_currentLevel]);
    }

    private void DifficultyChange(int _offset) {
        VRDifficultyObj.transform.Find(s_currentDifficulty.ToString()).GetComponent<VR_Difficulty_Item>().DisHighlight();

        s_currentDifficulty = (s_currentDifficulty + _offset + 3) % 3;
        NotChanged.context.difficulty_level = s_currentDifficulty;

        VRDifficultyObj.transform.Find(s_currentDifficulty.ToString()).GetComponent<VR_Difficulty_Item>().HighLight();
    }
    #endregion 
}

