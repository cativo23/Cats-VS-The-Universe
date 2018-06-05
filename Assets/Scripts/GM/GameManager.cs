using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class Fila{
	public string nombre;

	public List<GameObject> gatos;
	public List<GameObject> perros;


}




public class GameManager : MonoBehaviour {
	//instance
	public static GameManager Instance;
	public AudioManager AM;
	public WaveManager WM;

	//valores de variables del juego
	public int puntaje;
	public int elixir;
	public int cookies;

	public GameObject[] items;
	public float[] probs;

	//new values
	public Slider sliderelixir;
	public Text sliderText;
	public Text itemTex;
	public Text Info;
	public Text cookiestext;
	bool iscooldown;
	float sumador;
	public int tiempocooldown = 1;
	public int valor = 2;

	public GameObject WinPanel;

	public bool win;

	//References to eventshandlers
	Spawner _Spawner;
	 public bool isTuto;

	//variables
	public Fila fila1;
	public Fila fila2;
	public Fila fila3; 

	//Delegates declarations
	public delegate void OnClick(RaycastHit2D[] hits);
	public delegate void OnReleased();
	public delegate void OnPressed();
	public delegate void OnCatDestroy(GameObject dog);
	public delegate void OnDogDestroyed (GameObject cat);
    public delegate void OnTheWallDestroyed(GameObject wall);
	public delegate void OnGameOver();

	public delegate void OnPause();

	public delegate void OnStart();

	//Events Declarations
	public event OnClick Clicked;
	public event OnPressed Pressed;
	public event OnReleased Released;
	public event OnCatDestroy CatDestroyed;
	public event OnDogDestroyed DogDestroyed;
    public event OnTheWallDestroyed WallDestroyed;
	public event OnGameOver GameOver;
	public event OnGameOver GameOverTuto;
	public event OnPause Pause;

//	public event OnStart Star;

