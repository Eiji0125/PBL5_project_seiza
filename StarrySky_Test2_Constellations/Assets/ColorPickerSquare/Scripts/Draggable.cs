using UnityEngine;
using Valve.VR;

public class Draggable : MonoBehaviour
{
    public Transform minBound;
    public bool fixX;
    public bool fixY;
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
                var point = hit.point;
                SetThumbPosition(point);
                SendMessage("OnDrag", Vector3.one - (thumb.localPosition - minBound.localPosition) / GetComponent<BoxCollider>().size.x);
            }
        }
    }

    void SetDragPoint(Vector3 point)
    {
        point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
        SetThumbPosition(point);
    }

    void SetThumbPosition(Vector3 point)
    {
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? thumb.localPosition.y : point.y, thumb.localPosition.z - 1);
    }
}
