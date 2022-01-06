using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuLoader : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene(1);
    }
}
