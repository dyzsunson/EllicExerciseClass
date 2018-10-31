using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicTarget : MonoBehaviour {
    protected float m_life = 0;
    public float m_life_max = 3;
    public float m_escape_time = 5.0f;
    public bool Is_Rotate_Escape;

    Transform life_sprite;

    float m_life_sprite_scaleX_ori;
    float m_life_sprite_positionX_ori;

    public Animator p_animator;

    // Use this for initialization
    void Start () {
        life_sprite = this.transform.Find("Life");
        m_life = m_life_max;

        m_life_sprite_scaleX_ori = life_sprite.localScale.x;
        m_life_sprite_positionX_ori = life_sprite.localPosition.x;

        if (m_escape_time > 0)
            Invoke("Start_Escape", m_escape_time);
	}


    private void OnTriggerEnter(Collider other) {
        if (m_life > 0 && other.gameObject.tag == "Bullet") {
            this.Hit(other);
        }
    }

    protected virtual void Hit(Collider _other) {
        m_life -= _other.GetComponent<Skeet_Bullet>().p_attack;

        if (p_animator != null)
            p_animator.Play("Base Layer.ShootBeat");

        if (m_life <= 0)
            Start_Die();
        ChangeLife();
        if (this.GetComponent<AudioSource>().isPlaying == false)
            this.GetComponent<AudioSource>().Play();
    }

    private void Die() {
        Shoot_ScoreCal score = SceneController.Level_Current.GetComponent<Shoot_ScoreCal>();
        if (score != null)
            score.Ellic_KnockDown();
        Destroy(this.gameObject);
    }

    private void Escape() {
        if (m_life > 0) {
            Shoot_ScoreCal score = SceneController.Level_Current.GetComponent<Shoot_ScoreCal>();
            if (score != null)
                score.Ellic_Escape();
            Destroy(this.gameObject);
        }
    }

    private void Start_Die() {
        if (p_animator != null) {
            p_animator.Play("Base Layer.ShootDie");
            Invoke("Die", 2.0f);
        }
        else {
            Die();
        }
    }

    private void Start_Escape() {
        if (m_life > 0) {
            if (p_animator != null) {
                if (Is_Rotate_Escape)
                    this.transform.forward = -this.transform.forward;
                p_animator.Play("Base Layer.ShootEscape");
            }
            Invoke("Escape", 2.5f);
        }
    }

    private void ChangeLife() {
        life_sprite.localScale = new Vector3(m_life / m_life_max * m_life_sprite_scaleX_ori, life_sprite.localScale.y, life_sprite.localScale.z);
    }
}
