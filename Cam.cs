using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{   

    private Transform player;
    public float smooth;

    // Start is called before the first frame update
    void Start()
    {
        //search tag player in the scene
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame, LateUpdate to get a little smoothness
    void LateUpdate()
    {
        if(player.position.x >= 0 )
        {
            // only moves o axis X   (< and >)
            Vector3 following = new Vector3(player.position.x, transform.position.y, transform.position.z);
            //Lerp return a vector 3 in two points , cam position and following in smooth velocity
            transform.position = Vector3.Lerp(transform.position, following, smooth * Time.deltaTime);
        }
       
    }
}
