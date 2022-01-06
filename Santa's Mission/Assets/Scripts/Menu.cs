using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public static Menu instance;
    [SerializeField]
    TextMeshProUGUI maxScoreText;
    [SerializeField]
    TextMeshProUGUI musicVol;
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    TextMeshProUGUI soundsVol;
    [SerializeField]
    Slider soundsSlider;
    [SerializeField]
    GameObject veil;
    Image veilSprite;
    [SerializeField] float veilOnTrans = 100;
    [SerializeField] float veilOnSpeed = 0.5f;
    [SerializeField]
    Transform settingsPanel;
    [SerializeField] float startSettingsPos = -2000;
    [SerializeField] float finalSettingsPos = 0;
    [SerializeField] float settingsSpeed = 0.5f;
    [SerializeField] float settingsDelay = 0.1f;
    void Awake()
    {
        veilSprite = veil.GetComponent<Image>();
        instance = this;
    }
    private void Start()
    {
        musicSlider.value = Manager.save.musicVolume;
        soundsSlider.value = Manager.save.soundsVolume;
        UpdateTexts();
        SetVolumeMusic();
        SetVolumeSounds();
    }
    public void Play()
    {
        Manager.instance.ClickSound();
        VeilLoadLevel.instance.LoadGame();
    }
    public void OpenSettings()
    {
        Manager.instance.ClickSound();
        veil.SetActive(true);
        LeanTween.value(gameObject, veilSprite.color.a, veilOnTrans, veilOnSpeed).setOnUpdate(
            (float val) => {
                veilSprite.color = new Color(veilSprite.color.r, veilSprite.color.g, veilSprite.color.b, val);
            }
        );
        settingsPanel.LeanMoveLocalY(startSettingsPos, settingsSpeed).setEaseOutExpo().delay = settingsDelay;
    }
    public void CloseSettings()
    {
        Manager.instance.ClickSound();
        LeanTween.value(gameObject, veilSprite.color.a, 0, veilOnSpeed).setOnUpdate(
            (float val) => {
                veilSprite.color = new Color(veilSprite.color.r, veilSprite.color.g, veilSprite.color.b, val);
            }
        ).setOnComplete(() => veil.SetActive(false));
        settingsPanel.LeanMoveLocalY(finalSettingsPos, settingsSpeed).setEaseOutExpo().delay = settingsDelay;
    }
    public void UpdateTexts()
    {
        musicSlider.value = Manager.save.musicVolume;
        musicVol.text = "Music volume: " + Mathf.RoundToInt(Manager.save.musicVolume * 100);
        soundsSlider.value = Manager.save.soundsVolume;
        soundsVol.text = "Sounds volume: " + Mathf.RoundToInt(Manager.save.soundsVolume * 100);
        maxScoreText.text = "Highest score: " + Manager.save.maxScore;
    }
    public void SetVolumeMusic()
    {
        Manager.instance.ClickSound();
        float value = musicSlider.value;
        Manager.instance.SetVolumeMusic(value);
        musicVol.text = "Music volume: " + Mathf.RoundToInt(value * 100);
    }
    public void SetVolumeSounds()
    {
        Manager.instance.ClickSound();
        float value = soundsSlider.value;
        Manager.instance.SetVolumeSounds(value);
        soundsVol.text = "Sounds volume: " + Mathf.RoundToInt(value * 100);
    }
}
