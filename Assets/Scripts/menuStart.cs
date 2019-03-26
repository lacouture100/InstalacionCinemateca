using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuStart : MonoBehaviour {

	public string sceneName = "";
	// Use this for initialization
	public void changemenuscene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
	}

}
