using UnityEngine;
using Valve.VR;

public class CloudLabelSelect : MonoBehaviour
{
    public Transform thumb;
    bool dragging;

    public SteamVR_Behaviour_Pose rightController;

    void FixedUpdate()
    {
        if (rightController == null)
            return;

        SteamVR_Action_Boolean triggerAction = SteamVR_Actions.default_Boolean;

        if (triggerAction.GetStateDown(rightController.inputSource))
        {
            dragging = false;
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                dragging = true;
                ColorManager.Instance.cloudLabel = hit.collider.gameObject.name;
                thumb.position = hit.collider.gameObject.transform.position;
            }
        }
        if (triggerAction.GetStateUp(rightController.inputSource))
        {
            dragging = false;
        }
        if (dragging && triggerAction.GetState(rightController.inputSource))
        {
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                ColorManager.Instance.cloudLabel = hit.collider.gameObject.name;
                thumb.position = hit.collider.gameObject.transform.position;
            }
        }
    }
}
