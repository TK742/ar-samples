using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class IPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI logText;

    ARRaycastManager m_RaycastManager;
    ARPlaneManager m_PlaneManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    Queue<string> logs;


    private void Start()
    {
        logs = new Queue<string>();
        m_RaycastManager = FindObjectOfType<ARRaycastManager>();
        m_PlaneManager = FindObjectOfType<ARPlaneManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Log($"this is random number {Random.Range(0, 10)}");

        if (Input.touchCount == 0)
            return;

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            foreach (var hit in hits)
                HandleRaycast(hit);
        }
    }


    private void HandleRaycast(ARRaycastHit hit)
    {
        // Determine if it is a plane
        if ((hit.hitType & TrackableType.Planes) != 0)
        {
            // Look up the plane by id
            var plane = m_PlaneManager.GetPlane(hit.trackableId);

            // Do something with 'plane':
            Log($"Hit a plane with alignment {plane.alignment}");
        }
        else
        {
            // What type of thing did we hit?
            Log($"Raycast hit a {hit.hitType}");
        }
    }

    private void Log(string logLine)
    {
        if (logs.Count >= 7)
            logs.Dequeue();
        logs.Enqueue(logLine);
        logText.text = "";
        foreach (var log in logs)
            logText.text += $"{log}\n";
    }
}
