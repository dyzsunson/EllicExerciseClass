using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeet_AI : BallAI {

    protected override void Start() {
        base.Start();
        m_fire_min = 0.5f;
        m_fire_max = 1.0f;
        m_y_rotateRange = new float[] { 10.0f, 15.0f, 20.0f };
        m_x_rotateRange = new float[] { 10.0f, 15.0f, 30.0f };
        // m_fire_wait_minTime = new float[] { 2.0f, 2.0f, 0.0f };
        // m_fire_wait_maxTime = new float[] { 3.5f, 2.5f, 0.3f };

        m_fire_wait_minTime = new float[] { 2.0f, 2.0f, 2.0f };
        m_fire_wait_maxTime = new float[] { 3.5f, 2.5f, 4.0f };
    }

    protected override void Update() {
        base.Update();
        if (this.m_state == GameState.RUNNING) {
            if (SceneController.context.TimeLeft < 3.0f) {
                this.GameEndBuffer();
            }
        }
    }
}
