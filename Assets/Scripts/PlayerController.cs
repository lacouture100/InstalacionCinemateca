using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public bool isLocalPlayer = false;
    // public GameObject bulletPrefab;
    // public Transform bulletSpawn;
    public float rotationSpeed = -30.0f;
    public float translationSpeed = -30.0f;

    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;

    void Start()
    {
    oldPosition = transform.position;
    currentPosition = oldPosition;
    oldRotation = transform.rotation;
    currentRotation = oldRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        var x = CrossPlatformInputManager.GetAxis ("Horizontal") * Time.deltaTime * rotationSpeed;
        var z = CrossPlatformInputManager.GetAxis ("Vertical") * Time.deltaTime * translationSpeed;


        transform.Rotate(0,x,0);
        transform.Translate(0,0,z);
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        if (currentPosition != oldPosition)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);

            oldPosition=currentPosition;
        }
        if (currentRotation != oldRotation)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);

            oldRotation=currentRotation;
        }
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //   //n.CommandShoot();
        //   // CmdFire();
        // }
    }

    // public void CmdFire()
    // {
    //   var bullet = Instantiate(bulletPrefab,
    //                            bulletSpawn.position,
    //                            bulletSpawn.rotation) as GameObject;
    //   Bullet b = bullet.GetComponent<Bullet>();
    // //   b.playerFrom = this.gameObject;
    //   bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 6;
    //   Destroy(bullet, 2.0f);

}
