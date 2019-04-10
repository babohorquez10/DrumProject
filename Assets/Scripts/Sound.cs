using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int max;
    private MainScript master;

    public bool termino = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        master = GameObject.Find("Master").GetComponent<MainScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        bool movingDown = other.gameObject.GetComponent<Velocidad>().isMovingDown();

        if (!("" + other.GetType()).Equals("UnityEngine.MeshCollider") && movingDown) source.Play();

    }

    public void playSound()
    {
        if(timesPlayed < max)
        {
            source.Play();
            if (!gameObject.name.Equals("Metronomo")) master.registrarGolpe(gameObject.transform.parent.gameObject.name);
            timesPlayed++;
        }
        else
        {
            termino = true;
            CancelInvoke("playSound");

            if (gameObject.name.Equals("Metronomo")) master.playTrack1();
        }
        
    }

    public void playRepeating(float initTime, float repeatTime, int maxTimes)
    {
        max = maxTimes;
        InvokeRepeating("playSound", initTime, repeatTime);
    }

    
    
}
