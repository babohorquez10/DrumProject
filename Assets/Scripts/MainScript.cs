using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject hiHat;
    public GameObject snare;

    public GameObject marcador;

    private Sound soundHiHat;
    private Sound soundSnare;

    private AudioSource hiHatSource;
    private AudioSource snareSource;

    // Start is called before the first frame update
    void Start()
    {

        Invoke("moverMarcador", 5f);

        hiHatSource = hiHat.transform.Find("Collid").GetComponent<AudioSource>();
        snareSource = snare.transform.Find("Collid").GetComponent<AudioSource>();

        soundHiHat = hiHat.transform.Find("Collid").GetComponent<Sound>();
        soundSnare = snare.transform.Find("Collid").GetComponent<Sound>();

        Invoke("playTrack1", 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void playTrack1()
    {
        soundHiHat.playRepeating(0f, 0.5f);
        soundSnare.playRepeating(1f, 2f);
    }

    private void moverMarcador()
    {
        MoverMarcador scrMarcador = marcador.GetComponent<MoverMarcador>();

        scrMarcador.endMarker = GameObject.Find("Fin").transform;

        scrMarcador.mover();
    }

}
