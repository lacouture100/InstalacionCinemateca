using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTime : MonoBehaviour {

	// Use this for initialization


    public float time = 10; //Seconds to read the text

    void Start()
    {
        Destroy(gameObject, time);
    }
}