    private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		WM = WaveManager.Instance;
		WM.WavesEnded += Win;
		Info.text = "";
		cookies = PlayerPrefs.GetInt("cans");
		itemTex.text = "" + cookies;
		AM = AudioManager.instance;
		AM.PlayMusic("Music");
		probs = new float[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			probs[i] = items[i].GetComponent<ItemB>().prob;
		}
		sliderelixir.value = elixir;
		sliderText.text = ""+elixir;
		fila1.nombre = "Fila 1";
		fila2.nombre = "Fila 2";
		fila3.nombre = "Fila 3";
		_Spawner = Spawner.Instance;
		_Spawner.CatPlaced += QuitarElixir;
		_Spawner.CatPlaced += AgregarGatos;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))///mandar la info del click
		{
			if(Clicked != null)
			{
				RaycastHit2D[] hits;
				hits = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100);
				Clicked(hits);
			}
		}
		if (Input.GetMouseButton(0))
		{
			if (Pressed != null)
				Pressed();
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (Released != null)
				Released();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
            if(Pause != null)
			    Pause();
		}


		if (iscooldown)//CADA CIERTOS SEGUNDOS SE AGREGA CIERTA CANTIDAD
		{
			sumador += Time.deltaTime;

			if (Mathf.Round(sumador) == tiempocooldown)
			{
				StartCoroutine(AddElixir(valor));
				//sliderelixir.value += valor;//agrego el valor a subir por segundos
				elixir =  (int)sliderelixir.value;
				sumador = 0;
			}
		}

	}

	public void AgregarGatos( GameObject catPlaced, string fila)
	{
		switch (fila)
		{
		case "Fila1":
			fila1.gatos.Add (catPlaced);
				break;
		case "Fila2":
			fila2.gatos.Add (catPlaced);
				break;
		case "Fila3":
			fila3.gatos.Add (catPlaced);
				break;
		}
        
	}

	public void RemoverGatos(GameObject catRemoved , string fila){
		switch (fila) {
		case "Fila1":
			fila1.gatos.Remove (catRemoved);
			break;
		case "Fila2":
			fila2.gatos.Remove (catRemoved);
			break;
		case "Fila3":
			fila3.gatos.Remove(catRemoved);
			break;
		}
        if (CatDestroyed != null)
        {
            CatDestroyed(catRemoved);
        }
    }

	public void AgregarPerros( GameObject dogPlaced, string fila)
	{
		switch (fila)
		{
		case "Fila1":
			fila1.perros.Add (dogPlaced);
			break;
		case "Fila2":
			fila2.perros.Add (dogPlaced);
			break;
		case "Fila3":
			fila3.perros.Add (dogPlaced);
			break;
		}
	}

	public void RemoverPerros(GameObject dogRemoved , string fila){

		switch (fila) {
		case "Fila1":
			fila1.perros.Remove (dogRemoved);
			break;
		case "Fila2":
			fila2.perros.Remove (dogRemoved);
			break;
		case "Fila3":
			fila3.perros.Remove(dogRemoved);
			break;
		}
        if (DogDestroyed != null)
            DogDestroyed(dogRemoved);
    }
	public void QuitarElixir(GameObject catPlaced, string fila)//al parecer se deselecciona el gato, descrequear interactividad
	{
		float nuevo = catPlaced.GetComponent<CatStats>().exilir.GetValue();
		if (sliderelixir.value < nuevo)
		{
			iscooldown = false;
			sliderelixir.value = 0f;
			sliderText.text = "" + sliderelixir.value;
			
		}
		else if (sliderelixir.value >= 0 && sliderelixir.value <= 100)
		{
			sliderelixir.value -= nuevo;
			sliderText.text = "" + sliderelixir.value;
			iscooldown = true;
			//sliderelixir.value += restaurarelixir(1,10);//HACER UNA CORRUTINA QUE SE EJECUTE CADA TANTOS SEGUNDOS
		}
		else if (sliderelixir.value > 100)
		{
			sliderelixir.value = 100;
			sliderText.text = "" + sliderelixir.value;
			iscooldown = false;
		}
		elixir = (int)sliderelixir.value;
	}

    public void DestroyWall(GameObject _wall)
    {
        if(WallDestroyed != null)
        WallDestroyed(_wall);
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			if(GameOver != null && !isTuto)
			{
				GameOver();
			}
			if (GameOverTuto != null && isTuto)
			{ GameOverTuto(); }
		}
	}

	public IEnumerator AddElixir(int value)
	{
		for (int i = 0; i < value; i++)
		{
			sliderelixir.value += 1;
			sliderText.text = ""+ sliderelixir.value;
			yield return 0;
		}
	}

	public void ItemIsDestroyed( int _quantity)
	{
		if (gameObject != null)
		{
			StartCoroutine(ChangeItem(_quantity));

		}
	}

	public IEnumerator ChangeItem( int cantidad)
	{
		for (int i = 0; i < cantidad; i++)
		{
			cookies += 1;
			itemTex.text = "" + cookies;
			yield return 0;
		}
	}

	public void DropItem(Transform pos)
	{
		float total = 0;
		int toSpawn =0;
        for (int i = 0; i < probs.Length; i++)
        {
            total += probs[i];
        }
		
		float randomPoint = Random.value * total;
		
		for (int i = 0; i < probs.Length; i++)
		{
			if (randomPoint < probs[i])
			{
				toSpawn= i;
				Instantiate(items[toSpawn], pos.position, Quaternion.identity);
				return;
			}
			else
			{
				randomPoint -= probs[i];
			}
		}
	}

	public void Paus()
	{
		if(Pause != null)
		Pause();
	}

	public void WallIsDamaged()
	{
		string sentence = "Un muro esta a punto de caer";
		StartCoroutine(TypeSentence(sentence));

	}

	public void typeSente(string sent)
	{
		StartCoroutine(TypeSentence(sent));
	}
	IEnumerator TypeSentence(string _sentence)
	{
		Info.text = "";
		foreach (char letter in _sentence.ToCharArray())
		{
			Info.text += letter;
			yield return 0;
		}
		yield return new WaitForSeconds(2f);

		Info.text = "";
	}

	public void Win(float time)
	{
        WinPanel.SetActive(true);
        if (!isTuto)
		{
			StartCoroutine(TypeSentenceF(cookies - PlayerPrefs.GetInt("cans")));
		}
	}
		
	IEnumerator TypeSentenceF(int cant )
	{
		int temo = PlayerPrefs.GetInt("cans");
		for (int i = 0; i < cant; i++)
		{
			temo += 1;
			cookiestext.text = "" + temo;
			yield return new WaitForSeconds(0.00001f);
		}
		PlayerPrefs.SetInt("cans", cookies);
	}

    public void StartLastStage()
    {

    }

}
