using UnityEngine;
using Valve.VR;

public class PaletteSelector : MonoBehaviour
{
    private bool isActive = false;

    public GameObject paletteObj;
    public GameObject lineRenderer;
    public SteamVR_Behaviour_Pose rightController;
    public GameObject sphereIndicator;
    Vector2 touchpad;

    private float minDistance = 0.5f;
    private float maxDistance = 4.0f;

    // Define the correct SteamVR actions
    public SteamVR_Action_Boolean touchpadClickAction; // Assign this in the inspector or in Start()
    public SteamVR_Action_Vector2 touchpadPositionAction; // Assign this in the inspector or in Start()

    // Use this for initialization
    void Start()
    {
        // You can also assign actions here if you haven't done so in the inspector
        // touchpadClickAction = SteamVR_Actions.default_InteractUI; // Replace with your boolean action
        // touchpadPositionAction = SteamVR_Actions.default_Teleport; // Replace with your vector2 action
    }

    void Update()
    {
        if (rightController == null)
            return;

        // Handle touchpad click action
        if (touchpadClickAction.GetStateDown(rightController.inputSource))
        {
            isActive = !isActive;

            // Toggle palette visibility
            if (paletteObj != null)
            {
                paletteObj.SetActive(isActive);
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetActive(isActive);
            }

            if (sphereIndicator != null)
            {
                sphereIndicator.SetActive(isActive);
            }
        }

        // Handle touchpad position action
        if (touchpadPositionAction.GetAxis(rightController.inputSource) != Vector2.zero && !paletteObj.activeSelf)
        {
            // Read the touchpad values
            touchpad = touchpadPositionAction.GetAxis(rightController.inputSource);

            // Handle movement via touchpad
            if ((touchpad.y > 0.2f || touchpad.y < -0.2f) && (touchpad.x < 0.2f && touchpad.x > -0.2f))
            {
                // Move forward based on touchpad input
                sphereIndicator.transform.localPosition = new Vector3(0f, 0f, Mathf.Clamp(touchpad.y * 2.0f + 1.0f, minDistance, maxDistance));
            }
        }
    }
}

