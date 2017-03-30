using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SteeringControl : MonoBehaviour {
    enum Direction {
    left, right, none};

    [SerializeField]
    private string grabButtonRightName;
    [SerializeField]
    string grabButtonLeftName;
    [SerializeField]
    string steeringAxisName;
    [SerializeField]
    Slider sliderGrabLeftHand;
    [SerializeField]
    Slider sliderGrabRightHand;
    [SerializeField]
    private float grabTimeCoefficient = 1;
    [SerializeField]
    WheelCollider[] frontWheelColliders;
    [SerializeField]
    private int maxSteeringAngle;

    private bool isGrabbingLeft;
    private bool hasGrabbedLeft;
    Direction previousDirectionSteering = Direction.none;
    Direction previousDirectionLeftGrab = Direction.right;
    Direction previousDirectionRightGrab = Direction.left;
    private bool canSteer;
    private bool hasGrabbedRight;
    private bool isGrabbingRight;
    private bool isTurning;

    // Use this for initialization
    void Start () {
        sliderGrabLeftHand.value = 0;
        sliderGrabRightHand.value = 0;
        isGrabbingLeft = false;
        hasGrabbedLeft = false;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateHasGrabbedLeft();
        UpdateHasGrabbedRight();
        UpdateSteering();
    }

    private void UpdateSteering() {

        //if (Input.GetAxis(steeringAxisName) <= 0.01f)
        //{
        //    isTurning = false;
        //}
        if (!isTurning)
        {
            if ((isGrabbingLeft && previousDirectionLeftGrab != previousDirectionSteering) || (isGrabbingRight && previousDirectionRightGrab != previousDirectionSteering))
            {
            if ((Input.GetAxisRaw(steeringAxisName) < 0) && canSteer)
            {
                previousDirectionSteering = Direction.left;
                isTurning = true;
            }
            if ((Input.GetAxisRaw(steeringAxisName) > 0) && canSteer)
            {
                previousDirectionSteering = Direction.right;
                isTurning = true;
            }
            }
        }

        if (isTurning)
            Steer();
    }

    private void Steer() {
        for (int i = 0; i < frontWheelColliders.Length; i++)
            frontWheelColliders[i].steerAngle = Mathf.Clamp(Input.GetAxis(steeringAxisName), -1, 1) * maxSteeringAngle;
    }

    private void UpdateHasGrabbedRight() {
        if (!isGrabbingRight && !hasGrabbedRight && Input.GetAxis(grabButtonRightName) > 0 && !isGrabbingLeft)
            isGrabbingRight = true;
        //Fill slider in x amount of time to grab steering wheel
        if (isGrabbingRight && !hasGrabbedRight)
        {
            GrabFill(sliderGrabRightHand);
            if (sliderGrabRightHand.value >= sliderGrabRightHand.maxValue)
            {
                hasGrabbedRight = true;
                isGrabbingRight = false;
            }
        }
        if (hasGrabbedRight)
        {
            canSteer = true;
            if (Input.GetAxis(grabButtonRightName) < 0.1)
            {
                hasGrabbedRight = false;
                sliderGrabRightHand.value = 0;
                canSteer = false;
                isTurning = false;
            }

        }
    }

    private void UpdateHasGrabbedLeft() {
        if (!isGrabbingLeft && !hasGrabbedLeft && Input.GetAxis(grabButtonLeftName) > 0 && !isGrabbingRight)
            isGrabbingLeft = true;
        //Fill slider in x amount of time to grab steering wheel
        if (isGrabbingLeft && !hasGrabbedLeft)
        {
            GrabFill(sliderGrabLeftHand);
            if (sliderGrabLeftHand.value >= sliderGrabLeftHand.maxValue)
            {
                hasGrabbedLeft = true;
                isGrabbingLeft = false;
            }
        }
        if (hasGrabbedLeft)
        {
            canSteer = true;
            if (Input.GetAxis(grabButtonLeftName) < 0.1)
            {
                hasGrabbedLeft = false;
                sliderGrabLeftHand.value = 0;
            }
        }
    }

    private void GrabFill(Slider sliderGrab) {
        sliderGrab.value += grabTimeCoefficient * Time.deltaTime;
    }
}
