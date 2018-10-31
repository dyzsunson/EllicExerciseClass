using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeetScoreCal : ScoreCalculation {
    private int m_score = 0;
    private int m_hit = 0;
    private int m_miss = 0;

    public void SkeetHit() {
        m_hit++;
    }

    public void SkeetMiss() {
        m_miss++;
    }

    public override void Calculate() {
        int score = m_hit * 100 / (m_hit + m_miss);
        // this.Calculate(Mathf.Min(100, score));

        this.Calculate(100 - m_miss * 2);
    }
}
