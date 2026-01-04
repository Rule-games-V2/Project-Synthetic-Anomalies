using UnityEngine;
using UnityEngine.Rendering.Universal;

public class warningLightScript : MonoBehaviour
{
    private Light2D myLight;
    
    public float minIntensity = 0f;
    public float maxIntensity = 50f;
    public float speed = 2f;

    void Start()
    {
        myLight = GetComponent<Light2D>(); 
    }

    void Update()
    {
        if (myLight != null)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
    }
}
