using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Transform planetCore;
    public Light sun, moon;
    public float sunIntensity = 1f, moonIntensity = 0.45f;
    public float sunsetAngle = 80f, sunriseAngle = 270f;
    public Text debugText;
    //public float distance = 1000f;
    public float angularSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        planetCore.Rotate(Vector3.right, angularSpeed * Time.deltaTime);
        debugText.text = planetCore.rotation.eulerAngles.ToString();
        float rotationX = planetCore.rotation.eulerAngles.x;
        float rotationY = planetCore.rotation.eulerAngles.y;
        if (rotationX <= 89f&& rotationY>=180f)
        {
            sun.intensity = 0f;
            moon.intensity = moonIntensity;
        }
        if (rotationX <= 271f && rotationY <= 0f)
        {
            sun.intensity = sunIntensity;
            moon.intensity = 0f;
        }
    }
}
