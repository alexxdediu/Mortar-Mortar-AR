using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class AutoPlacementController : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    private GameObject towerInstance;

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
            AddFixedJointsToTower(tower);
            tower.transform.position = new Vector3(hitPose.position.x, hitPose.position.y, hitPose.position.z);
            tower.transform.rotation = hitPose.rotation;
            Instantiate(tower);
            towerPlaced = true;

            RemoveFixedJointsFromTower(tower);

        }
    }

    private void AddFixedJointsToTower(GameObject tower)
    {
        Rigidbody[] rigidbodies = tower.GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < rigidbodies.Length; i++)
        {
            FixedJoint fixedJoint = rigidbodies[i].gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidbodies[i - 1];
        }
    }

    private void RemoveFixedJointsFromTower(GameObject tower)
    {
        FixedJoint[] fixedJoints = tower.GetComponentsInChildren<FixedJoint>();
        foreach (FixedJoint joint in fixedJoints)
        {
            Destroy(joint);
        }
    }
}