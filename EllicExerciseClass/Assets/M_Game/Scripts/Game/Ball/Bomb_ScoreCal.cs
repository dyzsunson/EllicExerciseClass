using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_ScoreCal : ScoreCalculation {
    private int m_totalEgg;
    private int m_hitEgg = 0;

    public int TotalEgg {
        set {
            m_totalEgg = value;
            m_hitEgg = 0;
        }
    }

    public void EggBreak() {
        m_hitEgg++;
    }

    public override void Calculate() {
        this.Calculate(100 - m_hitEgg);
    }
}
