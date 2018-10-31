using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley_Shoot_Controller : ShootController  {
    float m_reload_gun_speed = 0.2f;

    private Volley_Basket[] m_basketArray;

    public GameObject BasketEasy;
    public GameObject BasketMedium;
    public GameObject BasketHard;

    public GameObject Tutorial2DEasy;
    public GameObject Tutorial2DMedium;
    public GameObject Tutorial2DHard;

    public GameObject Tutorial3DEasy;
    public GameObject Tutorial3DMedium;
    public GameObject Tutorial3DHard;

    public Volley_Ball MoneyBall_Prefab;
    public ScoreBoard Double_Board;
    int m_basket_current;

    int m_total_num = 0;

    public Transform p_build_Soccer_Position;
    private GameObject m_current_volley;

    private float m_power_last;
    private GameObject m_last_volley;

    public override void GameReady() {
        base.GameReady();

        switch (SceneController.context.CurrentLevel.Difficulty) {
            case Level.DifficultyLevel.EASY:
                // m_basketArray = BasketEasy.GetComponentsInChildren<Volley_Basket>();
                break;
            case Level.DifficultyLevel.MEDIUM:
                m_basketArray = BasketMedium.GetComponentsInChildren<Volley_Basket>();
                BasketMedium.SetActive(true);
                BasketEasy.SetActive(false);

                Tutorial2DMedium.SetActive(true);
                Tutorial2DEasy.SetActive(false);

                Tutorial3DMedium.SetActive(true);
                Tutorial3DEasy.SetActive(false);

                break;
            case Level.DifficultyLevel.HARD:
                m_basketArray = BasketHard.GetComponentsInChildren<Volley_Basket>();
                BasketHard.SetActive(true);
                BasketEasy.SetActive(false);

                Tutorial2DHard.SetActive(true);
                Tutorial2DEasy.SetActive(false);

                Tutorial3DHard.SetActive(true);
                Tutorial3DEasy.SetActive(false);

                break;
            default:
                break;
        }
    
        if (m_basketArray.Length > 0) {
            for (int i = 0; i < m_basketArray.Length; i++)
                m_basketArray[i].Basket_ID = i;
        }
    }

    public override void GameStart() {
        base.GameStart();
        DeHighLightBasket();
    }

    protected override void Start() {
        base.Start();
        m_max_degree = 0.0f;
        m_max_reloadTime = 2.0f;

        m_basketArray = BasketEasy.GetComponentsInChildren<Volley_Basket>();

        this.BuildVolley();
    }

    protected override GameObject Fire() {
        m_power = (m_power + 6.2f) / 7.5f;

        if (m_current_volley == null)
            return null;

        m_power_last = m_power;
        m_last_volley = m_current_volley;
        Invoke("LateFire", 0.35f);

        this.transform.parent.GetComponent<EllicBallController>().Fire();
        GameObject ball = m_current_volley;
        m_current_volley = null;

        m_last_volley.GetComponent<Rigidbody>().useGravity = true;
        m_last_volley.GetComponent<Rigidbody>().AddForce(150.0f * new Vector3(0.0f, 1.0f, 0.0f));

        return ball;
    }

    protected override void ReloadEnd() {
        base.ReloadEnd();
        this.BuildVolley();
    }

    void LateFire() {
        m_last_volley.GetComponent<Rigidbody>().useGravity = true;
        m_last_volley.GetComponent<Collider>().enabled = true;

        FireOneBullet(m_last_volley, m_power_last, new Vector3(0.0f, 0.0f, 0.0f));
        m_last_volley.GetComponent<Volley_Ball>().Fire();

        if (m_total_num % 5 == 0) {
            ScoreBoard board = Instantiate(Double_Board);
            board.transform.position = m_last_volley.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
        }

        Invoke("HighLightBasket", 0.5f);
    }

    void HighLightBasket() {
        foreach (Volley_Basket basket in m_basketArray)
            basket.WrongBasket();
        m_basketArray[m_basket_current].RightBasket();
        Invoke("DeHighLightBasket", 2.5f);
    }

    void DeHighLightBasket() {
        foreach (Volley_Basket basket in m_basketArray)
            basket.WrongBasket();
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



    void BuildVolley() {
        m_total_num++;
        if (m_total_num % 5 == 0) {
            m_current_volley = Instantiate(MoneyBall_Prefab).gameObject;
        }
        else
            m_current_volley = Instantiate(BulletPrefab);

        if (m_basketArray.Length > 0) {
            m_basket_current = Random.Range(0, m_basketArray.Length);
            m_current_volley.GetComponent<Volley_Ball>().Basket_ID = m_basket_current;
        }

        
        m_current_volley.transform.position = p_build_Soccer_Position.position;
        m_current_volley.transform.SetParent(this.transform.parent.parent);

        m_current_volley.GetComponent<Rigidbody>().useGravity = false;
        m_current_volley.GetComponent<Collider>().enabled = false;

        
    }

}
