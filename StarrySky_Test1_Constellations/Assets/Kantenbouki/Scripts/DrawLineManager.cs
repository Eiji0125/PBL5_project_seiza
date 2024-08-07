﻿using System.Collections;
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

    List<GameObject> annotations = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        colMgr = ColorManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (trackedObj == null)
            return;

        SteamVR_Action_Boolean triggerAction = SteamVR_Actions.default_Boolean;

        if (triggerAction.GetStateDown(trackedObj.inputSource))
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

                GetComponent<MeshRenderer>().material.color = colMgr.color;

                Vector3 dir = trackedObj.transform.position - trackedObj.transform.forward * 2.0f;
                Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;

                if (ColorManager.Instance.cloudLabel != "_None" && ColorManager.Instance.cloudLabel != null)
                {
                    GameObject label = Instantiate(Resources.Load(ColorManager.Instance.cloudLabel), drawingPoint.transform.position, Camera.main.transform.rotation) as GameObject;
                    if (label.GetComponent<CloudLabelSelect>() != null)
                    {
                        Destroy(label.GetComponent<CloudLabelSelect>());
                    }

                    label.transform.eulerAngles = new Vector3(label.transform.eulerAngles.x, label.transform.eulerAngles.y, 180);
                    label.transform.localScale = new Vector3(0.5F, 0.5f, 0);
                    label.transform.parent = go.transform;

                    if (label.GetComponent<Collider>() != null)
                    {
                        Destroy(label.GetComponent<Collider>());
                    }
                }

                annotations.Add(go);
            }
        }
        else if (triggerAction.GetState(trackedObj.inputSource))
        {
            if (currLine != null)
            {
                currLine.AddPoint(drawingPoint.transform.position);
            }
        }
        else if (triggerAction.GetStateUp(trackedObj.inputSource))
        {
            if (annotations.Count > 0)
            {
                GameObject destroyed = annotations[annotations.Count - 1];
                annotations.Remove(destroyed);
                Destroy(destroyed);
            }
        }
    }
}
