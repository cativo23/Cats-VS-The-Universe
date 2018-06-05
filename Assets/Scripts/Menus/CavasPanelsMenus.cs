using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CavasPanelsMenus : MonoBehaviour {

    GameObject Canvas, PLevels, POptions, PCredits, PPause, PPlay, PPortada;
    float startTime, offset, speedcredits=-0.03f;
    private Renderer rend;
    public GameObject QuadCredits;
    private AudioManager audi;

    public Slider Slider1;
    public Slider Slider2;
    // Use this for initialization
    void Start ()
    {
        audi = AudioManager.instance;
        Time.timeScale = 1;
        rend = QuadCredits.GetComponent<Renderer>();//PARA LOS QUAD
        Canvas = GameObject.Find("Canvas");//SI AGREGO OTRO HIJO DENTRO DEL CANVAS, CAMBIARAN LOS INDICES
        POptions = Canvas.transform.GetChild(0).gameObject;
        PLevels = Canvas.transform.GetChild(1).gameObject;
        PPlay = Canvas.transform.GetChild(2).gameObject;
        PPause = Canvas.transform.GetChild(3).gameObject;
        PCredits = Canvas.transform.GetChild(4).gameObject;
        PPortada = Canvas.transform.GetChild(5).gameObject;
        //PANEL FIRST (PORTADA)
        if (PPortada.activeSelf == false)//Si esta desactivada... activarla
        {
            audi.PlayMusic("Main");
            PPortada.SetActive(true);
            DesactivPanels();
        }
        else//Sino... desactivar todas las demas
        {
            DesactivPanels();
        }
        if (PlayerPrefs.GetFloat("volumenMusica") == 0 && PlayerPrefs.GetFloat("volumenFX") == 0) {
            PlayerPrefs.SetFloat("volumenMusica", 1);
            PlayerPrefs.SetFloat("volumenFX", 1);
        }

        Slider1.value = PlayerPrefs.GetFloat("volumenMusica");
        Slider2.value=PlayerPrefs.GetFloat("volumenFX");

    }
	
	// Update is called once per frame
	void Update ()
    {
        //DSPS DE UN DELTA TIME DE 3 SEG O PRESS STAR IR A PANEL PLAY
        if(PPortada.activeSelf==true)
        {
            if(Input.anyKey || Input.touchCount > 0)//Al precionar ESPACIO ir al panel principal
            { PressStart(); }            
        }
        else if (PCredits.activeSelf == true)
        {
            offset = (Time.time - startTime) * speedcredits;
            rend.material.SetTextureOffset("_MainTex", new Vector2(0, offset));//MainTex: nombre por defecto del parametro
        }

    }
    void DesactivPanels()
    {
        POptions.SetActive(false);
        PLevels.SetActive(false);
        PPlay.SetActive(false);
        PPause.SetActive(false);
        PCredits.SetActive(false);
    }
    void PressStart()
    {
        PPortada.SetActive(false);
        PPlay.SetActive(true);
    }
    //------------------PANEL MAIN---------------------
    public void ButtonPlay()//Button Play
    {
        PPlay.SetActive(false);
        audi.PlaySound("Click");
        PLevels.SetActive(true);
    }
    public void ButtonOption()//Button Options
    {
        PPlay.SetActive(false);
        audi.PlaySound("Click");
        POptions.SetActive(true);
    }
    public void ButtonCredits()//Button Credits
    {
        PPlay.SetActive(false);
        audi.PlaySound("Click");
        PCredits.SetActive(true);
        startTime = Time.time;//Reiniciar el tiempo del Quad
    }
    //------------------PANEL Select Level---------------------
    public void ButtonLevel1()
    {
        
            GetComponent<LoadingBar>().LoadLevel("Main");
    }
    //------------------PANEL OPTIONS Y PAUSE---------------------
    //Barras de sonido
    //Button Salir y Reanudar-PAUSE
    //------------------PANEL CREDITS---------------------
    //Quad
    //------------------Butons Back---------------------
    public void ButtonBack()
    {
        audi.PlaySound("Click");
        if (POptions.activeSelf)
        { POptions.SetActive(false);
            PlayerPrefs.SetFloat("volumenMusica", Slider1.value);
            PlayerPrefs.SetFloat("volumenFX", Slider2.value);
        }
        else if (PCredits.activeSelf)
        { PCredits.SetActive(false); }
        else if (PLevels.activeSelf)
        { PLevels.SetActive(false); }        
        else if(PPause.activeSelf)//Para Boton Menu Principal
        { PPause.SetActive(false); }

        PPlay.SetActive(true);
    }
        
    public void BtnStore()
    {
        audi.PlaySound("Click");
        SceneManager.LoadScene("Tienda");
    }
}
