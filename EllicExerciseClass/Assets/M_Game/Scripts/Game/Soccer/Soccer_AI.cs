using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soccer_AI : BallAI {
    public bool Is_power_left_hold = false;
    public bool Is_power_right_hold = false;

    protected override void Start() {
        base.Start();

        m_fire_wait_minTime = new float[] { 2.0f, 1.0f, 0.6f};
        m_fire_wait_maxTime = new float[] { 3.0f, 1.5f, 0.7f};

        m_fire_min = 0.4f;
        m_fire_max = 0.7f;

        m_y_rotateRange = new float[] { 8.0f, 12.0f, 15.0f };
        m_x_rotateRange = new float[] { 6.0f, 8.0f, 10.0f };

        m_rotate_wait_minTime = new float[] { 0.5f, 0.3f, 0.05f };
        m_rotate_wait_maxTime = new float[] { 0.8f, 0.4f, 0.1f };
}

    protected override void Fire() {
        // base.Fire();
        IsPowerButtonHold = true;

        float t;

        t = 1.2f * GetFireTime();


        float angle = ellicBallController.transform.rotation.eulerAngles.y;
        

        if (angle > 180.0f)
            angle -= 360.0f;

        float t_min = 0.0f, t_max = 0.5f;
        
        if (angle < -5.0f)
            Is_power_right_hold = true;
        else if (angle > 5.0f)
            Is_power_left_hold = true;
        else if (Random.Range(0.0f, 1.0f) < 0.5f)
            Is_power_left_hold = true;
        else
            Is_power_right_hold = true;

        t_min = Mathf.Abs(angle) * 0.05f;
        t_max = Mathf.Abs(angle) * 0.1f;

        if ((angle < 0.0f && IsLeftHold == true) || (angle > 0.0f && IsRightHold == true)) {
            t_min = 0.0f;
            t_max = Mathf.Min(0.5f / Mathf.Abs(angle), 0.1f);
        }

        if (m_difficultyLevel == Level.DifficultyLevel.EASY) {
            t_min *= 0.2f;
            t_max *= 0.2f;
        }
        else if (m_difficultyLevel == Level.DifficultyLevel.MEDIUM) {
            t_min *= 0.6f;
            t_max *= 0.6f;
        }

        float curveTime = 0.85f * Random.Range(t_min, t_max); // Mathf.Abs(angle) / m_y_rotateRange * 2.0f

        Invoke("CurveEnd", Mathf.Min(curveTime, t));
        // Invoke("PowerEnd", t);

        Invoke("WaitAfterFire", t);
    }

    void PowerEnd() {
        IsPowerButtonHold = false;
    }

    void CurveEnd() {
        Is_power_left_hold = Is_power_right_hold = false;
    }
}
