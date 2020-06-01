using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis; //Axis that the joint will rotate about
    public Vector3 startOffset; 

    void Awake()
    {
        startOffset = transform.localPosition;
    }
}
