using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Image colorBackground;
    public string worldColor = "red";

    private List<GameObject> blueWorldObjects = new List<GameObject>();
    private List<GameObject> redWorldObjects = new List<GameObject>();
    // Use this for initialization
    void Awake () {
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in allGameObjects)
        {
            if(gameObject != null)
            {
                if (gameObject.layer == LayerMask.NameToLayer("WaterWorld"))
                    blueWorldObjects.Add(gameObject);
                else if (gameObject.layer == LayerMask.NameToLayer("FireWorld"))
                    redWorldObjects.Add(gameObject);
            }

        }

	}

	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchColor()
    {
        if (worldColor == "blue")
            worldColor = "red";
        else
            worldColor = "blue";

        switch (worldColor)
        {
            case "blue":
                
                colorBackground.color = new Color(0.231f, 0.4549f, 0.9843f, 0.4352f);
                foreach (GameObject blueObject in blueWorldObjects)
                {
                    blueObject.SetActive(true);
                }
                foreach (GameObject redObject in redWorldObjects)
                {
                    redObject.SetActive(false);
                }
                break;
            case "red":
                colorBackground.color = new Color(0.941f, 0.176f, 0.176f, 0.4352f);
                foreach (GameObject blueObject in blueWorldObjects)
                {
                    blueObject.SetActive(false);
                }
                foreach (GameObject redObject in redWorldObjects)
                {
                    redObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
}
