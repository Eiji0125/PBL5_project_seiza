using UnityEngine;
using Valve.VR;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour
{
    public Transform minBound;
    public bool fixX;
    public bool fixY;
    public Transform thumb;
    bool dragging;

    public SteamVR_Behaviour_Pose rightController;

    public SteamVR_Action_Boolean triggerAction = SteamVR_Actions.selectMenu_SelectFromMenu;

    //public SteamVR_Action_Single vec1 = SteamVR_Actions.default_Squeeze;

    //public SteamVR_Action_Vector3 vec3raycast = SteamVR_Actions.selectMenu_SelectRayCast;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    [SerializeField]
    private InputActionReference select;

    private void Start()
    {
        triggerAction.AddOnStateDownListener(TriggerDown, handType);
        triggerAction.AddOnStateUpListener(TriggerUp, handType);

        //vec1.GetAxis( handType);
    }

    private void Awake()
    {
        if(select != null)
        {
            select.action.Enable();
        }
    }

    private void Update()
    {
        //Debug.Log(vec1.GetAxis(handType));

        if (select.action.IsPressed())
        {
            Debug.Log("PRESSED");
        }
        
        if (select.action.triggered)
        {
            Debug.Log("triggered");
        }
    }

    void FixedUpdate()
    {
        //Debug.Log((rightController != null) + "RC");
        if (rightController == null)
        return;

        SteamVR_Action_Boolean triggerAction = SteamVR_Actions.selectMenu_SelectFromMenu;
        //Debug.Log((triggerAction != null) + "TA");


        if (triggerAction.GetStateDown(rightController.inputSource))
        {
            Debug.Log((triggerAction != null) + "Clicked");
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
            Debug.Log((triggerAction != null) + "Released");
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
        Debug.Log(point);
        SetThumbPosition(point);
    }

    void SetThumbPosition(Vector3 point)
    {
        Vector3 temp = thumb.localPosition;
        thumb.position = point;
        thumb.localPosition = new Vector3(fixX ? temp.x : thumb.localPosition.x, fixY ? thumb.localPosition.y : point.y, thumb.localPosition.z - 1);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
        //Sphere.GetComponent<MeshRenderer>().enabled = false;
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        //Sphere.GetComponent<MeshRenderer>().enabled = true;
    }



    public void TriggerUpSelect(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
        //Sphere.GetComponent<MeshRenderer>().enabled = false;
    }
    public void TriggerDownSelect(SteamVR_Action_Single fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        //Sphere.GetComponent<MeshRenderer>().enabled = true;
    }
}
