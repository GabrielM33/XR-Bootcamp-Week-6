using UnityEngine;

public class BladeSwing : MonoBehaviour
{
    public float speed = 0.5f;
    public float maxRotation = 160f;
    private readonly float _length = 1f;
    
    void Start()
    {
        GetComponent<Collider>();
    }

    void Update()
    {
        // Calculate the swing based on the variables speed and max angle rotation
        float swing = Mathf.PingPong(Time.time * speed, _length) * maxRotation;
        
        // Set the new angle to the X-axis rotation of the object
        transform.rotation = Quaternion.Euler(swing, 0, 0);
    }
}