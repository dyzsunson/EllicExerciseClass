using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : Game_Process_Object{
    public Game_Process_Object[] m_gameProcessObject_List;
    public GameObject HandObj;
    private GameObject m_tutorialObj;

    public Sprite LevelIcon;
    public string LevelName;
    public string LevelDescription;

    public float EndingBufferTime = 1.0f;

    public enum DifficultyLevel {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2
    }

    private DifficultyLevel m_difficultyLevel = DifficultyLevel.EASY;
    public DifficultyLevel Difficulty {
        get {
            return m_difficultyLevel;
        }
        set {
            m_difficultyLevel = value;
        }
    }

    private void Awake() {
        m_tutorialObj = this.transform.Find("Tutorial").gameObject;

        Game_Process_Object[] t_list = this.transform.GetComponentsInChildren<Game_Process_Object>();
        m_gameProcessObject_List = new Game_Process_Object[t_list.Length - 1];
        Array.Copy(t_list, 1, m_gameProcessObject_List, 0, t_list.Length - 1);
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#region Game_Process_Interface
    public override void GameReady() {
        base.GameReady();

        // HandObj.SetActive(true);

        if (m_tutorialObj != null) {
            m_tutorialObj.SetActive(true);

            m_tutorialObj.GetComponent<FadeInOut>().FadeIn(1.0f);
        }

        Invoke("HideTutorial", SceneController.context.ReadyWaitTime - 5.0f);

        foreach (Game_Process_Object obj in m_gameProcessObject_List) {
            if (obj.gameObject.activeInHierarchy == true)
                obj.GameReady();
        }
    }

    public override void GameStart() {
        base.GameStart();


        if (m_tutorialObj != null)
            m_tutorialObj.SetActive(false);

        foreach (Game_Process_Object obj in m_gameProcessObject_List) {
            if (obj.gameObject.activeInHierarchy == true)
                obj.GameStart();
        }
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();

        foreach (Game_Process_Object obj in m_gameProcessObject_List) {
            if (obj.gameObject.activeInHierarchy == true)
                obj.GameEndBuffer();
        }
    }

    public override void GameEnd() {
        base.GameEnd();

        // HandObj.SetActive(false);

        foreach (Game_Process_Object obj in m_gameProcessObject_List) {
            if (obj.gameObject.activeInHierarchy == true)
                obj.GameEnd();
        }
    }
#endregion

    void ShowTutorial() {
        
    }

    void HideTutorial() {
        m_tutorialObj.GetComponent<FadeInOut>().FadeOut(2.0f);
        Transform obj_3d = m_tutorialObj.transform.Find("3D_OBJ");
        if (obj_3d != null)
            obj_3d.gameObject.SetActive(false);
    }
}
