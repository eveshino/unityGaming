using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 3;
    public float speed;
    public float jumpForce;

    
    private float movement;

    private bool isJumping;
    private bool doubleJumping;
    private bool isFiring;

    private Rigidbody2D rig;
    private Animator anim;

    public GameObject bow;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameController.instance.UpdateLives(health);
    }

    // Update is called once per frame
    void Update()
    {
       
        Jump();
        BowFire();
    }
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //if not pressed the value is 0. if pressed to right(>) max value is 1, to left(<) max value is -1
         movement = Input.GetAxis("Horizontal");
    
        //Add velocity to character body in axis X and Y
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        // running to right
        if(movement > 0 ) 
        {
            if(!isJumping) 
            {
                anim.SetInteger("transition", 1);
            }
          
            transform.eulerAngles = new Vector3(0,0,0);
        }
        // running to left
        if(movement < 0)
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if(movement == 0 && !isJumping && !isFiring) 
        {
            anim.SetInteger("transition", 0);   
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            //if the character is not jumping 
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);  
                doubleJumping = true;
                isJumping = true;
            }
            else
            {
                {
                    if(doubleJumping)
                    {
                        anim.SetInteger("transition", 2);
                        rig.AddForce(new Vector2(0, jumpForce * 2), ForceMode2D.Impulse); 
                        doubleJumping = false;
                    }
                }
            }
               
        }

    }

    void BowFire()
    {
      StartCoroutine("Fire"); 

    }

    IEnumerator Fire()
    {
          if(Input.GetKeyDown(KeyCode.E))
        {
            isFiring = true;
            anim.SetInteger("transition", 3);
            GameObject Bow = Instantiate(bow, firePoint.position, firePoint.rotation);

            if(transform.rotation.y == 0)
            {
                Bow.GetComponent<Bow>().isRight = true;
            }

            if(transform.rotation.y == 180)
            {
                Bow.GetComponent<Bow>().isRight = false;
            }

            yield return new WaitForSeconds(0.2f);
            isFiring = false;
            anim.SetInteger("transition", 0);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 6)
        {
            isJumping = false;
        }
        if(coll.gameObject.layer == 7)
        {
            GameController.instance.GameOver();
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);
        anim.SetTrigger("hit");

        if( transform.rotation.y == 0 )
        {
            transform.position += new Vector3(-0.5f, 0, 0);
            // rig.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        }

        if ( transform.rotation.y == 180)
        {
            transform.position += new Vector3(0.5f, 0, 0);
            // rig.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        }

        if(health <= 0)
        {
            // Call GameOver
            GameController.instance.GameOver();
        }
    }

    public void IncreaseHealth(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }
   
}
