using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        bool movingDown = other.gameObject.GetComponent<Velocidad>().isMovingDown();

        if (!("" + other.GetType()).Equals("UnityEngine.MeshCollider") && movingDown) playSound();
        
    }

    public void playSound()
    {
        source.Play();
    }

    public void playRepeating(float initTime, float repeatTime)
    {
        InvokeRepeating("playSound", initTime, repeatTime);
    }
    
}
