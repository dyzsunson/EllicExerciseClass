using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicBallController : Game_Process_Object {
    public ShootController shooter;
    public BallAI ballAI;

    // float m_speed = 0.1f;
    protected float m_rotateSpeed = 0.0f;
    protected float m_max_rotateSpeed = 50.0f;
    protected float m_max_degree = 18.0f;
    protected float m_rotate_a = 250.0f;
    protected float m_degree = 0.0f;

    public float Max_RotateSpeed {
        get {
            return this.m_max_rotateSpeed;
        }
    }

    public float Rotate_A {
        get {
            return this.m_rotate_a;
        }
    }

    // Use this for initialization

    // Update is called once per frame
    protected void Update() {
        if (!ballAI.IsPowerButtonHold && ballAI.IsLeftHold) {
            if (m_rotateSpeed > -m_max_rotateSpeed) {
                m_rotateSpeed -= m_rotate_a * Time.deltaTime;
            }
        }
        else if (!ballAI.IsPowerButtonHold && ballAI.IsRightHold) {
            if (m_rotateSpeed < m_max_rotateSpeed) {
                m_rotateSpeed += m_rotate_a * Time.deltaTime;
            }
        }
        else {
            m_rotateSpeed = 0.0f;
        }
        

        float angle = this.transform.rotation.eulerAngles.y;
        if (angle > 180.0f)
            angle -= 360.0f;

        if (angle < m_max_degree && m_rotateSpeed > 0.0f)
            this.transform.Rotate(this.transform.up, m_rotateSpeed * Time.deltaTime);
        else if (angle > -m_max_degree && m_rotateSpeed < 0.0f)
            this.transform.Rotate(this.transform.up, m_rotateSpeed * Time.deltaTime);
    }

    #region Game_Process_Interface
    public override void GameReady() {
        base.GameReady();
    }

    public override void GameStart() {
        base.GameStart();
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();
    }

    public override void GameEnd() {
        base.GameEnd();
    }
    #endregion

    public virtual void Fire() {
        this.transform.Find("Ellic").GetComponent<Animator>().Play("Base Layer.CannonFire");
    }
}
