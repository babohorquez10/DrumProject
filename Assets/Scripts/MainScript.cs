using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject hiHat;
    public GameObject snare;
    public GameObject metronomo;

    public GameObject marcador;
    public GameObject textBPM;

    public float bpm;

    private float tempoScns;

    private Sound soundHiHat;
    private Sound soundSnare;
    private Sound soundMetronomo;

    private AudioSource hiHatSource;
    private AudioSource snareSource;
    private AudioSource metronomoSource;

    // Start is called before the first frame update
    void Start()
    {
        //Invoke("moverMarcador", 5f);

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
        if(soundMetronomo.termino)
        {
            
            soundMetronomo.termino = true;
        }
    }

    public void playTrack1()
    {
        soundHiHat.playRepeating(0f, tempoScns / 2f, 16);
        soundSnare.playRepeating(tempoScns, tempoScns * 2f, 4);
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
}
