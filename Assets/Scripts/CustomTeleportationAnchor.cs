using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Correct namespace for Vignette
using UnityEngine.XR.Interaction.Toolkit;

public class CustomTeleportationAnchor : TeleportationAnchor
{
    public Transform target;
    public float duration;
    public Volume volume;
    private Vignette vignette;

    private bool isTeleporting;

    private void Start()
    {
        // Try to get the Vignette component from the Volume
        if (volume.profile.TryGet<Vignette>(out var v))
        {
            vignette = v;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Call the base class implementation
        base.OnSelectEntered(args);

        // Check if the object is not currently teleporting
        if (!isTeleporting)
        {
            // Start a coroutine to interpolate the transform to the target position over a specified duration
            StartCoroutine(LerpToTarget(args.interactor.transform.root, target.position, duration));
        }
    }

    private IEnumerator LerpToTarget(Transform xrRig, Vector3 targetPosition, float duration)
    {
        // Set teleporting flag to true to prevent multiple teleportations at the same time
        isTeleporting = true;

        // If a Vignette component is attached, enable it for the duration of the teleportation
        if (vignette != null)
        {
            vignette.active = true;
        }

        // Initialize elapsed time
        float elapsedTime = 0f;

        // Continue to lerp the position of the XR Rig until the specified duration has passed
        while (elapsedTime < duration)
        {
            // Calculate the fraction of the total duration that has elapsed
            float t = elapsedTime / duration;

            // Apply a smooth step function to make the transition smoother
            //t = t * t * (3f - 2f * t);

            // Lerp the position of the XR Rig towards the target position
            xrRig.position = Vector3.Lerp(xrRig.position, targetPosition, t);

            // Increment the elapsed time by the time since the last frame
            elapsedTime += Time.unscaledDeltaTime;

            // Wait for the next frame before continuing the loop
            yield return new WaitForEndOfFrame();
        }

        // Snap the XR Rig to the target position to ensure it arrives exactly at the target
        xrRig.position = targetPosition;

        // If a Vignette component is attached, disable it now that the teleportation is complete
        if (vignette != null)
        {
            vignette.active = false;
        }

        // Set teleporting flag to false to allow future teleportations
        isTeleporting = false;
    }


    
    /// Generates a teleport request for the XRBaseInteractor using the provided raycast hit and updates the teleport request reference parameter.
    protected override bool GenerateTeleportRequest(XRBaseInteractor interactor, RaycastHit raycastHit, ref TeleportRequest teleportRequest)
    {
        // Check if a teleport is already in progress
        if (isTeleporting)
        {
            return false; // Return false if teleport is already in progress
        }

        // Set destination position and start lerping to target position
        teleportRequest.destinationPosition = target.position;
        StartCoroutine(LerpToTarget(interactor.transform.root, target.position, duration));
        
        return true; // Return true to indicate successful generation of teleport request
    }
}
