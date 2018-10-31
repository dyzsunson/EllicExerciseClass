using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ShootController {
    public GameObject BigBulletPrefab;
    public GameObject NaughtyBulletPrefab;
    public GameObject ThreeBulletPrefab;

    float m_reload_gun_speed = 1.0f;
    Vector3 m_gun_prePosition;

    enum BulletType {
        Normal = 3,
        BigBullet = 0, 
        ThreeBullet = 1,
        NaughtyBullet = 2
    }

    private BulletType m_bulletType = BulletType.Normal;

    float m_firePower = 0.0f;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        m_gun_prePosition = GunBodyTransform.localPosition;
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
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

    protected override void ReloadEnd() {
        base.ReloadEnd();
        GunBodyTransform.localPosition = m_gun_prePosition;
    }

    void FireContinuousBullet() {
        GameObject bullet = Instantiate(ThreeBulletPrefab) as GameObject;
        this.FireOneBullet(bullet, m_firePower);
    }

    protected override GameObject Fire() {
        m_firePower = m_power;

        m_bulletType = (BulletType) AI.BulletType;
       
        GameObject bullet = null;
        if (m_bulletType == BulletType.ThreeBullet) {
            
            bullet = Instantiate(ThreeBulletPrefab) as GameObject;
            Invoke("FireContinuousBullet", 0.2f);
            Invoke("FireContinuousBullet", 0.4f);
        }
        else if (m_bulletType == BulletType.BigBullet)
            bullet = Instantiate(BigBulletPrefab) as GameObject;
        else if (m_bulletType == BulletType.NaughtyBullet)
            bullet = Instantiate(NaughtyBulletPrefab) as GameObject;
        else
            bullet = Instantiate(BulletPrefab) as GameObject;

        if (m_bulletType == BulletType.BigBullet)
            FireOneBullet(bullet, 100.0f * m_power);
        else
            FireOneBullet(bullet, m_power);

        this.transform.parent.GetComponent<EllicBallController>().Fire();

        return bullet;
    }
}
