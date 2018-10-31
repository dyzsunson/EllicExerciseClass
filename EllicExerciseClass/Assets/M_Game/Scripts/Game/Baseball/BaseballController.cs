using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseballController : Bullet {
    bool m_is_flying = true;

    public ScoreBoard BoardPrefab;

    public PhysicMaterial GrassMaterial;

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
                SceneController.Level_Current.GetComponent<SkeetScoreCal>().SkeetMiss();
            }
        }
    }

    void HitObjects(GameObject _obj) {
        if (_obj.tag == "Bat") {
            if (m_is_flying) {

                
            }
            // Destroy(this.gameObject);
        }
        else {
            if (m_is_flying && _obj.tag != "net") {
                m_is_flying = false;
                if (_obj.tag == "Target") {
                    int score = _obj.GetComponent<BaseballGoal>().Score;
                    GetPoint(score);
                }
            }
            
            // this.GetComponent<Rigidbody>().velocity *= 0.7f;
            this.GetComponent<SphereCollider>().material = GrassMaterial;
        }
    }

    private void GetPoint(int _score) {
        string scoreTxt = "";
        if (_score == 3)
            scoreTxt = "Home Run!";
        else
            scoreTxt = "+" + _score;

        ScoreBoard board = Instantiate(BoardPrefab);
        board.transform.Find("ScoreText").GetComponent<Text>().text = scoreTxt;
        board.transform.position = this.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
        board.transform.forward = -(Camera.main.transform.position - this.transform.position).normalized;
    }
}
