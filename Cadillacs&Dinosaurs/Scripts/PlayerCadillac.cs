using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCadillac : MonoBehaviour
{
    [SerializeField]

    private float maxSpeed;

    [SerializeField]

    private float speed;

    [SerializeField]

    private float jumpForce;

    [SerializeField]

    private Animator anim;

    [SerializeField]

    private Rigidbody rig;

    [SerializeField]

    private SpriteRenderer sprite;

    [SerializeField]

    private bool run;

    [SerializeField]

    private bool isGround;

    [SerializeField]

    private bool isGroundUp;

    [SerializeField]

    private bool jump;

    [SerializeField]

    private bool action;

    [SerializeField]

    private GameObject[] NPC;

    [SerializeField]

    private GameObject Trigger;

    [SerializeField]

    private Vector3 Weapon_Pos;

    [SerializeField]

    private float lastTapTime, tapSpeed;




    void Start()
    {
        maxSpeed = speed * 2;

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(DisableTitle());
    }

    IEnumerator DisableTitle()
    {

        //This function has the responsibility  of "freeze" the game for some seconds and disable the HUD. 
        ////After "unfreeze" and the player he can move and active the HUD.

        GameObject HUD = GameObject.Find("Canvas");

        HUD.SetActive(false);


        action = false;

        yield return new WaitForSeconds(4);

        GameObject cam = GameObject.Find("Main Camera");

        cam.transform.GetChild(0).gameObject.SetActive(false);

        rig.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);

        anim.SetTrigger("Especial_Attack");

        this.transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        GameObject soundIntro = GameObject.Find("Introducion");

        GameObject soundStage = GameObject.Find("StageMusic");

        Destroy(soundIntro);
        soundStage.GetComponent<AudioSource>().enabled = true;

        this.transform.GetChild(0).gameObject.SetActive(false);
        action = true;
        HUD.SetActive(true);
        

    }

    void FixedUpdate()
    {
        Movement();
        Jump();


    }

    private void Update()
    {
        Attacks();
    }

    private void Jump() {

        if (Input.GetKeyDown(KeyCode.Space) && isGround) 
        {

            jump = true;
            rig.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            anim.SetBool("Jump", true);

        }

    }

    private void EnableMovement()
    {
        action = true;
       
    }

    private void DisableMovement()
    {
        action = false;
      
    }

    private void Movement()
    {
        if (action)
        {

            float h = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            float v = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
       

            transform.Translate(h, 0, v);



            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                anim.SetBool("Walk", true);
                sprite.flipX = false;

            }

            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                anim.SetBool("Walk", true);
                sprite.flipX = true;
            }

            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                anim.SetBool("Walk", true);
                sprite.flipX = true;
            }

            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                anim.SetBool("Walk", true);
                sprite.flipX = false;
            }

            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                anim.SetBool("Walk", false);
                run = false;
                anim.SetBool("Run", false);
                speed = maxSpeed / 2;


            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {


                //Time for the next "Walk" input.
                //This enable the double click for the player "Run"

                if ((Time.time - lastTapTime) < tapSpeed && Input.GetKey(KeyCode.RightArrow))
                {
                    run = true;
                    anim.SetBool("Run", run);
                    sprite.flipX = false;
                    speed = maxSpeed;
                }



                lastTapTime = Time.time;

            }

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

                if ((Time.time - lastTapTime) < tapSpeed && Input.GetKey(KeyCode.LeftArrow))
                {
                    run = true;

                    anim.SetBool("Run", run);
                    sprite.flipX = true;
                    speed = maxSpeed;

                }



                lastTapTime = Time.time;

            }

        }

    }



    private void Attacks()
    {

        if (Input.GetKeyDown(KeyCode.E) && isGround && !run && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            Debug.Log("Especial Attack");
            anim.SetTrigger("Especial_Attack");
            rig.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);



        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Punch");
            anim.SetBool("Punch", true);

        }
        else {

            anim.SetBool("Punch", false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isGround)
        {
            Debug.Log("Air strike Normal");
            anim.SetTrigger("Jump_K_Attack");

        }

        if (Input.GetKeyDown(KeyCode.R) && isGround && run)
        {
            Debug.Log("Air strike");
            //anim.SetTrigger("Jump_K_F_Attack");
            rig.AddForce(new Vector3(0, jumpForce - 1, 0), ForceMode.Impulse);


        }


    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")

        {

            isGround = true;
            anim.SetBool("Jump", false);

        }

       

        


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TriggerCam")

        {

            GameObject Go = GameObject.Find("Canvas").transform.GetChild(1).gameObject;

            Go.SetActive(false);

            GameObject cam = GameObject.Find("Main Camera");
            cam.GetComponent<Animator>().enabled = false;
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(5f, 2.17f, cam.transform.position.z),0.05f);


        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")

        {
            anim.SetBool("Jump", true);

            isGround = false;
        }




    }
}
