using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocidad : MonoBehaviour
{
    private Vector3 prevLoc;
    private Vector3 curVel;
    private bool movingDown;

    // Start is called before the first frame update
    void Start()
    {
        prevLoc = transform.position;
        movingDown = false;
    }

    private void FixedUpdate()
    {
        Vector3 curVel = (transform.position - prevLoc) / Time.deltaTime;

        movingDown = curVel.y <= 0;

        prevLoc = transform.position;
    }

    public bool isMovingDown()
    {

        return movingDown;
    }
}
