using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capCol;
    BoxCollider boxCol;
    Animator ani;
    GameObject body;
    
    float inputH;
    float inputV;
    Vector3 forwardVector;
    Vector3 rightVector;
    Vector3 gravityVec;

    float speed;
    float castTime;
    float castTimeElapsed;

    bool isGrounded;
    bool isCastingSpell;

    public float maxSpeed;
    public float gravity;

    public Transform camera;
    public Transform camArm;
    public GameObject groundPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        boxCol = GetComponent<BoxCollider>();
        ani = GetComponent<Animator>();
        body = this.gameObject;

        maxSpeed = 2.0f;
        speed = maxSpeed;

        gravity = -9.81f;
        gravityVec = Vector3.zero;

        castTime = 0.6f;
        castTimeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ValueUpdate();
        
        CastSpell();

        if (!isCastingSpell)
        {
            Movement();
            Animation();
        }

    }

    void ValueUpdate()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        forwardVector = camera.forward;
        rightVector = camera.right;
    }
    
    void Movement()
    {
        Vector3 forwardVec = new Vector3(forwardVector.x, 0.0f, forwardVector.z);
        Vector3 rightVec = new Vector3(rightVector.x, 0.0f, rightVector.z);

        //Vertical
        if(inputV != 0.0f)
        {
            forwardVec = forwardVec * inputV * Time.deltaTime;
        }
        else
        {
            forwardVec = Vector3.zero;
        }

        //Horizontal
        if(inputH != 0.0f)
        {
            rightVec = rightVec * inputH * Time.deltaTime;
        }
        else
        {
            rightVec = Vector3.zero;
        }

        //Custom Gravity
        if (!isGrounded)
        {
            gravityVec = gravityVec + (Vector3.up * gravity) * Time.deltaTime;
        }
        else
        {
            gravityVec = Vector3.zero;
        }

        rb.velocity = ((forwardVec + rightVec).normalized * speed) + gravityVec;
    }

    void Animation()
    {
        if (Vector3.Magnitude(rb.velocity) > 0.0f)
        {
            ani.SetBool("isMoving", true);
        }
        else
        {
            ani.SetBool("isMoving", false);
        }

        Vector2 moveVec = new Vector2(Mathf.Abs(inputH), inputV);
        float forwardMove = Vector2.SqrMagnitude(moveVec.normalized);

        //Moving backward
        if (inputV < 0.0f)
        {
            ani.SetFloat("MoveSpeed", inputV);
        }
        //Moving forward
        else
        {
            ani.SetFloat("MoveSpeed", forwardMove);
        }

        //Rotate player based on camera rotation
        if (inputH != 0.0f || inputV != 0.0f)
        {
            float angle = camArm.rotation.eulerAngles.y;

            if (inputV >= 0.0f)
            {
                angle += inputH * 90.0f * (-0.5f * inputV + 1.0f);
                body.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
            else
            {
                angle += -inputH * 90.0f * (0.5f * inputV + 1.0f);
                body.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
        }
        
    }

    void CastSpell()
    {
        if (Input.GetButtonDown("Fire1") && !isCastingSpell)
        {
            rb.velocity = Vector3.zero;
            ani.SetBool("isMoving", false);

            isCastingSpell = true;
            ani.SetBool("Spell01", isCastingSpell);

            castTimeElapsed = castTime;
        }
        else
        {
            if (castTimeElapsed <= 0.0f)
            {
                isCastingSpell = false;
                ani.SetBool("Spell01", isCastingSpell);

                castTimeElapsed = 0.0f;

                return;
            }

            castTimeElapsed -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ground")
        {
            isGrounded = false;
        }
    }

}
