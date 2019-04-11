using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int maxStrokes;
    private MainScript master;
    public string nombreElemento;

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

        if (!("" + other.GetType()).Equals("UnityEngine.MeshCollider") && movingDown)
        {
            source.Play();
            master.registrarGolpeUsuario(nombreElemento);
        }

    }

    public void playSound()
    {
        if(timesPlayed < maxStrokes)
        {
            //source.Play();
            master.registrarGolpe(nombreElemento);
            timesPlayed++;
        }
        else
        {
            termino = true;
            CancelInvoke("playSound");
        }
        
    }

    public void playRepeating(float initTime, float repeatTime, int maxTimes)
    {
        maxStrokes = maxTimes;
        InvokeRepeating("playSound", initTime, repeatTime);
    }
}
