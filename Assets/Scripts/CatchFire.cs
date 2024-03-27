using UnityEngine;

public class CatchFire : MonoBehaviour
{
    public ParticleSystem fire;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>();
    }

    // Reacts to collisions with specific objects by playing/stopping the fire particle system
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected with: " + collision.tag);

        // If the collider has the "Torch" tag, play the fire particle system.
        if (collision.CompareTag("Torch"))
        {
            fire.Play();
            Debug.Log("Torch is on fire!");
        }
        // If the collider has the "Water" tag, stop the fire particle system.
        else if (collision.CompareTag("Water"))
        {
            fire.Stop();
            Debug.Log("Fire is extinguished!");
        }
    }
}
