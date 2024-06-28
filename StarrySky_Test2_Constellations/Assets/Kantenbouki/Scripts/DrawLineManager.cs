using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DrawLineManager : MonoBehaviour
{
    public SteamVR_Behaviour_Pose trackedObj;
    public GameObject drawingPoint;
    public ColorManager colMgr;
    private MeshLineRenderer currLine;
    public Material lMat;
    private float timer = 0;
    private List<GameObject> annotations = new List<GameObject>();
    private SteamVR_Action_Boolean triggerAction;

    void Start()
    {
        colMgr = ColorManager.Instance;
        triggerAction = SteamVR_Actions.default_InteractUI;
        Debug.Log("DrawLineManager initialized.");


        if (triggerAction == null)
        {
            Debug.LogError("Trigger action is not initialized correctly");
        }
    }



    void Update()
    {
        timer += Time.deltaTime;
        // Debug.Log("Update method running"); // 不要なデバッグログはコメントアウトまたは削除

        if (trackedObj == null)
        {
            Debug.LogWarning("Tracked object is null");
            return;
        }

        if (triggerAction == null)
        {
            Debug.LogError("Trigger action is not initialized");
            return;
        }

        // トリガーが押されたときの処理
        if (triggerAction.GetStateDown(trackedObj.inputSource))
        {
            Debug.Log("Trigger pressed down");
            CreateNewLine();
        }
        // トリガーが押され続けているときの処理
        else if (triggerAction.GetState(trackedObj.inputSource))
        {
            Debug.Log("Trigger held down");
            AddPointToLine();
        }
        // トリガーが離されたときの処理
        else if (triggerAction.GetStateUp(trackedObj.inputSource))
        {
            Debug.Log("Trigger released");
            EndLine();
        }
    }


    private void CreateNewLine()
    {
        if (timer > 1.0f)
        {
            timer = 0;
            GameObject go = new GameObject(ColorManager.Instance.cloudLabel);
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();

            currLine = go.AddComponent<MeshLineRenderer>();
            currLine.setColorManager(colMgr);
            currLine.setWidth(0.1f);
            currLine.lmat = this.lMat;

            go.GetComponent<MeshRenderer>().material.color = colMgr.color;

            if (ColorManager.Instance.cloudLabel != "_None" && ColorManager.Instance.cloudLabel != null)
            {
                GameObject label = Instantiate(Resources.Load(ColorManager.Instance.cloudLabel), drawingPoint.transform.position, Camera.main.transform.rotation) as GameObject;
                if (label.GetComponent<CloudLabelSelect>() != null)
                {
                    Destroy(label.GetComponent<CloudLabelSelect>());
                }

                label.transform.eulerAngles = new Vector3(label.transform.eulerAngles.x, label.transform.eulerAngles.y, 180);
                label.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                label.transform.parent = go.transform;

                if (label.GetComponent<Collider>() != null)
                {
                    Destroy(label.GetComponent<Collider>());
                }
            }

            annotations.Add(go);
        }
    }

    private void AddPointToLine()
    {
        if (currLine != null)
        {
            currLine.AddPoint(drawingPoint.transform.position);
        }
    }

    private void EndLine()
    {
        if (annotations.Count > 0)
        {
            GameObject destroyed = annotations[annotations.Count - 1];
            annotations.Remove(destroyed);
            Destroy(destroyed);
        }
    }
}
