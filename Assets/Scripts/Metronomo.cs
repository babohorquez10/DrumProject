using UnityEngine;

public class Metronomo : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int maxStrokes;
    private MainScript master;
    private float time;

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
            master.playTrack();

            if (guiaMetronomo) {
                guiaMetronomo = false;
                timesPlayed = 0;
                maxStrokes = 8;
                InvokeRepeating("playSound", 0f, time);
            }
        }

    }

    private void tocar()
    {
        source.Play();
    }

    public void playRepeating(float initTime, float repeatTime, int maxTimes)
    {
        time = repeatTime;
        maxStrokes = maxTimes;
        InvokeRepeating("playSound", initTime, repeatTime);
    }
}
