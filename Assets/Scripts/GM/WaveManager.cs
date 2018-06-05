using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
	public string name;
	public Transform[] Enemies;
	public int enemies2Spawn;
	public float timeEnemies;
	public float time;
	public string[] sides;
}



public class WaveManager : MonoBehaviour {

	GameManager GM;

	public float countdown;

	public Wave[] waves;

	public int currentWave;

	public bool starts;
	public WaveState state;

	public Transform[] SpawnPointsLeft;
	public Transform[] SpawnPointsRight;
    public Transform[] SpawnPointsUp;
    public delegate void OnFinished(float tim);
    public delegate void OnLastWave();
	public event OnFinished WavesEnded;
    public event OnLastWave StartLastWave;
	public enum WaveState
	{
		SPAWNING, WAITING, ENDED
	}

	bool enemiesAlive;
	public bool finished;

	public static WaveManager Instance;
	void Awake(){
		Instance = this;
	}

	void Start(){
		GM = GameManager.Instance;
		finished = false;
		state = WaveState.WAITING;
		InvokeRepeating ("EnemiesAlive",0, 0.5f);

	}

	void Update()
	{
		float tim = 0;
		if (starts)
		{
			tim += Time.deltaTime;
			countdown += Time.deltaTime;
		}

        if (currentWave < waves.Length-1) {
            if (countdown > waves[currentWave].time && state == WaveState.WAITING && enemiesAlive == false) {
                //print ("Empieza Wave");
                GM.typeSente("Empieza la oleada # " + (currentWave + 1));
                StartCoroutine(SpawnWave(waves[currentWave]));
            }
        } else if (currentWave == waves.Length-1) {
            if (countdown > waves[currentWave].time && state == WaveState.WAITING && enemiesAlive == false)
            {
                if (StartLastWave != null)
                    StartLastWave();
                //print ("Empieza Wave");
                StartCoroutine(SpawnWave(waves[currentWave]));
                GM.typeSente("Empieza la oleada # " + (currentWave + 1));
            }
        } else if (currentWave > waves.Length && finished == false && enemiesAlive == false) {
			finished = true;
			print ("all waves completed");
			if(WavesEnded != null)
			WavesEnded(tim);
		}



		if(currentWave < currentWave +1 && state == WaveState.ENDED && enemiesAlive == false){
			print ("Wave ended");
			GM.typeSente("Termino la oleada");
			state = WaveState.WAITING;
			countdown = 0;
			currentWave++;
		}

	}

	IEnumerator	SpawnWave(Wave _wave){
		
		state = WaveState.SPAWNING;
		for(int i=0 ; i < _wave.enemies2Spawn; i++){
		yield return new WaitForSecondsRealtime (_wave.timeEnemies);
			int ranSide = Random.Range(0, _wave.sides.Length);
            switch (_wave.sides[ranSide])
            {
                case "right":
                    int ranEnem = Random.Range(0, _wave.Enemies.Length);
                    int ranPos = Random.Range(0, SpawnPointsRight.Length);
                    Transform Dog2Instanciate = Instantiate(_wave.Enemies[ranEnem], SpawnPointsRight[ranPos].position, Quaternion.identity);
                    Dog2Instanciate.transform.parent = SpawnPointsRight[ranPos].parent;
                    break;

                case "left":
                    int ranEnem1 = Random.Range(0, _wave.Enemies.Length);
                    int ranPos1 = Random.Range(0, SpawnPointsLeft.Length);
                    Transform Dog2Instanciate1 = Instantiate(_wave.Enemies[ranEnem1], SpawnPointsLeft[ranPos1].position, Quaternion.identity);
                    Dog2Instanciate1.transform.parent = SpawnPointsLeft[ranPos1].parent;
                    break;

                case "up":
                    int ranEnem2 = Random.Range(0, _wave.Enemies.Length);
                    int ranPos2 = Random.Range(0, SpawnPointsUp.Length);
                    Instantiate(_wave.Enemies[ranEnem2], SpawnPointsUp[ranPos2].position, Quaternion.identity);
                    break;
                default:
                    break;
            }
           	
		}
		state = WaveState.ENDED;
		yield break;
	}

	void EnemiesAlive(){
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length <= 0 ) {
			enemiesAlive = false;
		} else {
			enemiesAlive = true;
		}
	}

	public void Starts()
	{
		starts = true;
	}
}
