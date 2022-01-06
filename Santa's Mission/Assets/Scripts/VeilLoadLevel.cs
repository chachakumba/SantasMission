using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class VeilLoadLevel : MonoBehaviour
{
    public static VeilLoadLevel instance;
    Animator anim;
    [SerializeField] string veilShowUpName;
    [SerializeField] string veilShowDownName;
    [SerializeField] string veilHideUpName;
    [SerializeField] string veilHideDownName;
    [SerializeField] string veilExit;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        anim = GetComponent<Animator>();
        anim.Play(veilHideDownName);
        Advertisement.Initialize("4547037");
    }
    public void LoadGame()
    {
        anim.Play(veilShowDownName);
    }
    public void LoadMenu()
    {
        anim.Play(veilShowUpName);
    }
    public void LoadGameVeil()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        load.completed += (ctx)=> SceneLoad(true);
    }
    public void LoadMenuVeil()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        load.completed += (ctx) => SceneLoad(false);
    }
    void SceneLoad(bool isToGame)
    {
        ShowAd();
        if (isToGame)
        {
            anim.Play(veilHideUpName);
        }
        else
        {
            anim.Play(veilHideDownName);
        }
    }
    public void Exit()
    {
        anim.Play(veilExit);
    }
    public void Quit()
    {
        Application.Quit();
    }
    void ShowAd()
    {
        if (Advertisement.IsReady("Interstitial_Android"))
        {
            Advertisement.Show("Interstitial_Android");
            Debug.LogWarning("ShowingAd");
        }
        Debug.LogWarning("Not ShowingAd");
    }
}
