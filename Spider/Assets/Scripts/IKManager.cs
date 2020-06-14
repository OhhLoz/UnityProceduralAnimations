using UnityEngine;

public class IKManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Joint[] jointArr;
    public float[] angleArr;

    private const float SamplingDistance = 5f;
    private const float LearningRate = 100f;        //How quickly the partial gradient tends towards the target, increasing this makes it faster but more likely to overshoot
    private const float DistanceThreshold = 0.01f;


    private void Start()
    {
        float[] angles = new float[jointArr.Length];

        for (int i = 0; i < jointArr.Length; i++)
        {
            if (jointArr[i].Axis.x == 1)  //If X Axis
                angles[i] = jointArr[i].transform.localRotation.eulerAngles.x;
            else if (jointArr[i].Axis.y == 1)  //If Y Axis
                angles[i] = jointArr[i].transform.localRotation.eulerAngles.y;
            else if (jointArr[i].Axis.z == 1)  //If Z Axis
                angles[i] = jointArr[i].transform.localRotation.eulerAngles.z;
        }
        angleArr = angles;
    }

    private void Update()
    {
        InverseKinematics(target.position, angleArr);
    }

    // Used to indicate the current space the arm occupies
    public Vector3 ForwardKinematics(float[] angles)
    {
        // Saves current position as 'prev' position
        Vector3 prevPoint = jointArr[0].transform.position;
        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < jointArr.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], jointArr[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * jointArr[i].startOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    // Simply returns the current distance to the target
    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }

    // Uses derivatives to 'tend' towards the target destination in each update loop call
    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        // Saves the angle
        float angle = angles[i];

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Restores saved angle from start of function
        angles[i] = angle;

        return gradient;
    }

    // Actually move the objects towards the target as indicated by the gradient value & learning rate
    public void InverseKinematics(Vector3 target, float[] angles)
    {
        for (int i = jointArr.Length - 1; i >= 0; i--)
        {
            // Gradient descent
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= LearningRate * gradient;

            if (jointArr[i].Axis.x == 1)  //If X Axis
                jointArr[i].transform.localEulerAngles = new Vector3(angles[i], 0, 0);
            if (jointArr[i].Axis.y == 1)  //If Y Axis
                jointArr[i].transform.localEulerAngles = new Vector3(0, angles[i], 0);
            if (jointArr[i].Axis.z == 1)  //If Z Axis
                jointArr[i].transform.localEulerAngles = new Vector3(0, 0, angles[i]);
        }
    }
}