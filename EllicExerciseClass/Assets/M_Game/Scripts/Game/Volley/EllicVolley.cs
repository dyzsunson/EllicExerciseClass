﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllicVolley: EllicBallController {
    /*
    protected override void Start() {
        base.Start();
        m_max_degree = 10.0f;
    }
    */
    public override void Fire() {
        this.transform.Find("Ellic").GetComponent<Animator>().Play("Base Layer.VolleyHit");
    }
    
}
