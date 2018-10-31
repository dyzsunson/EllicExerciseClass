using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculation : MonoBehaviour {
    protected int m_bulletFired_num = 0;
    protected int m_bulletBlocked_num = 0;

    string m_levelName;

    public void BulletFired() {
        m_bulletFired_num++;
    }

    public void BulletBlocked() {
        m_bulletBlocked_num++;
    }

    // Use this for initialization
    void Start () {
        m_levelName = this.GetComponent<Level>().LevelName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Calculate() {

    }

    protected void Calculate(int _score) {
        if (_score < 0)
            _score = 0;

        string path_name = "Ellic's Exercise Class_Data/Record/";
        string file_name = m_levelName + "_" + SceneController.context.CurrentLevel.Difficulty + ".txt";
  
        if (!Directory.Exists(path_name)) {
            Directory.CreateDirectory(path_name);
        }

        string fileTxt = "";

        if (!File.Exists(path_name + file_name)) {
            File.Create(path_name + file_name).Dispose();
        }

        string[] lines = File.ReadAllLines(path_name + file_name);

        int[] scoreArray = new int[10];

        for (int i = 0; i < lines.Length; i++) {
            scoreArray[i] = int.Parse(lines[i]);
            
        }

        int rank = -1;
        for (int i = 0; i < 10; i++) {
            if (_score >= scoreArray[i]) {
                rank = i;
                break;
            }
        }

        if (rank != -1) {
            for (int i = 9; i > rank; i--) {
                scoreArray[i] = scoreArray[i - 1];
            }
            scoreArray[rank] = _score;
        }

        string rank1 = "", rank2 = "";
        for (int i = 0; i < 5; i++) {
            if (i == rank)
                rank1 += "<color=#00B242FF>";

            rank1 += (i + 1) + ". ";
            if (scoreArray[i] <= 0)
                rank1 += "N/A";
            else
                rank1 += scoreArray[i];

            if (i == rank)
                rank1 += "</color>";

            rank1 += "\r\n";

            fileTxt += scoreArray[i] + "\r\n";
        }

        for (int i = 5; i < 10; i++) {
            if (i == rank)
                rank1 += "<color=#00B242FF>";

            rank2 += (i + 1) + ". ";
            if (scoreArray[i] <= 0)
                rank2 += "N/A";
            else
                rank2 += scoreArray[i];

            if (i == rank)
                rank1 += "</color>";

            rank2 += "\r\n";

            fileTxt += scoreArray[i] + "\r\n";
        }

        File.WriteAllText(path_name + file_name, fileTxt);

        GameObject scoreVRObj = SceneController.context.ScoreVRObj;
        GameObject score2DObj = SceneController.context.Score2DObj;

        score2DObj.transform.Find("Score").GetComponent<Text>().text = scoreVRObj.transform.Find("Score").GetComponent<Text>().text = "" + _score;
        score2DObj.transform.Find("ScoreBG").GetComponent<Text>().text = scoreVRObj.transform.Find("ScoreBG").GetComponent<Text>().text = "" + _score;

        score2DObj.transform.Find("Score1").GetComponent<Text>().text = scoreVRObj.transform.Find("Score1").GetComponent<Text>().text = rank1;
        score2DObj.transform.Find("Score2").GetComponent<Text>().text = scoreVRObj.transform.Find("Score2").GetComponent<Text>().text = rank2;

        int starNum = Mathf.Min(5, _score / 20);
        for (int i = 0; i < starNum; i++) {
            score2DObj.transform.Find("Stars/" + (i + 1) + "/gold").gameObject.SetActive(true);
            scoreVRObj.transform.Find("Stars/" + (i + 1) + "/gold").gameObject.SetActive(true);
        }
    }
}
