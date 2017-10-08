using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour {

    private static BackGroundMusic instance = null;

    static BackGroundMusic Instance {
        get { return instance;  }
    }

    // Use this for initialization
    void Awake () {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            GetComponent<AudioSource>().Play();
        }

        DontDestroyOnLoad(gameObject);
	}

}
