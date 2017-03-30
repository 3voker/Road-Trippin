using UnityEngine;
using System.Collections;
using System;

public class HandMovement : MonoBehaviour {

    public bool isAttached;

    [SerializeField]
    Transform handAttachPoint;
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    string grabInputAxisName;
    [SerializeField]
    string steerInputAxisName;
    [SerializeField]
    private float handMovementTime = 0.5f;
    [SerializeField]
    private float detectionRadius = .5f;

    AnimationCurve movementAnimationCurveX;
    AnimationCurve movementAnimationCurveY;
    AnimationCurve movementAnimationCurveZ;
    private bool isMovingHand;
    private float animationFrameCounter;
    private Vector3 startingPosition;

    // Use this for initialization
    void Start () {
        isMovingHand = false;
        isAttached = false;
        animationFrameCounter = 0;
        transform.parent = playerTransform;
        startingPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        CheckForMoveHand();
        CheckAttachment();
        CheckForInitiateGrab();
    }

    private void CheckForInitiateGrab() {
        if (!isMovingHand && !isAttached && Input.GetAxis(grabInputAxisName) > 0)
        {
            TellHandToMove(handAttachPoint.position);
        }
    }

    private void CheckAttachment() {
        if (isAttached)
        {
            if (Input.GetAxis(grabInputAxisName) > 0)
            {
                transform.parent = handAttachPoint;
            }
            else
            {
                isAttached = false;
                transform.parent = playerTransform;
                TellHandToMove(startingPosition);
            }
        }
    }

    private void CheckForMoveHand() {
        if (isMovingHand)
        {
            MoveHandAlongAnimationCurves();
            CheckEndOfAnimation();
        }
    }

    private void CheckEndOfAnimation() {
        if (animationFrameCounter >= handMovementTime)
        {
            isMovingHand = false;
            Collider[] collidersInRange = Physics.OverlapSphere(transform.position, detectionRadius);
            if (collidersInRange.Length > 0)
                for (int i = 0; i < collidersInRange.Length; i++)
                    if (collidersInRange[i].gameObject == handAttachPoint.gameObject)
                        isAttached = true;
        }
    }

    private void MoveHandAlongAnimationCurves() {
        animationFrameCounter += Time.deltaTime;
        float newPositionX = movementAnimationCurveX.Evaluate(animationFrameCounter);
        float newPositionY = movementAnimationCurveY.Evaluate(animationFrameCounter);
        float newPositionZ = movementAnimationCurveZ.Evaluate(animationFrameCounter);
        transform.position = new Vector3(newPositionX, newPositionY, newPositionZ);
    }

    private void TellHandToMove(Vector3 targetPosition) {
        movementAnimationCurveX = new AnimationCurve(new Keyframe(0, transform.position.x), new Keyframe(handMovementTime, targetPosition.x));
        movementAnimationCurveY = new AnimationCurve(new Keyframe(0, transform.position.y), new Keyframe(handMovementTime, targetPosition.y));
        movementAnimationCurveZ = new AnimationCurve(new Keyframe(0, transform.position.z), new Keyframe(handMovementTime, targetPosition.z));
        isMovingHand = true;
        animationFrameCounter = 0;
    }

    
}
