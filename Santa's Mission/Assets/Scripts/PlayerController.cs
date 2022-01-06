using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public int score = 0;
    public static PlayerController instance;
    [SerializeField] AreaEffector2D windZone;
    [SerializeField] HingeJoint2D presentJoint;
    [SerializeField] Transform windVisual;
    public float mainWindForce = 5;
    float forward = -1;
    public float minXSpawn;
    public float maxXSpawn;
    public float minYSpawn;
    public float maxYSpawn;
    [SerializeField] GameObject targetObj;
    bool missed = false;
    [Space]
    [Header("Cheers")]
    [SerializeField] GameObject cheerWinds;
    [SerializeField] GameObject santaCheerOff;
    [SerializeField] GameObject downWinds;
    public float cheerTime = 1;
    public float rechargeTime = 0.5f;
    GameObject target;
    [SerializeField] GameObject present;
    [SerializeField] Transform presentSpawn;
    Transform presentTransform;
    Vector3 presentStartLocalPos;
    Rigidbody2D presentRigidbody;
    public bool isPlaying = true;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject destroyParticles;
    [SerializeField] GameObject spawnParticles;
    public float timeToDestroyParticles = 3;
    [SerializeField] List<AudioClip> cheersAud = new List<AudioClip>();
    [SerializeField] List<AudioClip> missesAud = new List<AudioClip>();
    [SerializeField] List<AudioClip> bellsAud = new List<AudioClip>();
    [SerializeField] AudioSource audSourceVoice;
    [SerializeField] GameObject audioSpawn;
    [SerializeField] AudioClip windAud;
    [SerializeField] List<Sprite> presentSprites = new List<Sprite>();
    SpriteRenderer presentSprite;
    [SerializeField] GameObject forceText;
    [SerializeField] Transform camera;
    Transform initialCamPos;
    public float shakeCamDurationImpulse = 0.1f;
    public float shakeCamDuration = 1;
    public float shakeCamPower = 1;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] List<GameObject> buildings;
    [SerializeField] GameObject buildingsPref;
    [SerializeField] Transform buildingsSpawn;
    [SerializeField] float buildingsXSpawnOffset = 10;
    [SerializeField] float buildingsXWidth = 10;
    [SerializeField] float buildingsXSpeed = 5;
    [SerializeField] float buildingsMoveDuration = 1;
    public GameObject canvas;
    private void Awake()
    {
        instance = this;        
        UpdateScoreText();
        presentStartLocalPos = present.transform.localPosition;
        presentRigidbody = present.GetComponent<Rigidbody2D>();
        presentTransform = present.transform;
        presentSprite = present.GetComponent<SpriteRenderer>();
        initialCamPos = camera;
        SpawnNewTarget();
        buildings.Add(Instantiate(buildingsPref, buildingsSpawn));
        SpawnBuildings();
    }
    public void ChangeWindDirection()
    {
        forceText.SetActive(false);
        PlayAud(windAud);
        forward = -forward;
        windZone.forceMagnitude = mainWindForce * forward;
        //windVisual.localScale = new Vector3(Mathf.Abs(windVisual.localScale.x) * forward, windVisual.localScale.y, windVisual.localScale.z);
        windVisual.localRotation = new Quaternion(0, 0, 90 + 90 * forward, 0);
    }
    public void ThrowPresent()
    {
        //presentJoint.breakForce = 0;
        //presentJoint.connectedBody = null;
        presentJoint.enabled = false;
        buttons.SetActive(false);
    }
    public void Hit()
    {
        score++;
        Cheer();
        isPlaying = false;
    }
    public void Miss()
    {
        Manager.Save(score);
        score = 0;
        UpdateScoreText();
        audSourceVoice.clip = missesAud.GetRandAudio();
        audSourceVoice.Play();
        target.GetComponentInChildren<Target>().enabled = false;
        ResetPos();
        isPlaying = true;
    }
    async void Cheer()
    {
        UpdateScoreText();
        audSourceVoice.clip = cheersAud.GetRandAudio();
        audSourceVoice.Play();
        target.GetComponentInChildren<Target>().enabled = false;

        cheerWinds.SetActive(true);
        santaCheerOff.SetActive(false);
        await Task.Delay(Mathf.RoundToInt(cheerTime*1000));
        ResetPos();
    }
    async void ResetPos()
    {
        ShakeCam();
        cheerWinds.SetActive(false);
        downWinds.SetActive(true);

        foreach(GameObject building in buildings)
        {
            building.LeanMoveX(building.transform.position.x - buildingsXSpeed, buildingsMoveDuration);
        }
        LeanTween.delayedCall(buildingsMoveDuration, SpawnBuildings);

        SpawnNewTarget();
        await Task.Delay(Mathf.RoundToInt(rechargeTime * 1000));
        santaCheerOff.SetActive(true);
        downWinds.SetActive(false);

        Destroy(Instantiate(destroyParticles, presentTransform.position, Quaternion.identity), timeToDestroyParticles);
        presentSprite.sprite = presentSprites[Random.Range(0, presentSprites.Count)];
        presentJoint.enabled = true;
        present.transform.localPosition = presentStartLocalPos;
        presentJoint.connectedBody = presentRigidbody;
        presentJoint.anchor = Vector2.up / -2;
        presentJoint.connectedAnchor = Vector2.up / 2;
        present.transform.localPosition = presentStartLocalPos;
        Destroy(Instantiate(spawnParticles, presentSpawn), timeToDestroyParticles);


        missed = false;
        isPlaying = true;
        buttons.SetActive(true);
    }

    void SpawnBuildings()
    {
        float maxX = 0;
        float minX = 0;
        foreach(GameObject building in buildings)
        {
            if(building.transform.position.x < minX)
            {
                minX = building.transform.position.x;
            }else if(building.transform.position.x > maxX)
            {
                maxX = building.transform.position.x;
            }
        }

        if(minX < -buildingsXSpawnOffset)
        {
            foreach (GameObject building in buildings)
            {
                if (building.transform.position.x == minX)
                {
                    Destroy(building, 0.1f);
                    buildings.Remove(building);
                }
            }
        }

        while (maxX < buildingsXSpawnOffset)
        {
            buildings.Add(Instantiate(buildingsPref, buildingsSpawn));
            buildings[buildings.Count - 1].transform.position += Vector3.right * (maxX + buildingsXWidth);
            maxX += buildingsXWidth;
        }

    }
    void SpawnNewTarget()
    {
        PlayAud(bellsAud[Random.Range(0, bellsAud.Count)]);
        if (target != null)
        {
            target.LeanMoveY(minYSpawn, rechargeTime);
            Destroy(target, rechargeTime);
        }

        float spawnX = Random.Range(minXSpawn, maxXSpawn);

        target = Instantiate(targetObj, new Vector3(spawnX, minYSpawn, 0), Quaternion.identity);

        target.LeanMoveY(maxYSpawn,rechargeTime);
        forceText.SetActive(true);
    }
    private void Update()
    {
        if (!missed && presentTransform.position.y < minYSpawn)
        {
            missed = true;
            Miss();
        }
    }

    public void PlayAud(AudioClip clip)
    {
        GameObject aud = Instantiate(audioSpawn);
        AudioSource sour = aud.GetComponent<AudioSource>();
        sour.clip = clip;
        sour.Play();
        StartCoroutine(PlayAudDeleter(sour));
    }
    IEnumerator PlayAudDeleter(AudioSource sour)
    {
        while (sour.isPlaying)
        {
            yield return null;
        }
        Destroy(sour.gameObject);
    }

    void ShakeCam()
    {
        Shake(1);
    }
    void Shake(int iterr)
    {
        if (iterr * shakeCamDurationImpulse < shakeCamDuration)
        {
            if (iterr % 2 == 0)
                camera.LeanMoveY(initialCamPos.position.y + shakeCamPower, shakeCamDurationImpulse).setOnComplete(() => Shake(iterr + 1));
            else
                camera.LeanMoveY(initialCamPos.position.y + -shakeCamPower, shakeCamDurationImpulse).setOnComplete(() => Shake(iterr + 1));
        }
        else
            camera.LeanMoveY(initialCamPos.position.y, shakeCamDurationImpulse);
    }
    public void UpdateScoreText()
    {
        if(score >= Manager.save.maxScore)
        {
            scoreText.text = $"Max streak:\n{score}\nCurrent streak:\n{score}";
        }
        else
        {
            scoreText.text = $"Max streak:\n{Manager.save.maxScore}\nCurrent streak:\n{score}";
        }
    }
}
public static class Extentions
{
    public static AudioClip GetRandAudio(this List<AudioClip> list)
    {
        AudioClip res = null;
        if (list.Count <= 0) return null;

        res = list[Random.Range(0, list.Count)];

        return res;
    }
}