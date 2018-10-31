using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_Difficulty_Item : MonoBehaviour {
    public Color ColorNormal;
    public Color ColorHighlight;

    private GameObject m_a_object;

    public GameObject AObject2D;

    private void Awake() {
        m_a_object = this.transform.Find("A").gameObject;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HighLight() {
        this.GetComponent<Image>().color = ColorHighlight;
        m_a_object.SetActive(true);
        AObject2D.SetActive(true);
    }

    public void DisHighlight() {
        this.GetComponent<Image>().color = ColorNormal;
        m_a_object.SetActive(false);
        AObject2D.SetActive(false);
    }
}
