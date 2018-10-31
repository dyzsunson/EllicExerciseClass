using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soccer_ScoreCal : ScoreCalculation {
    private int m_ballIn = 0;

    public void BallIn() {
        m_ballIn++;
    }

    public override void Calculate() {
        this.Calculate(100 - m_ballIn * 2);
    }
}
