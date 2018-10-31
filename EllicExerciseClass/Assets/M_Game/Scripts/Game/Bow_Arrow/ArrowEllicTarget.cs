using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowEllicTarget : EllicTarget {

    public ScoreBoard BoardPrefab;
    private bool m_isWalking = false;
    private float m_speed = 2.0f;
    private float m_reverse_time = 15.0f;

    protected override void Hit(Collider _other) {
        base.Hit(_other);

        if (m_life > 0 && m_isWalking == true)
            p_animator.Play("Base Layer.WalkBeat");

        SceneController.Level_Current.GetComponent<Arrow_Score_Cal>().HitEllic();

        ScoreBoard board = Instantiate(BoardPrefab);
        board.transform.Find("ScoreText").GetComponent<Text>().text = "-10";
        board.transform.position = this.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
        board.transform.forward = -(Camera.main.transform.position - this.transform.position).normalized;
    }

    public void Walk() {
        if (m_isWalking == false && m_life > 0) {
            m_isWalking = true;
            this.transform.forward = new Vector3(-1.0f, 0.0f, 0.0f);
            p_animator.Play("Base Layer.Walk");

            Invoke("Reverse", m_reverse_time);
        }
    }

    private void Update() {
        if (p_animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Walk")) {
            this.transform.Translate(m_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f) );
        }
    }

    private void Reverse() {
        if (m_life > 0) {
            this.transform.forward = -this.transform.forward;
            Invoke("Reverse", m_reverse_time);
        }
    }
}
