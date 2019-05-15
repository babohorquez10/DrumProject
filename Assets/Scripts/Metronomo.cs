using UnityEngine;

public class Metronomo : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int maxStrokes;
    private MainScript master;
    private int track;
    private float time;
    public int numTotalStrokes;

    public bool termino = false;
    public bool guiaMetronomo;


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

    public void playSound()
    {
        if (timesPlayed < maxStrokes)
        {
            source.Play();
            timesPlayed++;
        }
        else
        {
            termino = true;
            CancelInvoke("playSound");

            track = master.darTrack();

            switch(track)
            {
                case 1:
                    master.playTrack1();
                    break;
                case 2:
                    master.playTrack2();
                    break;
                case 3:
                    master.playTrack3();
                    break;
            }

            if (guiaMetronomo) {
                guiaMetronomo = false;
                timesPlayed = 0;
                maxStrokes = numTotalStrokes;
                InvokeRepeating("playSound", 0f, time);
            }
        }

    }

    public void playRepeating(float initTime, float repeatTime, int maxTimes)
    {
        time = repeatTime;
        maxStrokes = maxTimes;
        InvokeRepeating("playSound", initTime, repeatTime);
    }
}
