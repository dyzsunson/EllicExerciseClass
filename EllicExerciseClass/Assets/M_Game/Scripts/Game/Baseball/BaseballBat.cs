using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour {
    float r_time = 0.0f;
    Vector3 m_bottomVelocity;
    Vector3 m_topVelocity;
    Vector3 m_bottomLastPosition;
    Vector3 m_topLastPosition;

    public Transform bottomTransform;
    public Transform topTransform;

    // Use this for initialization
    void Start() {
        m_bottomLastPosition = bottomTransform.position;
        m_topLastPosition = topTransform.position;

        m_topVelocity = m_bottomVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {
        if (r_time > 0.0f) {
            r_time -= Time.deltaTime;
            if (r_time <= 0.0f) {
                OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.RTouch);
            }
        }

        float k = 0.3f;

        m_bottomVelocity = m_bottomVelocity * k + (1.0f - k) * (this.bottomTransform.position - m_bottomLastPosition) / Time.deltaTime;
        m_topVelocity = m_topVelocity * k + (1.0f - k) * (this.topTransform.position - m_topLastPosition) / Time.deltaTime;

        m_bottomLastPosition = this.bottomTransform.position;
        m_topLastPosition = this.topTransform.position;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Baseball") {

            Vector3 ballPosition = collision.transform.position;

            float dis1 = Vector3.Distance(ballPosition, this.bottomTransform.position);
            float dis2 = Vector3.Distance(ballPosition, this.topTransform.position);
            float k = dis1 / (dis1 + dis2);

            Vector3 t_velocity = k * m_bottomVelocity + (1.0f - k) * m_topVelocity;

            collision.transform.GetComponent<Rigidbody>().velocity += 1.0f * t_velocity;

            this.GetComponent<AudioSource>().Play();
           
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
            r_time = 0.2f;
            
            if (collision.transform.GetComponent<Bullet>().isBlocked == false) {
                collision.transform.GetComponent<Bullet>().isBlocked = true;
                SceneController.Level_Current.GetComponent<ScoreCalculation>().BulletBlocked();
            }
        }
    }

    private void OnCollisionExit(Collision collision) {

    }
}
