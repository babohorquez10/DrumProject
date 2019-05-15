﻿using System;
using System.Collections;
using System.IO;
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

    public float margenError;

    public int numeroTrack;

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


        metronomoScr = metronomo.GetComponent<Metronomo>();
        soundHiHat = hiHat.transform.Find("Collid").GetComponent<Sound>();
        soundSnare = snare.transform.Find("Collid").GetComponent<Sound>();

        textBPM.GetComponent<TextMesh>().text = bpm.ToString();

        mano = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("RightHand").GetComponent<Hand>();

        //Invoke("count", 5f);
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
        GameObject b = GameObject.Instantiate(hiHat);
        Sound b2 = b.transform.Find("Collid").GetComponent<Sound>();

        soundHiHat.playRepeatingSound(0f, tempoScns, 4);
        b2.playRepeatingSound(0f, tempoScns / 2f, 16);


        listaSources = new Sound[2];
        listaSources[0] = soundHiHat;
        listaSources[1] = b2;
        InvokeRepeating("verificarTermino", 0f, 3f);
    }

    public void playTrack3()
    {

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

        //Recorrer Lista

        /*
        string str = "Estimado: ";

        foreach(Golpe golpe in listaGolpes)
        {
            //if(!golpe.nombreGolpeado.Contains("Hi"))
            str += golpe.nombreGolpeado + ": " + golpe.timestamp + "; ";
        }

        Debug.Log(str);

        str = "Real : ";
        foreach (Golpe golpe in listaGolpesUsuario)
        {
            str += golpe.nombreGolpeado + ": " + golpe.timestamp + "; ";
        }

        Debug.Log(str);
        
        ArrayList errores = validarGolpes();

        str = "Errores : ";
        foreach (Golpe golpe in errores)
        {
            str += golpe.nombreGolpeado + ": " + golpe.timestamp + "; ";
        }

        Debug.Log(str);
        */

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


