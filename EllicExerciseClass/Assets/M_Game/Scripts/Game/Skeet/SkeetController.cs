using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeetController : Bullet {
    bool m_is_flying = true;

    public ScoreBoard BoardPrefab;

    protected override void Start() {
        m_lifeTime = 10.0f;
        m_bottom = -0.5f;
        // base.Start();
    }

    public void Fire() {
        base.Start();
    }


    private void OnCollisionEnter(Collision collision) {
        HitObjects(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        HitObjects(other.gameObject);
    }

    protected override void Update() {
        bool tag = is_inWater;
 
        base.Update();

        if (tag != is_inWater) {

            if (m_is_flying) {
                m_is_flying = false;

                GetPoint("-1");
                SceneController.Level_Current.GetComponent<SkeetScoreCal>().SkeetMiss();

            }
        }
    }

    void HitObjects(GameObject _obj) {
        if (_obj.tag == "Bullet") {
            if (m_is_flying) {

                GetPoint("+1");
                SceneController.Level_Current.GetComponent<SkeetScoreCal>().SkeetHit();
            }
            Destroy(this.gameObject);
        }
        else if (_obj.tag != "Target") {
            if (m_is_flying) {
                GetPoint("-1");
                SceneController.Level_Current.GetComponent<SkeetScoreCal>().SkeetMiss();
            }
        }
    }

    private void GetPoint(string _scoreTxt) {
        m_is_flying = false;

        ScoreBoard board = Instantiate(BoardPrefab);
        board.transform.Find("ScoreText").GetComponent<Text>().text = _scoreTxt;
        board.transform.position = this.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
        board.transform.forward = -(Camera.main.transform.position - this.transform.position).normalized;
    }
}
