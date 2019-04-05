using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverMarcador : MonoBehaviour
{
    private float startTime;
    private float journeyLength;

    private Transform startMarker;
    public Transform endMarker;
    public float tiempo;

    public bool empezoMovimiento = false;


    // Start is called before the first frame update
    void Start()
    {
        startMarker = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(empezoMovimiento)
        {
            float distCovered = (Time.time - startTime) * 0.01f;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

        }
    }

    public void mover()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

        empezoMovimiento = true;
    }
}
