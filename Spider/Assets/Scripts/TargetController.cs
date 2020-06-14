using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        var newXPos = transform.position.x + deltaX;
        float deltaZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        var newZPos = transform.position.z + deltaZ;
        var newYPos = transform.position.y;

        if(Input.GetKey("space"))
        {
             newYPos = transform.position.y + (moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey("left shift"))
        {
             newYPos = transform.position.y - (moveSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(newXPos, newYPos, newZPos);
    }

    //Space = UP, Shift = Down, WASD = Movement horizontal, X = random pos around arm?
}
