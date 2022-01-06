using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        /*
        SceneManager.UnloadSceneAsync(0);
        AsyncOperation load = SceneManager.LoadSceneAsync(1);*/
        load.completed += (ctx)=> SceneLoad(true);
    }
    public void LoadMenuVeil()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        /*
        SceneManager.UnloadSceneAsync(1);
        AsyncOperation load = SceneManager.LoadSceneAsync(0);*/
        load.completed += (ctx) => SceneLoad(false);
    }
    void SceneLoad(bool isToGame)
    {
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
}
