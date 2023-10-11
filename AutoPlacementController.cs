using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AutoPlacementController : MonoBehaviour
{
    [SerializeField] private GameObject tower;

    private ARRaycastManager aRRaycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool towerPlaced = false;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!towerPlaced && aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            tower.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            tower.transform.position = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);
            tower.transform.rotation = Quaternion.LookRotation(hitPose.up, Vector3.up) * Quaternion.Euler(90, 0, 0); Instantiate(tower);

            towerPlaced = true;
        }
    }
}
