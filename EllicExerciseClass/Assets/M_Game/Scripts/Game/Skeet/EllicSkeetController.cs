using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicSkeetController : EllicBallController {
    public GameObject pinkEllic;
    bool m_isPinkHere = false;

    public override void GameReady() {
        base.GameReady();
        m_max_rotateSpeed *= 2.0f;
        m_rotate_a *= 2.0f;
        m_max_degree = 30.0f;

        if (SceneController.context.CurrentLevel.Difficulty == Level.DifficultyLevel.HARD && pinkEllic != null) {
            m_isPinkHere = true;
            pinkEllic.SetActive(true);
            foreach (Game_Process_Object _obj in pinkEllic.GetComponentsInChildren<Game_Process_Object>())
                _obj.GameReady();
        }
    }

    public override void Fire() {
        this.transform.Find("Ellic").GetComponent<Animator>().Play("Base Layer.VolleyHit");
    }

    public override void GameStart() {
        base.GameStart();

        if (m_isPinkHere) {
            foreach (Game_Process_Object _obj in pinkEllic.GetComponentsInChildren<Game_Process_Object>())
                _obj.GameStart();
        }
    }

    public override void GameEnd() {
        base.GameEnd();

        if (m_isPinkHere) {
            foreach (Game_Process_Object _obj in pinkEllic.GetComponentsInChildren<Game_Process_Object>())
                _obj.GameEnd();
        }
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();

        if (m_isPinkHere) {
            foreach (Game_Process_Object _obj in pinkEllic.GetComponentsInChildren<Game_Process_Object>())
                _obj.GameEndBuffer();
        }
    }

}
