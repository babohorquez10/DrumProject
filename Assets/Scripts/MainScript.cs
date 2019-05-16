using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MainScript : MonoBehaviour
{
    public GameObject hiHat;
    public GameObject snare;
    public GameObject snare2;
    public GameObject snare3;
    public GameObject snare4;
    public GameObject metronomo;

    public GameObject marcador;
    public GameObject textBPM;

    public float bpm;

    public float margenError;

    public int numeroTrack;

    private ArrayList listaGolpes;
    private ArrayList listaGolpesUsuario;

    private float tempoScns;

    private Sound soundHiHat;
    private Sound soundSnare;
    private Sound soundSnare2;
    private Sound soundSnare3;
    private Sound soundSnare4;
    private Metronomo metronomoScr;

    private AudioSource hiHatSource;
    private AudioSource snareSource;
    private AudioSource snareSource2;
    private AudioSource snareSource3;
    private AudioSource snareSource4;
    private AudioSource metronomoSource;
    private int pickUpItems = 0;

    private Sound[] listaSources;
    private Hand mano;
    private bool termino = false;

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
        snareSource2 = snare2.transform.Find("Collid").GetComponent<AudioSource>();
        snareSource3 = snare3.transform.Find("Collid").GetComponent<AudioSource>();
        snareSource4 = snare4.transform.Find("Collid").GetComponent<AudioSource>();


        metronomoScr = metronomo.GetComponent<Metronomo>();
        soundHiHat = hiHat.transform.Find("Collid").GetComponent<Sound>();
        soundSnare = snare.transform.Find("Collid").GetComponent<Sound>();
        soundSnare2 = snare2.transform.Find("Collid").GetComponent<Sound>();
        soundSnare3 = snare3.transform.Find("Collid").GetComponent<Sound>();
        soundSnare4 = snare4.transform.Find("Collid").GetComponent<Sound>();

        textBPM.GetComponent<TextMesh>().text = bpm.ToString();

        mano = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand").GetComponent<Hand>();

        //Invoke("count", 3f);
        //InvokeRepeating("velocidad", 0f, 0.2f);
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

    private void count()
    {
        listaGolpesUsuario = new ArrayList();

        metronomoScr.numTotalStrokes = 8;
        metronomoScr.playRepeating(0f, tempoScns, 4);

        /*
        metronomoScr.numTotalStrokes = 16;
        metronomoScr.playRepeating(0f, tempoScns, 4);
        */
    }

    public void playTrack1()
    {
        soundHiHat.playRepeatingSound(0f, tempoScns / 2f, 16);
        soundSnare.playRepeatingSound(tempoScns, tempoScns * 2f, 4);

        /*
        soundHiHat.playRepeating(0f, tempoScns / 2f, 32);
        soundSnare.playRepeating(tempoScns, tempoScns * 2f, 8);
        */

        listaSources = new Sound[2];

        listaSources[0] = soundHiHat;
        listaSources[1] = soundSnare;

        InvokeRepeating("verificarTermino", 0f, 3f);
    }

    public void playTrack2()
    {
        soundHiHat.playRepeatingSound(0f, tempoScns / 2f, 16);
        soundSnare.playRepeatingSound(tempoScns / 2f, tempoScns * 2f, 4);
        soundSnare2.playRepeatingSound(tempoScns, tempoScns * 4f, 2);
        soundSnare3.playRepeatingSound(tempoScns * 1.75f, tempoScns * 4f, 2);
        soundSnare4.playRepeatingSound(tempoScns * 3.25f, tempoScns * 4f, 2);

        listaSources = new Sound[1];

        listaSources[0] = soundHiHat;

        InvokeRepeating("verificarTermino", 0f, 3f);
    }

    public void playTrack3()
    {
        soundHiHat.playRepeatingSound(0f, tempoScns / 2f, 16);

        soundSnare.playRepeatingSound(tempoScns, tempoScns * 2f, 4);
        soundSnare2.playRepeatingSound(tempoScns * 1.75f, tempoScns * 4f, 2);
        soundSnare3.playRepeatingSound(tempoScns * 2.25f, tempoScns * 4f, 2);

        listaSources = new Sound[1];

        listaSources[0] = soundHiHat;

        InvokeRepeating("verificarTermino", 0f, 3f);
    }

    private void verificarTermino()
    {

        foreach (Sound sonido in listaSources)
        {
            if (!sonido.termino) return;
        }

        CancelInvoke("verificarTermino");

        termino = true;
        validarResultados();

    }

    private ArrayList validarGolpes()
    {
        ArrayList errores = new ArrayList();

        foreach (Golpe golpe in listaGolpes)
        {
            bool correctoActual = buscarGolpeCorrecto(golpe);

            if (!correctoActual) errores.Add(golpe);
        }

        return errores;
    }

    private bool buscarGolpeCorrecto(Golpe golpeCorrecto)
    {
        float tiempoCorrecto = golpeCorrecto.timestamp;
        string nombreCorrecto = golpeCorrecto.nombreGolpeado;

        foreach (Golpe golpe in listaGolpesUsuario)
        {
            float tiempoActual = golpe.timestamp;

            float dif = Mathf.Abs(tiempoCorrecto - tiempoActual);

            if (golpe.nombreGolpeado.Equals(nombreCorrecto) && dif <= margenError) return true;
        }

        return false;
    }

    public void registrarGolpe(string nombre)
    {
        listaGolpes.Add(new Golpe { nombreGolpeado = nombre, timestamp = Time.time * 1000 });
    }

    public void registrarGolpeUsuario(string nombre)
    {
        if(!termino) listaGolpesUsuario.Add(new Golpe { nombreGolpeado = nombre, timestamp = Time.time * 1000 });
    }

    public int darTrack()
    {
        return numeroTrack;
    }

    private void validarResultados()
    {

        GolpeGuardar nuevoGolpe = null;

        Archivo arch = new Archivo
        {
            beatsPorMinuto = bpm,
            numeroPartitura = numeroTrack,
            golpes = new GolpeGuardar[listaGolpes.Count]
        };

        for(int i = 0; i < listaGolpes.Count; i++)
        { 
            Golpe golpeActual = (Golpe) listaGolpes[i];

            nuevoGolpe = new GolpeGuardar { nombreElemento = golpeActual.nombreGolpeado, tiempo = golpeActual.timestamp };
            nuevoGolpe.desfase = buscarCoincidenciaGolpe(golpeActual);

            arch.golpes[i] = nuevoGolpe;
        }

        string json = JsonUtility.ToJson(arch);

        DateTime fecha = new DateTime();

        //string filePath = "./Assets/" +     fecha.Year + "-" + fecha.Month + "-" + fecha.Day + "-" +             fecha.Hour + "_" + fecha.Minute + "" + fecha.Second + ".json";
        string filePath = "./Assets/Tester.json";

        File.WriteAllText(filePath, json);
    }

    private float buscarCoincidenciaGolpe(Golpe golpeBuscado)
    {
        float menorDiferencia = float.PositiveInfinity;

        foreach (Golpe golpeActual in listaGolpesUsuario)
        {
            float difActual = Mathf.Abs(golpeBuscado.timestamp - golpeActual.timestamp);

            if (golpeActual.nombreGolpeado.Equals(golpeBuscado.nombreGolpeado) && difActual < menorDiferencia) menorDiferencia = difActual;
        }

        if(menorDiferencia == float.PositiveInfinity) return golpeBuscado.timestamp;
        
        return menorDiferencia;
    }

    [Serializable]
    public class Archivo
    {
        public float beatsPorMinuto;
        public int numeroPartitura;
        public GolpeGuardar[] golpes;
    }

    [Serializable]
    public class GolpeGuardar
    {
        public string nombreElemento;
        public float tiempo;
        public float desfase;
    }
}

public class Golpe
{
    public string nombreGolpeado;
    public float timestamp;
}


