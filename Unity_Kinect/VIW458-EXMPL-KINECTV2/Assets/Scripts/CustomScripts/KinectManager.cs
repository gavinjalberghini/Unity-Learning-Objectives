using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectManager : MonoBehaviour
{

    GameObject player;
    private BodySourceManager bodyManager;

    private float[] headPos = new float[3];
    private float[] leftHandPos = new float[3];
    private float[] rightHandPos = new float[3];
    private float[] relativePos = new float[3];
    private bool initialized = false;


    // Start is called before the first frame update
    void Start()
    {
        bodyManager = GetComponent<BodySourceManager>();
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyManager == null)
            return;

        Windows.Kinect.Body[] data = bodyManager.GetData();
        if (data == null)
            return;
        

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                headPos[0] = body.Joints[Windows.Kinect.JointType.Head].Position.X;
                headPos[1] = body.Joints[Windows.Kinect.JointType.Head].Position.Y;
                headPos[2] = body.Joints[Windows.Kinect.JointType.Head].Position.Z;

                leftHandPos[0] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.X;
                leftHandPos[1] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.Y;
                leftHandPos[2] = body.Joints[Windows.Kinect.JointType.HandLeft].Position.Z;

                rightHandPos[0] = body.Joints[Windows.Kinect.JointType.HandRight].Position.X;
                rightHandPos[1] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Y;
                rightHandPos[2] = body.Joints[Windows.Kinect.JointType.HandRight].Position.Z;

                if (leftHandPos[1] > headPos[1] && !initialized)
                {
                    relativePos[0] = body.Joints[Windows.Kinect.JointType.Head].Position.X;
                    relativePos[1] = body.Joints[Windows.Kinect.JointType.Head].Position.Y;
                    relativePos[2] = body.Joints[Windows.Kinect.JointType.Head].Position.Z;
                    initialized = true;
                }

                break;
            }
        }

        if (rightHandPos[1] > headPos[1] && initialized)
        {
            initialized = false;
        }

        if (initialized)
        {

            Debug.Log("REL :: X: " + relativePos[0] + ", Y: " + relativePos[1] + ", Z: " + relativePos[2]);
            Debug.Log("Head :: X: " + headPos[0] + ", Y: " + headPos[1] + ", Z: " + headPos[2]);
            //Debug.Log("LH :: X: " + leftHandPos[0] + ", Y: " + leftHandPos[1] + ", Z: " + leftHandPos[2]);
            //Debug.Log("RH :: X: " + rightHandPos[0] + ", Y: " + rightHandPos[1] + ", Z: " + rightHandPos[2]);

            //Left and right motion
            float magnitudeRot = Mathf.Abs(headPos[0] - relativePos[0]);
            float magnitudeMov = Mathf.Abs(headPos[2] - relativePos[2]);
            if (magnitudeRot > 0.2)
            {
                if(headPos[0] > relativePos[0])
                {
                    player.transform.Rotate(0, 2.5f, 0);
                } else
                {
                    player.transform.Rotate(0, -2.5f, 0);
                }
            } else if (magnitudeMov > 0.1)
            {
                if (headPos[2] > relativePos[2])
                {
                    player.transform.Translate(-player.transform.forward * 0.125f, Space.World);
                }
                else
                {
                    player.transform.Translate(player.transform.forward * 0.125f, Space.World);
                }
            }
        }
    }
}
