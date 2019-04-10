using UnityEngine;

public class Metronomo : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int maxStrokes;
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
        }

    }

    public void playRepeating(float initTime, float repeatTime, int maxTimes)
    {
        maxStrokes = maxTimes;
        InvokeRepeating("playSound", initTime, repeatTime);
    }
}
