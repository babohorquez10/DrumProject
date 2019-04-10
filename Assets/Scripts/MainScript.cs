using System;
using System.Collections;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject hiHat;
    public GameObject snare;
    public GameObject metronomo;

    public GameObject marcador;
    public GameObject textBPM;

    public float bpm;

    private ArrayList listaGolpes;

    private float tempoScns;

    private Sound soundHiHat;
    private Sound soundSnare;
    private Sound soundMetronomo;

    private AudioSource hiHatSource;
    private AudioSource snareSource;
    private AudioSource metronomoSource;

    private Sound[] listaSources;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("moverMarcador", 5f);

        listaSources = new Sound[2];

        listaGolpes = new ArrayList();

        tempoScns = 1f / (bpm/60);

        metronomoSource = metronomo.GetComponent<AudioSource>();

        hiHatSource = hiHat.transform.Find("Collid").GetComponent<AudioSource>();
        snareSource = snare.transform.Find("Collid").GetComponent<AudioSource>();


        soundMetronomo = metronomo.GetComponent<Sound>();
        soundHiHat = hiHat.transform.Find("Collid").GetComponent<Sound>();
        soundSnare = snare.transform.Find("Collid").GetComponent<Sound>();

        textBPM.GetComponent<TextMesh>().text = bpm.ToString();

        Invoke("count", 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playTrack1()
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

        string l = "Todos: ";

        foreach(Golpe golpe in listaGolpes)
        {
            l += golpe.nombreGolpeado + ", T: " + golpe.timestamp + "; ";
        }

        Debug.Log(l);
        CancelInvoke("verificarTermino");
    }

    private void moverMarcador()
    {
        MoverMarcador scrMarcador = marcador.GetComponent<MoverMarcador>();

        scrMarcador.endMarker = GameObject.Find("Fin").transform;

        scrMarcador.mover();
    }

    private void count()
    {
        soundMetronomo.playRepeating(0f, tempoScns, 4);
    }

    public void registrarGolpe(string nombre)
    {
        listaGolpes.Add(new Golpe { nombreGolpeado = nombre, timestamp = Time.time * 1000 });
    }
}

public class Golpe
{
    public string nombreGolpeado;
    public float timestamp;
}


