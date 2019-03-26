using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour
{

    public OSC osc;

    public Light light1;
    public float ahora = 0.0f;

    // Use this for initialization
    void Start()
    {
        osc.SetAddressHandler("/luzX", LightIntensityControl);
        light1 = GetComponent<Light>();


    }

    // Update is called once per frame
    void Update()
    {
        ///light1.intensity = ahora;
    }

    void LightIntensityControl(OscMessage message)
    {
        float intensityOSC = message.GetFloat(0);
        float intensity = light1.intensity;
        intensity = intensityOSC;
        light1.intensity = intensity;


    }
}