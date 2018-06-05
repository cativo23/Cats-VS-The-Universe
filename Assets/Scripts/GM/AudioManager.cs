using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.5f, 1.5f)]
    public float pitch;

    [Range(0f, 1f)]
    public float randomVolumen = 0.1f;
    [Range(0f, 1f)]
    public float randomPitch= 0.1f;
    public bool loop;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
		source.playOnAwake = false;
    }

    public AudioSource GetSource()
    {
        return source;
    }
    public void Play()
    {
       source.volume = volume * (1+ Random.Range(-randomVolumen /2f, randomVolumen / 2f));
       source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;


    [SerializeField]
    private Sound[] soundsFX;


    [SerializeField]
    private Sound[] music;
    // Use this for initialization


    private void Awake()
    {
        if(instance!=null)
        {
            print("more than one audiomanager");
        }
        else
        {
            instance = this;
        }
    }

    void OnEnable() {
        for (int i = 0; i < soundsFX.Length; i++)
        {
            GameObject _go = new GameObject("Sound_"+i+"_"+soundsFX[i].name);
            _go.transform.SetParent(this.transform);
            soundsFX[i].SetSource(_go.AddComponent<AudioSource>());
        }

        for (int i = 0; i < music.Length; i++)
        {
            GameObject _go = new GameObject("Music_" + i + "_" + music[i].name);
            _go.transform.SetParent(this.transform);
			//_go.AddComponent<Visualize>();
            music[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }
	
public void PlaySound(string _name)
    {
        for (int i = 0; i < soundsFX.Length; i++)
        {
            if(soundsFX[i].name == _name)
            {
                soundsFX[i].Play();
                return;
            }
        }


    }

    public void StopSound(string _name)
    {

        if (_name == "all")
        {
            for (int i = 0; i < soundsFX.Length; i++)
            {
                soundsFX[i].Stop();
            }
        }
        else
        {
            for (int i = 0; i < soundsFX.Length; i++)
            {
                if (soundsFX[i].name == _name)
                {
                    soundsFX[i].Stop();
                    return;
                }
            }
        }
    }

    public void PlayMusic(string _name)
    {
		
        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == _name)
            {
				music[i].Play();
                return;
            }
        }


    }

    public void StopMusic(string _name)
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == _name)
            {
                music[i].Stop();
                return;
            }
        }
    }


    public void ChangeFXVol(float vol) {

        for (int i = 0; i < soundsFX.Length; i++)
        {
            soundsFX[i].volume = vol;
        }
    }

    public void ChangeMusicVol(float vol) {
        for (int i = 0; i < music.Length; i++)
        {
            music[i].GetSource().volume = vol;
        }
    }

}
