using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class Target : MonoBehaviour
{
    public float checkTime = 0.5f;
    Collider2D thisCol;
    private void Awake()
    {
        thisCol = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckScore(collision);
    }
    async void CheckScore(Collider2D collision)
    {
        await Task.Delay(Mathf.RoundToInt(checkTime * 1000));
        if (PlayerController.instance.isPlaying)
        {
            if(collision!=null && thisCol!= null)
            if (collision.IsTouching(thisCol))
            {
                PlayerController.instance.Hit();
            }
        }
    }
}
