using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_ScoreCal : ScoreCalculation {
    private int m_escape_num = 0;
    private int m_knockdown_num = 0;

    public void Ellic_Escape() {
        m_escape_num++;
    }

    public void Ellic_KnockDown() {
        m_knockdown_num++;
    }

    public override void Calculate() {
        this.Calculate(100 - m_escape_num * 2);
    }
}
