using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameController.instance.UpdateScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
