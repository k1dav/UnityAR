using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class PlaceObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private Pose pose;
    private bool poseIsValid;

    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera aRCamera;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePose();

        if (poseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            placeObject();
        }
    }

    private void UpdatePose()
    {
        var screenCenter = aRCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits, TrackableType.AllTypes);
        poseIsValid = hits.Count > 0;

        if (poseIsValid)
        {
            pose = hits[0].pose;
            var cameraForward = aRCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            pose.rotation = Quaternion.LookRotation(cameraBearing);
            positionIndicator.SetActive(true);
            positionIndicator.transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
        else
        {
            positionIndicator.SetActive(false);
        }
    }

    private void placeObject()
    {
        Instantiate(prefabToPlace, pose.position, pose.rotation);
    }
}
