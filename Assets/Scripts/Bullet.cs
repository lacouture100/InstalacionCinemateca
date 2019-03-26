using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public GameObject playerFrom;
    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(playerFrom,10);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
