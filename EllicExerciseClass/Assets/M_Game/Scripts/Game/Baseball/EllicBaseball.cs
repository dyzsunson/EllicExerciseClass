using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicBaseball : EllicBallController {
    public override void GameReady() {
        base.GameReady();
        m_max_degree = 0.2f;
    }

    public override void Fire() {
        this.transform.Find("Ellic").GetComponent<Animator>().Play("Base Layer.VolleyHit");
    }
}
