using UnityEngine;
using UnityEngine.Events;

// Attach this script to the "Collider" game object of the "MyButton" prefab
public class ButtonScript : MonoBehaviour
{
    public GameObject buttonMesh;
    
    public UnityEvent onPress;
    public UnityEvent onRelease;
    // public UnityEvent onToggle;
    
    private GameObject _triggerCollider;
    private AudioSource _soundEffect;
    
    private bool _isPressed;
    private bool _isToggled;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Assign the trigger collider game object to the triggerCollider variable
        _triggerCollider = GetComponent<Collider>().gameObject;
    
        // Assign the AudioSource component to the soundEffect variable
        _soundEffect = GetComponent<AudioSource>();
    
        // Set the isPressed flag to false
        _isPressed = false;
        _isToggled = false;
    }


    /// Called when another object enters a trigger collider attached to this object.
    private void OnTriggerEnter(Collider collision)
    {
        // Check if the button is not already pressed
        if (!_isPressed)
        {
            // Move the button mesh down near its base
            buttonMesh.transform.localPosition = new Vector3(0f, 0.002f, 0f);

            // Store the collided object
            _triggerCollider = collision.gameObject;
            
            // Toggle the state of the button each time it's pressed
            _isToggled = !_isToggled;
            
            if (_isToggled)
            {
                onPress.Invoke();
            }
            else
            {
                onRelease.Invoke();
            }

            // Play the sound effect
            _soundEffect.Play();

            // Set the button as pressed
            _isPressed = true;
        }
    }
    
    /// Called when a collider exits the trigger collider.
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == _triggerCollider)
        {
            // Move the button mesh to its original position
            buttonMesh.transform.localPosition = new Vector3(0f, 0.02f, 0f);
            
            // Invoke the onRelease event
            //onRelease.Invoke();
            
            // Set the isPressed flag to false
            _isPressed = false;
        }
    }
}
