using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MainScript : MonoBehaviour
{
    public GameObject hiHat;
    public GameObject snare;
    public GameObject metronomo;

    public GameObject marcador;
    public GameObject textBPM;

    public float bpm;

    private ArrayList listaGolpes;
    private ArrayList listaGolpesUsuario;

    private float tempoScns;

    private Sound soundHiHat;
    private Sound soundSnare;
    private Metronomo metronomoScr;

    private AudioSource hiHatSource;
    private AudioSource snareSource;
    private AudioSource metronomoSource;
    private int pickUpItems = 0;

    private Sound[] listaSources;




    // Start is called before the first frame update
    void Start()
    {
        listaSources = new Sound[2];

        listaGolpes = new ArrayList();
        listaGolpesUsuario = new ArrayList();

        tempoScns = 1f / (bpm / 60);

        metronomoSource = metronomo.GetComponent<AudioSource>();

        hiHatSource = hiHat.transform.Find("Collid").GetComponent<AudioSource>();
        snareSource = snare.transform.Find("Collid").GetComponent<AudioSource>();


        metronomoScr = metronomo.GetComponent<Metronomo>();
        soundHiHat = hiHat.transform.Find("Collid").GetComponent<Sound>();
        soundSnare = snare.transform.Find("Collid").GetComponent<Sound>();

        textBPM.GetComponent<TextMesh>().text = bpm.ToString();

        //Invoke("count", 5f);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void sostenerBaqueta()
    {
        pickUpItems++;

        if (pickUpItems == 2) Invoke("count", 5f);
    }

    public void soltarBaqueta()
    {
        pickUpItems--;
        CancelInvoke("count");
    }

    public void playTrack()
    {
        soundHiHat.playRepeating(0f, tempoScns / 2f, 16);
        soundSnare.playRepeating(tempoScns, tempoScns * 2f, 4);

        listaSources = new Sound[2];

        listaSources[0] = soundHiHat;
        listaSources[1] = soundSnare;

        InvokeRepeating("verificarTermino", 0f, 1f);
    }

    private void verificarTermino()
    {

        foreach(Sound sonido in listaSources)
        {
            if (!sonido.termino) return;
        }

        //Recorrer Lista

        string str = "Estimado: ";

        foreach(Golpe golpe in listaGolpes)
        {
            str += golpe.nombreGolpeado + ", T: " + golpe.timestamp + "; ";
        }

        Debug.Log(str);

        str = "Real : ";
        foreach (Golpe golpe in listaGolpesUsuario)
        {
            str += golpe.nombreGolpeado + ", T: " + golpe.timestamp + "; ";
        }

        Debug.Log(str);
        CancelInvoke("verificarTermino");
    }

    private void count()
    {
        listaGolpesUsuario = new ArrayList();
        metronomoScr.playRepeating(0f, tempoScns, 4);
    }

    public void registrarGolpe(string nombre)
    {
        listaGolpes.Add(new Golpe { nombreGolpeado = nombre, timestamp = Time.time * 1000 });
    }

    public void registrarGolpeUsuario(string nombre)
    {
        listaGolpesUsuario.Add(new Golpe { nombreGolpeado = nombre, timestamp = Time.time * 1000 });
    }
}

public class Golpe
{
    public string nombreGolpeado;
    public float timestamp;
}


