using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalikkapeliRobot : MonoBehaviour 
{
//    public GameObject robotStartPoint;
    public GameObject robotHorizontalEndPoint;
    public GameObject robotVerticalEndPoint;

    public GameObject robot, robotArmature;
    public GameObject scanner;

    public GameObject palikkapeli;

    public PalikkaWatch pWatch;

    public Animator scannerAnimator;

    public float closeEnoughDistance = 0.2f; // Because getting to exact position takes forever
    public bool horizontallyCloseEnough = false;
    public bool verticallyCloseEnough = false;

    public bool handsRotated = false;

    public float robotMovementSpeed = 0.5f;
    public float robotRotationSpeed = 1f;

    private Palikkapeli pPeli;

    public bool boxReady = false;
    public bool readyConfirmed = false;

    public GameObject robotLeftHand, robotRighthand, robotHead, rightEye, leftEye, eyeTarget, railThingys, uselessDoor;

    public Vector3 robotStartPoint;
    public Vector3 currentLeftHandRotation;
    public Vector3 currentRightHandRotation;
    public Vector3 currentHeadRotation;

    public Vector3 openLeftHandRotation;
    public Vector3 openRightHandRotation;
    public Vector3 turnedHeadRotation;

    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;
    private bool step4 = false;
    private bool step5 = false;
    private bool step6 = false;
    private bool step7 = false;
    private bool step8 = false;

    private Vector3 tempPos;
    private Vector3 tempPos2;
    public Vector3 tempRot;


    private bool tempPosSet = false;
    private bool tempPos2Set = false;
    private bool tempRotSet = false;

    public bool scanningAnimationStarted = false;

    public float startTime = 0;
    public Vector3 robotVelocity = Vector3.zero;
    public float robotSmoothDampening = 1f;

	void Start () 
    {

//        robotStartPoint = this.transform.FindChild("RobotStartPoint").gameObject;
        robotStartPoint = this.transform.position;

//        robotHorizontalEndPoint = this.transform.FindChild("HorizontalEndPoint").gameObject;
        robotVerticalEndPoint = robotHorizontalEndPoint.transform.FindChild("VerticalEndPoint").gameObject;

//        robot = this.transform.FindChild("Robot").gameObject;
        robot = this.transform.FindChild("ClearParent/Robot").gameObject;
        robotArmature = this.transform.FindChild("ClearParent/Robot/RobotArmature").gameObject;

        scannerAnimator = scanner.GetComponent<Animator>();       

        // Find robot parts
//        robotLeftHand = this.transform.FindChild("Robot/BodyBone/Arm_L").gameObject;
//        robotRighthand = this.transform.FindChild("Robot/BodyBone/Arm_R").gameObject;

        robotLeftHand = this.transform.FindChild("ClearParent/Robot/RobotArmature/RotationBone/BodyBone/Arm_L").gameObject;
        robotRighthand = this.transform.FindChild("ClearParent/Robot/RobotArmature/RotationBone/BodyBone/Arm_R").gameObject;

        // Rail thingys
        railThingys = this.transform.FindChild("ClearParent/RailThingys").gameObject;

        robotHead = this.transform.FindChild("ClearParent/Robot/RobotArmature/RotationBone/BodyBone/NeckBone").gameObject;
        rightEye = this.transform.FindChild("ClearParent/Robot/RobotArmature/RotationBone/BodyBone/NeckBone/HeadBone/RightEye").gameObject;
        leftEye = this.transform.FindChild("ClearParent/Robot/RobotArmature/RotationBone/BodyBone/NeckBone/HeadBone/LeftEye").gameObject;

        palikkapeli = this.transform.Find("ClearParent/Robot/RobotArmature/Palikkaboxi").gameObject;

        // These only work properly with positive numbers from 0 to 360
        openLeftHandRotation = new Vector3(288, 0, 176);
        openRightHandRotation = new Vector3(291, 352, 192);
        turnedHeadRotation = new Vector3(25, 390, 321);

        currentLeftHandRotation = robotLeftHand.transform.eulerAngles;
        currentRightHandRotation = robotRighthand.transform.eulerAngles;
        currentHeadRotation = robotHead.transform.eulerAngles;

        pPeli = this.GetComponentInChildren<Palikkapeli>();
        pWatch = this.GetComponentInChildren<PalikkaWatch>();

	}
	
	void Update ()
    {

        if (pPeli.getPuzzleState())
        {
            if (!readyConfirmed)
            {
                boxReady = true;
                pPeli.pw.disableChecking();
                readyConfirmed = true;

                // We destroy all palikat here because otherwise they mess up box movement no matter wether they are kinematic or not for some reason
                pWatch.destroyPalikat();

            }
        }

        if (boxReady)
        {
            if (!step1) // Going up
            {
                // Run this only once
                if (!tempPosSet)
                {
                    tempPos = robot.transform.position;
                    tempPos.y += 6;
                    tempPosSet = true;
                }

                startTime = Time.time;

                robot.transform.position = Vector3.SmoothDamp(robot.transform.position, tempPos, ref robotVelocity, robotSmoothDampening);

                // Check when close enough
                if (Vector3.Distance(robot.transform.position, tempPos) < closeEnoughDistance)
                {
                    step1 = true;
                    tempPosSet = false;
                }

            }

            if (step1 && !step2) // Going Right
            {
                // Run this only once
                if (!tempPosSet)
                {
                    tempPos = robot.transform.position;
                    tempPos2 = railThingys.transform.position;
                    tempPos.x -= 24.7f;
                    tempPos2.x -= 24.7f;
                    tempPosSet = true;
                }

                // Move
                robot.transform.position = Vector3.SmoothDamp(robot.transform.position, tempPos, ref robotVelocity, robotSmoothDampening);
                railThingys.transform.position = Vector3.SmoothDamp(railThingys.transform.position, tempPos2, ref robotVelocity, robotSmoothDampening);


                // Check when close enough
                if (Vector3.Distance(robot.transform.position, tempPos) < closeEnoughDistance)
                {
                    step2 = true;
                    tempPosSet = false;
                }
            }

            if (step1 && step2 && !step3) // Going down
            {
                // Run this only once
                if (!tempPosSet)
                {
                    tempPos = robot.transform.position;
                    tempPos.y -= 6.5f;
                    tempPosSet = true;
                }

                // Move
                robot.transform.position = Vector3.SmoothDamp(robot.transform.position, tempPos, ref robotVelocity, robotSmoothDampening);

                // Check when close enough
                if (Vector3.Distance(robot.transform.position, tempPos) < closeEnoughDistance)
                {
                    step3 = true;
                    tempPosSet = false;
                }
            }


            if (step1 && step2 && step3 && !step4) // Turning the robot towards scanner
            {
                if (!tempRotSet)
                {
                    tempRot = robotArmature.transform.rotation.eulerAngles;
                    tempRot.y -= 60;
                    tempRotSet = true;
                }

                robotArmature.transform.eulerAngles = Vector3.Lerp(robotArmature.transform.eulerAngles, tempRot, Time.deltaTime * robotRotationSpeed);

                // Check when close enough
                if (Quaternion.Angle(Quaternion.Euler(robotArmature.transform.eulerAngles), Quaternion.Euler(tempRot)) < 1f)
                {
                    step4 = true;
                    tempRotSet = false;
                }
            }

            if (step1 && step2 && step3 && step4 && !step5) // Scanner animation
            {
                if (!scanningAnimationStarted)
                {
                    scannerAnimator.Play("Scan");
                    scanningAnimationStarted = true;
                }
               
                if (scannerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
                    step5 = true;
            }

            if (step1 && step2 && step3 && step4 && step5 && !step6) // Opening robot arm
            {
                if (!tempRotSet)
                {
                    tempRot = robotLeftHand.transform.eulerAngles;
                    tempRot.x += 60;
                    tempRotSet = true;
                }

                robotLeftHand.transform.eulerAngles = Vector3.Lerp(robotLeftHand.transform.eulerAngles, tempRot, Time.deltaTime * robotRotationSpeed);

                // Check when close enough
                if (Quaternion.Angle(Quaternion.Euler(robotLeftHand.transform.eulerAngles), Quaternion.Euler(tempRot)) < 3f)
                {
                    step6 = true;
                    tempRotSet = false;
                }
            }

            if (step1 && step2 && step3 && step4 && step5 && step6 && !step7) // Turning robot as in it'd be throwing the box
            {
                if (!tempRotSet)
                {
                    tempRot = robotArmature.transform.eulerAngles;
                    tempRot.y -= 20;
                    tempRotSet = true;
                }

                robotArmature.transform.eulerAngles = Vector3.Lerp(robotArmature.transform.eulerAngles, tempRot, Time.deltaTime * robotRotationSpeed * 6);

                // Check when close enough
                if (Quaternion.Angle(Quaternion.Euler(robotArmature.transform.eulerAngles), Quaternion.Euler(tempRot)) < 1f)
                {
                    step7 = true;
                    tempRotSet = false;
                }
            }

            if (step1 && step2 && step3 && step4 && step5 && step6 && step7 && !step8) // Add forces to the box
            {
                Rigidbody tempRB = palikkapeli.GetComponent<Rigidbody>();

                tempRB.isKinematic = false;
                tempRB.AddForce(-Vector3.forward * 3 , ForceMode.Impulse);
                tempRB.AddTorque(-Vector3.forward, ForceMode.Impulse);

                step8 = true;
                StartCoroutine(OpenUselessDoor());
            }
        }               
	}

    IEnumerator OpenUselessDoor()
    {
        yield return new WaitForSeconds(2f);
        AudioSource uselessdoorAS = uselessDoor.GetComponentInChildren<AudioSource>();
        uselessdoorAS.Play();
        uselessDoor.GetComponent<Animator>().Play("Open");
        yield return new WaitForSeconds(2f);
        uselessdoorAS.Stop();
    }

}
