using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] Transform surePanel;

    [SerializeField] float startsurePos = 400;
    [SerializeField] float finalsurePos = 0;
    [SerializeField] float sureSpeed = 0.5f;
    [SerializeField] float sureDelay = 0.1f;
    bool sureOpen = false;
    private void Awake()
    {
        surePanel.localPosition = new Vector2(surePanel.localPosition.x, startsurePos);
    }
    public void Exit()
    {
        PlayerController.instance.canvas.SetActive(false);
        Manager.instance.ClickSound();
        VeilLoadLevel.instance.Exit();
    }
    public void ToMenu()
    {
        PlayerController.instance.canvas.SetActive(false);
        Manager.instance.ClickSound();
        VeilLoadLevel.instance.LoadMenu();
    }
    public void OpenPause()
    {
        Manager.instance.ClickSound();
        sureOpen = true;
        surePanel.localPosition = new Vector2(surePanel.localPosition.x, startsurePos);
        surePanel.LeanMoveLocalY(finalsurePos, sureSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = sureDelay;
    }
    public void ClosePause()
    {
        Manager.instance.ClickSound();
        sureOpen = false;
        surePanel.localPosition = new Vector2(surePanel.localPosition.x, finalsurePos);
        surePanel.LeanMoveLocalY(startsurePos, sureSpeed).setIgnoreTimeScale(true).setEaseOutExpo().delay = sureDelay;
    }
}
