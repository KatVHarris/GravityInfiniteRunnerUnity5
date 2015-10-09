using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class HandManager : MonoBehaviour
{

    public GameObject BodyManager;
    public GameObject Rotator;
    private PlatformRotate platformRotate;
    public GameObject player;
    public GameObject gameController;
    GameController gameControllerComponent; 
//    public GameObject ovrPlayer; 
//    private OVRInfinitePlayerController ovrPlayerScript; 
    private InfinitePlayerController playerScript; 

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodyManager _BodyManager;

    private float flipTimer;
    private float flipDelay = 2; 

    bool rightHandActive = false;
    bool leftHandActive = false;
    bool shoot = false;
    public bool signalStart = false; 

    int prevState = 0; 

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft }, //Need this for HandSates
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight }, //Needthis for Hand State
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Start()
    {
        Debug.Log("Hand Manager Start"); 
        platformRotate = Rotator.GetComponent<PlatformRotate>();
        //player = GameObject.Find("FPSPreFab");
        
        //playerScript = player.GetComponent<InfinitePlayerController>();
 //       ovrPlayerScript = ovrPlayer.GetComponent<OVRInfinitePlayerController>();
    }

    void Update()
    {
        int state = 0;

        if (BodyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //Left
            platformRotate.TurnLeft();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Right
            platformRotate.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Flip
            platformRotate.Flip();

        }

        //if (playerScript.flipjump)
        //{
        //    // .. increment a timer to count up to restarting.
        //    flipTimer += Time.deltaTime;

        //    if (flipTimer >= flipDelay)
        //    {
        //        playerScript.flipjump = false;

        //    }
        //}


        //if (playerScript.flipjump)
        //    playerScript.flipjump = false; 

        _BodyManager = BodyManager.GetComponent<BodyManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
                Debug.Log("Found Body");
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {

                    //Check Hand State if body is being Tracked
                    state = CheckLeftHandState(body.HandLeftState) + (2 * CheckRightHandState(body.HandRightState));
                Debug.Log("state: " + state);
                    switch (state)
                    {
                        case 0:
                            signalStart = false; 
                            break;
                        case 1:
                            //Left
                            Debug.Log("Left");
                            if (prevState == 0)
                                platformRotate.TurnLeft();
                            break;
                        case 2:
                            //Right
                            Debug.Log("Right");
                            signalStart = true; 
                            if (prevState == 0)
                                platformRotate.TurnRight();
                            break;
                        case 3:
                            //Both
                            Debug.Log("Both");
                            if (prevState == 0)
                            {
                                //                     playerScript.flipjump = true; 
                                platformRotate.Flip();
                            }

                            break;
                        default:
                            break;
                    }
                    prevState = state;
                
            }
        }
    }

    private int CheckHandState(Kinect.HandState handState)
    {
        int result = 0;
        switch (handState)
        {
            //Normal
            case Kinect.HandState.Closed:
                result = 0;
                break;
            //Shoot
            case Kinect.HandState.Open:
                result = 0;
                break;
            //Flip
            case Kinect.HandState.Lasso:
                result = 1;
                break;
            default:
                result = 0;
                break; 
        }
        return result;
    }

    private int CheckLeftHandState(Kinect.HandState handState1)
    {
        int result = 0;
        switch (handState1)
        {
            //Normal
            case Kinect.HandState.Closed:
                leftHandActive = false;
                result = 0;
                break;
            //Shoot
            case Kinect.HandState.Open:
                leftHandActive = false;
                result = 3;
                break;
            //Flip
            case Kinect.HandState.Lasso:
                leftHandActive = true;
                Debug.Log("left");
                result = 1;
                break;
            default:
                result = 0;
                break;
        }
        return result; 
    }

    private int CheckRightHandState(Kinect.HandState handState2)
    {
        int result = 0;
        switch (handState2)
        {
            //Normal
            case Kinect.HandState.Closed:
                rightHandActive = false;
                result = 0;
                break;
            //Shoot
            case Kinect.HandState.Open:
                rightHandActive = false;
                result = 0;
                break;
            //Flip
            case Kinect.HandState.Lasso:
                rightHandActive = true;
                Debug.Log("Right");
                result = 1;
                break;
            default:
                result = 0;
                break;
        }
        return result; 
    }


}
