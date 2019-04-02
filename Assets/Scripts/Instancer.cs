using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instancer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameObject1;
    public int numeroDePantallas;
    public int Xmin,Xmax;
    public int Ymin,Ymax;
    public int Zmin,Zmax;
    private

    void Start()
    {
        InstantiateObject();
    }

    // Update is called once per frame
    void InstantiateObject()
    {
        for ( int i=0;i<numeroDePantallas;i++)
        {
            Instantiate(gameObject1,GeneratedPosition(),  Quaternion.Euler(new Vector3(-270, 0, 180)));
        }
    }
    Vector3 GeneratedPosition()
    {
        int x,y,z;
        x = UnityEngine.Random.Range(Xmin,Xmax);
        y = UnityEngine.Random.Range(Ymin,Ymax);
        z = UnityEngine.Random.Range(Zmin,Zmax);
        return new Vector3(x,y,z);
    }
}
