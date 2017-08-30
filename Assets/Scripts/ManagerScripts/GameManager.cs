using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Image colorBackground;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchColor(string color)
    {
        switch (color)
        {
            case "blue":
                colorBackground.color = new Color(0.231f, 0.4549f, 0.9843f, 0.4352f);
                break;
            case "red":
                colorBackground.color = new Color(0.941f, 0.176f, 0.176f, 0.4352f);
                break;
            default:
                break;
        }

    }
}
