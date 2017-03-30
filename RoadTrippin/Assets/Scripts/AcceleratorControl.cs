using UnityEngine;
using System.Collections;

public class AcceleratorControl : MonoBehaviour {
    [SerializeField]
    WheelCollider[] frontWheelColliders;
    [SerializeField]
    WheelCollider[] allWheelColliders;
    [SerializeField]
    Transform[] wheelModels;
    [SerializeField]
    float maxMotorTorque = -3000;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < allWheelColliders.Length; i++)
        {
            Vector3 newWorldPosition = new Vector3();
            Quaternion newWorldRotation = new Quaternion();
            allWheelColliders[i].GetWorldPose( out newWorldPosition, out newWorldRotation);
            newWorldRotation.y += 180;
            //wheelModels[i].position = newWorldPosition;
            wheelModels[i].rotation = newWorldRotation;


        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < allWheelColliders.Length; i++)
        {
            allWheelColliders[i].motorTorque += maxMotorTorque;
        }
	}
}
