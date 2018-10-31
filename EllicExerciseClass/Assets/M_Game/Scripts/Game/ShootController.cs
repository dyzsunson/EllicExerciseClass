using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController :Game_Process_Object {
    // y rotate
    protected float m_rotateSpeed = 0.0f;
    protected float m_max_rotateSpeed = 50.0f;
    protected float m_max_degree = 8.0f;
    protected float m_rotate_a = 250.0f;
    protected float m_pre_degree = 0.0f;

    // fire obj
    public GameObject BulletPrefab;
    public Transform GunBodyTransform;
    public Transform FireTransform;
    public Transform StartTransform;

    // power variable
    protected float m_power = 0.0f;
    protected float m_max_power = 1.25f;
    protected float m_min_power = 0.5f;
    protected float m_scaler = 1200.0f;

    protected bool m_allow_power_reloading = true;

    // reload
    protected float m_max_reloadTime = 0.4f;
    protected float m_reloadTime = 0.0f;

    protected bool is_reloading = false;

    public BallAI AI;

    // Use this for initialization
    protected virtual void Start () {
        m_power = m_min_power;
        m_pre_degree = GunBodyTransform.rotation.eulerAngles.x;
        if (m_pre_degree > 180.0f)
            m_pre_degree -= 360.0f;

    }

    // Update is called once per frame
    protected virtual void Update () {
        if (m_state == GameState.RUNNING) {
            if (!is_reloading && AI.IsPowerButtonHold) {
                if (!m_allow_power_reloading && is_reloading)
                    ;
                else if (m_power < m_max_power)
                    m_power += Time.deltaTime;
            }

            if (AI.IsPowerButtonUp) {
                if (!is_reloading) {
                    Fire();
                    StartReload();
                }
                else {
                    ResetFire();

                }
            }

            if (!AI.IsPowerButtonHold && AI.IsUpHold) {
                if (m_rotateSpeed > -m_max_rotateSpeed) {
                    m_rotateSpeed -= m_rotate_a * Time.deltaTime;
                }
            }
            else if (!AI.IsPowerButtonHold && AI.IsDownHold) {
                if (m_rotateSpeed < m_max_rotateSpeed) {
                    m_rotateSpeed += m_rotate_a * Time.deltaTime;
                }
            }
            else {
                m_rotateSpeed = 0.0f;
            }

            float rotationX = GunBodyTransform.rotation.eulerAngles.x;

            if (rotationX > 180.0f)
                rotationX -= 360.0f;

            if (rotationX < (m_pre_degree + m_max_degree) && m_rotateSpeed > 0.0f)
                GunBodyTransform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), m_rotateSpeed * Time.deltaTime);
            else if (rotationX > (m_pre_degree - m_max_degree) && m_rotateSpeed < 0.0f)
                GunBodyTransform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), m_rotateSpeed * Time.deltaTime);

            if (is_reloading) {
                if (m_reloadTime > m_max_reloadTime) {
                    ReloadEnd();
                }
                else {
                    Reloading();
                }
                // this.transform.Translate()
            }
        }
    }

    protected virtual void ResetFire() {
        m_power = m_min_power;
    }

    protected void StartReload() {
        ResetFire();
        is_reloading = true;
        m_reloadTime = 0.0f;
    }

    protected virtual void ReloadEnd() {
        is_reloading = false;
    }

    protected virtual void Reloading() {
        m_reloadTime += Time.deltaTime;
    }

    protected virtual void FireOneBullet(GameObject _obj, float _power) {
        FireOneBullet(_obj, _power, Vector3.zero);
    }

    protected virtual void FireOneBullet(GameObject _obj, float _power, Vector3 _offset) {
        _obj.transform.position = FireTransform.position;
        _obj.GetComponent<Rigidbody>().AddForceAtPosition(m_scaler * _power * (FireTransform.position - StartTransform.position).normalized, _obj.transform.position + _offset);

        this.GetComponent<AudioSource>().Play();
        SceneController.Level_Current.GetComponent<ScoreCalculation>().BulletFired();
    }

    protected virtual GameObject Fire() {
        return Fire(BulletPrefab);
    }

    protected virtual GameObject Fire(GameObject _BulletPrefab) {
        GameObject obj = Instantiate(_BulletPrefab);
        FireOneBullet(obj, m_power, Vector3.zero);

        this.transform.parent.GetComponent<EllicBallController>().Fire();

        return obj;
    }

#region game process interface
    public override void GameReady() {
        base.GameReady();
    }

    public override void GameStart() {
        base.GameStart();
    }

    public override void GameEnd() {
        base.GameEnd();
    }

    public override void GameEndBuffer() {
        base.GameEndBuffer();
    }
}
#endregion