using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAI : EllicAI {
    // button
    public bool IsLeftHold;
    public bool IsRightHold;
    public bool IsUpHold;
    public bool IsDownHold;
    public bool IsPowerButtonHold;
    public bool IsPowerButtonUp;

    // rotate variable
    protected float m_rotate_a = 250.0f;
    protected float m_max_speed = 50.0f;

    protected float[] m_y_rotateRange = { 5.0f, 5.5f, 6.0f };
    protected float[] m_x_rotateRange = { 2.0f, 2.0f, 2.0f };

    // fire variable
    protected float m_fire_min = 0.4f;
    protected float m_fire_max = 0.60f;

    // private float m_y_rotate_a = 250.0f;
    // private float m_y_max_speed = 50.0f;

    // reloadTime
    protected float[] m_fire_wait_minTime = { 0.5f, 0.3f, 0.1f };
    protected float[] m_fire_wait_maxTime = { 0.8f, 0.5f, 0.2f };
    protected float[] m_rotate_wait_minTime = { 0.5f, 0.3f, 0.1f };
    protected float[] m_rotate_wait_maxTime = { 0.8f, 0.4f, 0.2f };

    protected bool is_last_powerButtonHold = false;

    protected int m_current_skill = -1;
    protected float m_min_skill_time = 10.0f;
    protected float m_max_skill_time = 12.0f;
    
    float m_skill_reloading_time;
    float m_skill_last_time;
    bool is_skill_on = false;

    protected Level.DifficultyLevel m_difficultyLevel = 0;

    public int BulletType {
        get {
            if (is_skill_on)
                return m_current_skill;
            else
                return 3;
        }
    }

    public EllicBallController ellicBallController;


    #region game process interface
    public override void GameReady() {
        base.GameReady();

        m_difficultyLevel = SceneController.context.CurrentLevel.Difficulty;

        m_skill_reloading_time = Random.Range(m_min_skill_time, m_max_skill_time);
    }

    public override void GameStart() {
        base.GameStart();

        RotateNext();
    }

    public override void GameEnd() {
        base.GameEnd();
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();
    }
    #endregion

    // Use this for initialization
    protected virtual void Start() {
        
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (!is_last_powerButtonHold && IsPowerButtonHold) {
            is_last_powerButtonHold = true;
        }

        if (is_last_powerButtonHold && !IsPowerButtonHold) {
            is_last_powerButtonHold = false;
            IsPowerButtonUp = true;
        }
        else {
            IsPowerButtonUp = false;
        }

        if (this.m_state == GameState.RUNNING) {
            m_skill_reloading_time -= Time.deltaTime;
            

            if (m_skill_last_time > 0.0f) {
                m_skill_last_time -= Time.deltaTime;
                if (m_skill_last_time <= 0.0f) {
                    m_skill_last_time = 0.0f;
                    is_skill_on = false;
                }
            }

            if (m_skill_reloading_time < 0.0f) {
                UseSkill();
            }
        }
    }

    void UseSkill() {
        is_skill_on = true;
        m_current_skill++;
        m_current_skill %= 3;

        m_skill_last_time = 5.0f;
        m_skill_reloading_time = Random.Range(m_min_skill_time, m_max_skill_time);
    }

    #region rotation
    void RotateNext() {
        if (m_state != GameState.RUNNING)
            return;

        float ty = RotateAroundY();
        float tx = RotateAroundX();

        Invoke("WaitAfterRotate", Mathf.Max(tx, ty));
    }

    float RotateAroundY() {
        float startDegree = ellicBallController.transform.rotation.eulerAngles.y;
        if (startDegree > 180.0f)
            startDegree -= 360.0f;

        float endDegree = 0.0f;

        endDegree = Random.Range(-m_y_rotateRange[(int)m_difficultyLevel], m_y_rotateRange[(int)m_difficultyLevel]);

        float t = TimeRotateFromTo(startDegree, endDegree, m_max_speed, m_rotate_a);
        if (endDegree > startDegree)
            IsRightHold = true;
        else
            IsLeftHold = true;

        Invoke("RotateYEnd", t);

        return t;
    }

    float RotateAroundX() {
        float startDegree = ellicBallController.shooter.GunBodyTransform.rotation.eulerAngles.x;
        if (startDegree > 180.0f)
            startDegree -= 360.0f;

        float endDegree = 0.0f;

        endDegree = Random.Range(-m_x_rotateRange[(int)m_difficultyLevel], m_x_rotateRange[(int)m_difficultyLevel]);

        float t = TimeRotateFromTo(startDegree, endDegree, m_max_speed, m_rotate_a);

        if (endDegree < startDegree)
            IsUpHold = true;
        else
            IsDownHold = true;

        Invoke("RotateXEnd", t);

        return t;
    }

    float TimeRotateFromTo(float _start, float _end, float _speed, float _a) {
        float t = 0.0f;

        float a_length = 0.5f * m_max_speed * m_max_speed / m_rotate_a;

        float dis = Mathf.Abs(_end - _start);
        if (dis < a_length) {
            t = Mathf.Sqrt(dis * 2.0f / m_rotate_a);
        }
        else {
            t = (dis - a_length) / m_max_speed + m_max_speed / m_rotate_a;
        }

        return t;
    }

    void RotateXEnd() {
        IsUpHold = IsDownHold = false;
    }

    void RotateYEnd() {
        IsLeftHold = IsRightHold = false;
    }

    void WaitAfterRotate() {
        IsUpHold = IsDownHold = IsLeftHold = IsRightHold = false;

        float t = Random.Range(m_rotate_wait_minTime[(int)m_difficultyLevel], m_rotate_wait_maxTime[(int)m_difficultyLevel]);
        Invoke("Fire", t);
    }
    #endregion

    #region FIRE
    protected virtual void Fire() {
        IsPowerButtonHold = true;

        float t;

        t = GetFireTime();

        Invoke("WaitAfterFire", t);
    }

    protected virtual float GetFireTime() {
        float degree = ellicBallController.shooter.GunBodyTransform.rotation.eulerAngles.x;
        if (degree > 180.0f)
            degree -= 360.0f;

        if (degree > 5.0f)
            degree = 5.0f;
        if (degree < -5.0f)
            degree = -5.0f;

        float low = m_fire_min;
        float high = m_fire_max;

        float diff = m_fire_max - m_fire_min;
        if (degree < 0.0f)
            high -= diff * (-degree) / 5.0f;
        else
            low += 0.5f * diff * (degree) / 5.0f;


        return Random.Range(low, high);
    }

    void WaitAfterFire() {
        IsPowerButtonHold = false;

        float t = Random.Range(m_fire_wait_minTime[(int)m_difficultyLevel], m_fire_wait_maxTime[(int)m_difficultyLevel]);
        Invoke("RotateNext", t);
    }
    #endregion

   
}
