using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballShootController : ShootController {
    float m_reload_gun_speed = 0.2f;

    public Transform p_build_Skeet_Position;
    private GameObject m_current_skeet;

    private float m_power_last;
    private GameObject m_last_skeet;

    private float[] m_baseballSize = { 1.0f, 0.75f, 0.5f };
    private int m_difficulty;

    public override void GameReady() {
        base.GameReady();
        m_difficulty = (int)SceneController.context.CurrentLevel.Difficulty;
    }

    public override void GameStart() {
        base.GameStart();
    }

    protected override void Start() {
        base.Start();
        m_max_degree = 0.2f;
        m_max_reloadTime = 0.5f;

        m_scaler = 1500.0f;

        this.BuildSkeet();
    }

    protected override GameObject Fire() {
        m_power = (m_power + 6.2f) / 7.5f;

        if (m_current_skeet == null)
            return null;

        m_power_last = m_power;
        m_last_skeet = m_current_skeet;
        Invoke("LateFire", 0.35f);

        this.transform.parent.GetComponent<EllicBallController>().Fire();
        GameObject ball = m_current_skeet;
        m_current_skeet = null;

        m_last_skeet.GetComponent<Rigidbody>().useGravity = true;
        m_last_skeet.GetComponent<Rigidbody>().AddForce(150.0f * new Vector3(0.0f, 1.0f, 0.0f));

        return ball;
    }

    protected override void ReloadEnd() {
        base.ReloadEnd();
        this.BuildSkeet();
    }

    void LateFire() {
        m_last_skeet.GetComponent<Rigidbody>().useGravity = true;
        m_last_skeet.GetComponent<Collider>().enabled = true;

        FireOneBullet(m_last_skeet, m_power_last, new Vector3(0.0f, 0.0f, 0.0f));
        m_last_skeet.GetComponent<BaseballController>().Fire();

    }


    protected override void Reloading() {
        base.Reloading();
        if (m_reloadTime < m_max_reloadTime / 2.0f) {
            GunBodyTransform.Translate(-m_reload_gun_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
        }
        else {
            GunBodyTransform.Translate(m_reload_gun_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
        }
    }



    void BuildSkeet() {
        m_current_skeet = Instantiate(BulletPrefab);

        m_current_skeet.transform.position = p_build_Skeet_Position.position;
        m_current_skeet.transform.SetParent(this.transform.parent.parent);

        m_current_skeet.transform.localScale = m_baseballSize[m_difficulty] * Vector3.one;

        m_current_skeet.GetComponent<Rigidbody>().useGravity = false;
        m_current_skeet.GetComponent<Collider>().enabled = false;
    }

}
