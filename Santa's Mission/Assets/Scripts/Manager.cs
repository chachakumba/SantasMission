using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Audio;


public class Manager : MonoBehaviour
{
    public static Manager instance;
    public static SaveData save = new SaveData();
    [SerializeField]
    AudioMixer masterVol;
    [SerializeField] GameObject soundObj;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource musicSource;
    public List<AudioClip> musics;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        LoadSave();
    }
    private void Start()
    {
        Menu.instance.UpdateTexts();
        StartMusic();
    }
    void StartMusic()
    {
        int rand = UnityEngine.Random.Range(0, musics.Count);
        musicSource.clip = musics[rand];
        musicSource.Play();
        StartCoroutine(MusicChecker(musics[rand].length));
    }
    IEnumerator MusicChecker(float time)
    {
        yield return new WaitForSeconds(time);
    }
    public static void LoadSave()
    {
        if (
        File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            save = (SaveData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogError("There is no save data!");
            save = new SaveData();
        }
    }
    public void SetVolumeMusic(float value)
    {
        if (value <= 0) value = 0.0001f;
        masterVol.SetFloat("musicVol", Mathf.Log10(value) * 20);

        if (save == null)
        {
            LoadSave();
        }
        save.musicVolume = value;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Sounds saved");
    }
    public void SetVolumeSounds(float value)
    {
        if (value <= 0) value = 0.0001f;
        masterVol.SetFloat("soundsVol", Mathf.Log10(value) * 20);


        if (save == null)
        {
            LoadSave();
        }
        save.soundsVolume = value;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Sounds saved");
    }
    public static void Save(int res)
    {
        if (res > save.maxScore)
        {
            save.maxScore = res;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
            bf.Serialize(file, save);
            file.Close();
            Debug.Log("Saved");
        }
    }
    public void ClickSound()
    {
        PlaySound(Vector3.zero, buttonClickClip);
    }
    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        AudioSource source = Instantiate(soundObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroySound(source));
    }
    public void PlaySound(Vector3 pos, AudioClip clip, float timeToPlay)
    {
        StartCoroutine(PlaySoundCour(pos, clip, timeToPlay));
    }
    IEnumerator PlaySoundCour(Vector3 pos, AudioClip clip, float timeToPlay)
    {
        yield return new WaitForSeconds(timeToPlay);
        AudioSource source = Instantiate(soundObj, pos, Quaternion.identity).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroySound(source));
    }
    IEnumerator DestroySound(AudioSource source)
    {
        yield return new WaitUntil(() => source == null || !source.isPlaying);

        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }
}
[System.Serializable]
public class SaveData
{
    public int maxScore = 0;
    [Header("Settings save")]
    public float masterVolume = 1;
    public float soundsVolume = 1;
    public float musicVolume = 1;
}
