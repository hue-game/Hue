using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Image colorBackground;
    public GameObject controlCanvas;
    public GameObject pauseCanvas;
    public GameObject player;

    private string worldColor;
    private List<GameObject> blueWorldObjects = new List<GameObject>();
    private List<GameObject> redWorldObjects = new List<GameObject>();
    private bool isPaused = false;

    // Use this for initialization
    void Awake () {
        //Start in blue world which will then be switched to red at start (change this to color eventually)
        worldColor = "blue";

        //Get an array of all GameObjects in the scene
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in allGameObjects)
        {
            if(gameObject != null)
            {
                //Add objects with a blue or red layer (to be changed to gray and color) to their respective lists
                if (gameObject.layer == LayerMask.NameToLayer("WaterWorld"))
                    blueWorldObjects.Add(gameObject);
                else if (gameObject.layer == LayerMask.NameToLayer("FireWorld"))
                    redWorldObjects.Add(gameObject);
            }
        }

        //Call SwitchColor at start to change the background color and correctly set the gameObjects for that world (assume you always start in the grey world)
        SwitchColor();
	}

    public void Update()
    {
        //Check for escape (later add back button on phone or pause button on control canvas
        if (Input.GetButtonDown("Cancel"))
            PauseGame();
    }

    public void SwitchColor()
    {
        //Change what color we want to switch to
        if (worldColor == "blue")
            worldColor = "red";
        else
            worldColor = "blue";

        //Change the actual background color of the canvas and the color of the player, also turn on or off the objects for the gray and color world
        switch (worldColor)
        {
            case "blue":
                colorBackground.color = new Color(0.231f, 0.4549f, 0.9843f, 0.4352f);
                player.GetComponent<SpriteRenderer>().color = Color.blue;

                //For each object in the both worlds enable/disable the collider and change the opacity of the sprite
                foreach (GameObject blueObject in blueWorldObjects)
                {
                    blueObject.GetComponent<SpriteRenderer>().color = Color.white * 1.0f;
                    blueObject.GetComponent<Collider2D>().enabled = true;
                }
                
                foreach (GameObject redObject in redWorldObjects)
                {
                    redObject.GetComponent<SpriteRenderer>().color = Color.white * 0.1f;
                    redObject.GetComponent<Collider2D>().enabled = false;

                }
                break;
            case "red":
                colorBackground.color = new Color(0.941f, 0.176f, 0.176f, 0.4352f);
                player.GetComponent<SpriteRenderer>().color = Color.blue;

                foreach (GameObject blueObject in blueWorldObjects)
                {
                    blueObject.GetComponent<SpriteRenderer>().color = Color.white * 0.1f;
                    blueObject.GetComponent<Collider2D>().enabled = false;
                }
                
                foreach (GameObject redObject in redWorldObjects)
                {
                    redObject.GetComponent<SpriteRenderer>().color = Color.white * 1.0f;
                    redObject.GetComponent<Collider2D>().enabled = true;
                }
                break;
            default:
                break;
        }
    }

    //Function to pause game, pausing the game will set the timeScale to 0 and enable the pause Canvas
    private void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            controlCanvas.SetActive(false);
            pauseCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            controlCanvas.SetActive(true);
            pauseCanvas.SetActive(false);
        }

    }

}
