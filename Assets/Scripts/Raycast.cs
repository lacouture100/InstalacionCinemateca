using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Raycast : MonoBehaviour
{
    public float maxDistance =200;
    private Camera cam;

    // Update is called once per frame

    void  Awake ()
  	{
  		// if no camera referenced, grab the main camera
  		if (!cam)
  			cam = Camera.main;
  	}

    void FixedUpdate()
    {
      // Create a ray from the transform position along the transform's z-axis

        //Ray ray = new Ray(transform.position, -transform.forward);
        //Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        Debug.DrawLine(cam.transform.position, cam.transform.forward - Vector3.forward* maxDistance, Color.red);

        //if (Physics.Raycast(ray, out hit, maxDistance))
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance))

        {
            //string hitObject = hit.collider.gameObject.name;
            //GameObject videoContainer = GameObject.Find(hitObject);
            //VideoPlayer video = videoContainer.GetComponent<VideoPlayer>();
            Debug.Log(hit.collider.name);
            VideoPlayer video = hit.collider.GetComponent<VideoPlayer>();
            video.Play();
        }
        // else
        // {
        //     GameObject[] videos;
        //     videos = GameObject.FindGameObjectsWithTag("Video");
        //     VideoPlayer video = videos.GetComponent<VideoPlayer>();
        //     video.Pause();
        // }
        // else
        // {
        //   GameObject[] getObjects() {
        //       Script[] scripts =FindObjectsOfType<VideoPlayer> ();
        //       GameObject[] objects = new GameObject[scripts.Length];
        //           for (int i = 0; i < objects.Length; i++)
        //           objects [i] = scripts [i].gameobject;
        //
        //       return objects;
        //   }
        //   VideoPlayer video = objects.GetComponent<VideoPlayer>();
        //   video.Pause();


        }

}
