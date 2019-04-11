using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    private int timesPlayed = 0;
    private int maxStrokes;
    private MainScript master;

    private Velocidad scrDerecha;
    private Velocidad scrIzquierda;

    public string nombreElemento;

    public bool termino = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        master = GameObject.Find("Master").GetComponent<MainScript>();

        scrDerecha = GameObject.Find("DKFYB_Drumstick_Der").GetComponent<Velocidad>();
        scrIzquierda = GameObject.Find("DKFYB_Drumstick_Izq").GetComponent<Velocidad>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        bool movingDown = other.gameObject.name.Equals("DKFYB_Drumstick_Der") ? scrDerecha.isMovingDown() : scrIzquierda.isMovingDown();

       //bool movingDown = other.gameObject.GetComponent<Velocidad>().isMovingDown();
        
        if (!("" + other.GetType()).Equals("UnityEngine.MeshCollider") && movingDown)
        {
            master.registrarGolpeUsuario(nombreElemento);
            source.Play();
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
