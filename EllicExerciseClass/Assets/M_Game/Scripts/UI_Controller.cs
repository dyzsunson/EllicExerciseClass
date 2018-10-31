using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : Game_Process_Object {
    #region public value from outside
    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public GameObject StartMenuVR;
    public GameObject GameMenuVR;
    public GameObject EndMenuVR;

    public GameObject TutorialMenu;

    public GameObject Firework;
    #endregion

    #region private value


    Text LevelText;
    Image LevelIcon;

    Text LevelText_VR;
    Text LevelDescription_Text_VR;
    Image LevelIcon_VR;
    #endregion

    void Awake() {
        LevelText = StartMenu.transform.Find("Level/LevelText").GetComponent<Text>();
        LevelIcon = StartMenu.transform.Find("Level/Icon").GetComponent<Image>();

        LevelText_VR = StartMenuVR.transform.Find("Level/LevelText").GetComponent<Text>();
        LevelIcon_VR = StartMenuVR.transform.Find("Level/Icon").GetComponent<Image>();
        LevelDescription_Text_VR = StartMenuVR.transform.Find("Level/Level_Description").GetComponent<Text>();
    }

    // Use this for initialization
    void Start() {
        StartMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        StartMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        EndMenu.SetActive(false);
        EndMenuVR.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public override void GameReady() {
        base.GameReady();

        GameMenu.SetActive(true);
        GameMenu.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenuVR.SetActive(true);
        GameMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        StartMenu.SetActive(false);
        StartMenuVR.SetActive(false);
    }

    public override void GameStart() {
        base.GameStart();
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);
    }

    public override void GameEnd() {
        base.GameEnd();

        EndMenu.SetActive(true);
        EndMenuVR.SetActive(true);

        Firework.SetActive(true);

        EndMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        EndMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public void ShowTutorial() {
        TutorialMenu.SetActive(true);
        TutorialMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public void HideTutorial() {
        TutorialMenu.SetActive(false);
    }

    public void LevelChange(Level _previous, Level _level) {
        LevelText.text = LevelText_VR.text = _level.LevelName;
        LevelDescription_Text_VR.text = _level.LevelDescription;

        LevelIcon.sprite = LevelIcon_VR.sprite = _level.LevelIcon;

        /*
        if (_previous != null)
            _previous.TutorialCanvas.SetActive(false);

        _level.TutorialCanvas.SetActive(true);
        */
    }
}
