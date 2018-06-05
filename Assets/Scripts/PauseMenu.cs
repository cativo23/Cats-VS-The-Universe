
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour {
	public static PauseMenu Instance;

    public GameObject CatsBtn;
    public GameObject Joy;

	public GameManager GM;
    public GameObject pauseUI;
    public Slider Slider1;
    public Slider Slider2;

    WaveManager WM;

	public Animator ani;

    public bool pause;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
    {
        Joy.SetActive(false);
        WM = WaveManager.Instance;
        WM.StartLastWave += DisableCatBtn;
		GM = GameManager.Instance;
		GM.Pause += EnBoolAnimator;
		
		Slider1.value = PlayerPrefs.GetFloat("volumenMusica");
        Slider2.value = PlayerPrefs.GetFloat("volumenFX");
    }

    public void DisableCatBtn()
    {
        Destroy(CatsBtn);
        Joy.SetActive(true);
    }


	public void DisBoolAnimator()
	{
        //Resume();
        Time.timeScale = 1; 
        ani.SetBool("isIn", false);
    }
    public void EnBoolAnimator()
	{
        pauseUI.SetActive(true);
        pause = !pause;
        ani.SetBool("isIn", true);
	}

	public void Resume()
	{
        pause = !pause;
        pauseUI.SetActive(pause);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            pause = !pause;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("selecnivel", LoadSceneMode.Single);
    }

    public void Quit()
   {
        Application.Quit();
    }


	public void Pause()
	{
		Time.timeScale = 0;
	}

	public void Go2FirstLvl()
	{
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}
}
