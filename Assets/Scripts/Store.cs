using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Store : MonoBehaviour {
    public GameObject btnsTienda;
    public GameObject panelCamisas;
    public GameObject panelJoyas;
    public GameObject panelGorros;
    public GameObject btnBuy;
    public GameObject btnPoner;

    public GameObject Chum;

    public int cans;

    public Text text;
    public Text precio;
    public Text aux;
    public Text can;
    private AudioManager audi;
    public GameObject item2Buy;
    public GameObject item2Wear;

    public bool notEnoghtMoney;

    public GameObject[] Shirts;
    public GameObject[] Gems;
    public GameObject[] Caps;
    public bool itemsCargados;
    public  bool first =false;


    private void Start()
    {
        cans = PlayerPrefs.GetInt("cans");

        audi = AudioManager.instance;
        cargarItems();
    }
    private void Update()
    {

        if (first == false)
        {
            audi.PlayMusic("Tienda");
            audi.ChangeFXVol(PlayerPrefs.GetFloat("volumenFX"));
            cargarItems();
            audi.ChangeMusicVol(PlayerPrefs.GetFloat("volumenMusica"));
            first = true;
        }
        can.text = "Latas: " + cans;

        if (panelCamisas.activeSelf) {
            text.text = "Selecciona una de estas camisas";
            itemsCargados = false;
        }

        if (panelJoyas.activeSelf)
        {
            text.text = "Selecciona una de estas joyas";
            itemsCargados = false;

        }
        if (panelGorros.activeSelf)
        {            text.text = "Selecciona una de estos gorros";
            itemsCargados = false;

        }

        if (notEnoghtMoney == true)
        {
            text.text = "¡Caracoles! Ya no hay dinero";
            itemsCargados = false;

        }
        if (btnsTienda.activeSelf)
        {
            if (itemsCargados== false)
            {
                cargarItems();
            }
            precio.text = "";
            text.text = "Bienvenido a Pawn Cats Stars";
            btnBuy.SetActive(false);
            btnPoner.SetActive(false);
            notEnoghtMoney = false;
        }
        else
        {
            btnBuy.SetActive(true);
        }


    }

    public void AbrirCamisas()
    {
        audi.PlaySound("Click");
        btnsTienda.SetActive(false);
        panelCamisas.SetActive(true);
    } 

    public void AbrirJoyas()
    {
        audi.PlaySound("Click");

        btnsTienda.SetActive(false);
        panelJoyas.SetActive(true);

    }


    public void AbrirGorros()
    {
        audi.PlaySound("Click");

        btnsTienda.SetActive(false);
        panelGorros.SetActive(true);

    }

    public void go2MainMenu()
    {
        audi.PlaySound("Click");

        if (btnsTienda.activeSelf == true)
        {
            SceneManager.LoadScene("selecnivel");
        }

        if (panelCamisas.activeSelf || panelGorros.activeSelf || panelJoyas.activeSelf)
        {
        
            panelJoyas.SetActive(false);
            panelCamisas.SetActive(false);
            panelGorros.SetActive(false);
            btnsTienda.SetActive(true);

        }

    }


    public void SeeCamisa(GameObject item)
    {

        audi.PlaySound("Click");
        itemsCargados = false;
        item.GetComponent<SpriteRenderer>().sortingOrder = 2;
        if (PlayerPrefs.GetString(item.name) == "yes")
        {
            item2Wear = item;
            precio.text = "";
            btnBuy.SetActive(false);
            btnPoner.SetActive(true);

        }
        else
        {
            item2Buy = item;
            precio.text = "Precio:" + item.GetComponent<ItemShop>().price;
            btnBuy.SetActive(true);
            btnPoner.SetActive(false);

        }

        for (int i = 0; i < Shirts.Length; i++)
        {
            if (Shirts[i].name != item.name)
            {
                Shirts[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }


    public void SeeGems(GameObject item)
    {
        audi.PlaySound("Click");
        itemsCargados = false;
        item.GetComponent<SpriteRenderer>().sortingOrder = 4;
        if (PlayerPrefs.GetString(item.name) == "yes")
        {
            item2Wear = item;
            precio.text = "";
            btnBuy.SetActive(false);
            btnPoner.SetActive(true);

        }
        else
        {
            item2Buy = item;
            precio.text = "Precio:" + item.GetComponent<ItemShop>().price;
            btnBuy.SetActive(true);
            btnPoner.SetActive(false);

        }

        for (int i = 0; i < Gems.Length; i++)
        {
            if (Gems[i].name != item.name)
            {
                Gems[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }

    public void cargarItems()
    {
      //  print("Deberia de poner: " + PlayerPrefs.GetString("lastCamisa"));
        for (int i = 0; i < Shirts.Length; i++)
        {
            if (Shirts[i].name == PlayerPrefs.GetString("lastCamisa"))
            {
         //       print("Shirts: " +Shirts[i].name);
                Shirts[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
        
            }
            else
            {
                Shirts[i].GetComponent<SpriteRenderer>().sortingOrder =0;
            }
        }

        for (int i = 0; i < Gems.Length; i++)
        { 
            if (Gems[i].name == PlayerPrefs.GetString("lastJoyas"))
            {
                Gems[i].GetComponent<SpriteRenderer>().sortingOrder =4;
             //   print("Joya:  " + Gems[i].name);
                print("Sortin " + Gems[i].GetComponent<SpriteRenderer>().sortingOrder);
				return;
            }
            else
            {
                Gems[i].GetComponent<SpriteRenderer>().sortingOrder =0;
               
            }
        }

        for (int i = 0; i < Caps.Length; i++)
        {
            if (Caps[i].name == PlayerPrefs.GetString("lastGorro"))
            {
                Caps[i].GetComponent<SpriteRenderer>().sortingOrder = 3;
				return;
            }
            else
            {
                Caps[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
        itemsCargados = true;
    }

    public void SeeCaps(GameObject item)
    {
        itemsCargados = false;
        audi.PlaySound("Click");

        item.GetComponent<SpriteRenderer>().sortingOrder = 3;
        if (PlayerPrefs.GetString(item.name) == "yes")
        {
            item2Wear = item;
            precio.text = "";
            btnBuy.SetActive(false);
            btnPoner.SetActive(true);

        }
        else
        {
            item2Buy = item;
            precio.text = "Precio:" + item.GetComponent<ItemShop>().price;
            btnBuy.SetActive(true);
            btnPoner.SetActive(false);

        }

        for (int i = 0; i < Caps.Length; i++)
        {
            if (Caps[i].name != item.name)
            {
                Caps[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }

    public void Buy()
    {
        audi.PlaySound("Click");

        if (item2Buy != null)
        {

            if (item2Buy.GetComponent<ItemShop>().price > cans)
            {
                notEnoghtMoney = true;
            }
            else
            {

                if (PlayerPrefs.GetString(item2Buy.name) == "yes")
                {
                    StartCoroutine(changetext("¡Caracoles! ya compraste esto"));
                    audi.PlaySound("Cant");
                }
                else
                {
                    StartCoroutine(changetext("Buena elección"));
                    PlayerPrefs.SetString(item2Buy.name, "yes");
                    PlayerPrefs.SetString("last" + item2Buy.tag, item2Buy.name);
                    cans -= item2Buy.GetComponent<ItemShop>().price;
                    PlayerPrefs.SetInt("cans", cans);
                }

            }
           
        }
        else
        {
            StartCoroutine(changetext("¡Caracoles! Selecciona algo que se pueda comprar"));
            audi.PlaySound("Cant");
        }
        PlayerPrefs.Save();
        item2Buy = null;
    }

    IEnumerator changetext(string texto)
    {

        Chum.GetComponent<SpriteRenderer>().sortingOrder = -1;
        aux.text = texto;
        yield return new WaitForSeconds(1f);
        Chum.GetComponent<SpriteRenderer>().sortingOrder = -2;

        aux.text = "";
    }
     public void Poner()
    {
        audi.PlaySound("Click");

        if (item2Wear != null)
        {
            if (PlayerPrefs.GetString(item2Wear.name) == "yes")
            {
                StartCoroutine(changetext("¡Caracoles! se ve fabuloso"));
                PlayerPrefs.SetString("last" + item2Wear.tag, item2Wear.name);
            }

            PlayerPrefs.Save();
            item2Wear = null;
        }
        else
        {
            StartCoroutine(changetext("Selecciona algo que se pueda poner :/"));
            audi.PlaySound("Cant");

        }


    }

    public void deleall()
    {
        PlayerPrefs.DeleteAll();
    }

}
