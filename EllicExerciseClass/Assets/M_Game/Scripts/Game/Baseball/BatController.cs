using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour {
    public Transform left_hand;
    public Transform right_hand;
    // Use this for initialization
    void Start () {
        float[] batSize = { 2.0f, 1.5f, 1.0f };
        int difficulty = (int)SceneController.context.CurrentLevel.Difficulty;
        this.transform.Find("bat").localScale = new Vector3(1.5f * batSize[difficulty], 1.5f, 1.5f * batSize[difficulty]); 
	}

    // Update is called once per frame
    void LateUpdate() {
        this.transform.position = right_hand.position;

        float k = 0.7f;
        Vector3 t_up = k * right_hand.forward + (1 - k) * right_hand.up;
        if (t_up == Vector3.zero)
            t_up = new Vector3(0.0f, 0.0f, 1.0f);
        this.transform.up = t_up;
       
        // this.transform.up = right_hand.up;
    }
}
