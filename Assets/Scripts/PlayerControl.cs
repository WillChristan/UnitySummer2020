using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;
    Animator ani;
    GameObject body;
    
    float inputH;
    float inputV;
    float jumpVal;
    float gravityVal;
    Vector3 forwardVector;
    Vector3 rightVector;
    Vector3 projectileForce;

    float lastAngle = 0.0f;
    float speed;
    float castTime;
    float castTimeElapsed;

    bool isGrounded;
    bool isJumping;
    bool isFalling;
    bool isCastingSpell;
    bool isCastHold;
    bool stopMoving;

    public float maxSpeed;
    public float jumpForce;
    public float gravity;

    public Transform camera;
    public Transform camArm;
    public GameObject castSpawn;
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        body = this.gameObject;

        maxSpeed = 2.0f;
        speed = maxSpeed;

        if (jumpForce <= 0.0f) jumpForce = 800.0f;
        jumpVal = 0.0f;

        gravity = -9.81f;
        gravityVal = 0.0f;

        castTime = 0.2f;
        castTimeElapsed = 0.0f;

        castSpawn.SetActive(false);

        isCastHold = false;
        isJumping = false;
        isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        ValueUpdate();

        CastSpell();

        if (!stopMoving)
        {
            Movement();
            MoveAnimation();
        }

        if (isCastingSpell)
        {
            SpellAction();
        }

    }

    void ValueUpdate()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        forwardVector = camera.forward;
        rightVector = camera.right;

        if (isJumping)
        {
            if (rb.velocity.y < 0.0f && !isFalling)
            {
                isFalling = true;
                ani.SetBool("isFalling", true);
            }

            if (isFalling && isGrounded)
            {
                isFalling = false;
                isJumping = false;

                ani.SetBool("isFalling", false);
                ani.SetBool("isJumping", false);
            }
        }
    }

    void Movement()
    {
        Vector2 forwardVec = new Vector2(forwardVector.x, forwardVector.z);
        Vector2 rightVec = new Vector2(rightVector.x, rightVector.z);

        //Vertical
        if(inputV != 0.0f)
        {
            forwardVec = forwardVec * inputV * Time.deltaTime;
        }
        else
        {
            forwardVec = Vector2.zero;
        }

        //Horizontal
        if(inputH != 0.0f)
        {
            rightVec = rightVec * inputH * Time.deltaTime;
        }
        else
        {
            rightVec = Vector2.zero;
        }

        //Jumping
        if (Input.GetButtonDown("Jump"))
        {
            jumpVal += (jumpForce / (jumpVal + 1.0f)) * Time.deltaTime;

            isJumping = true;
            ani.SetBool("isJumping", true);
        }

        //Custom Gravity
        if (!isGrounded)
        {
            gravityVal = gravityVal + gravity * Time.deltaTime;
        }
        else
        {
            gravityVal = 0.0f;
        }

        Vector2 moveVec = (forwardVec + rightVec).normalized * speed;

        rb.velocity = new Vector3(moveVec.x, jumpVal + gravityVal, moveVec.y);
    }

    void MoveAnimation()
    {
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

            lastAngle = angle;
        }

        //Jump condition
        if (isJumping)
        {
            ani.SetFloat("MoveSpeed", 0.0f);
            ani.SetBool("isMoving", false);
            return;
        }

        if (rb.velocity.x != 0.0f || rb.velocity.z != 0.0f)
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

    }

    void CastSpell()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isCastHold = true;
            StartCoroutine(CastHoldCoroutine());
        }

        else if (Input.GetButtonUp("Fire1"))
        {
            isCastHold = false;

            if (!isCastingSpell)
            {
                return;
            }

            isCastingSpell = false;
            ani.SetBool("Spell01", isCastingSpell);

            StartCoroutine(PauseMoveCoroutine(0.8f));


            castSpawn.SetActive(false);

            isCastHold = false;
            castTimeElapsed = 0.0f;
        }
    }

    void SpellAction()
    {
        Vector3 angle = camArm.transform.rotation.eulerAngles;

        float angleHeight;

        if (angle.x > 180.0f)
        {
            angleHeight = angle.x - 360.0f;
        }
        else
        {
            angleHeight = angle.x;
        }

        float mult = 0.2f * angleHeight;
        mult *= mult;
        mult -= 45.0f;

        body.transform.rotation = Quaternion.Euler(0.0f, angle.y + 45.0f, 0.0f);

        castSpawn.transform.rotation = Quaternion.Euler(angleHeight + mult, angle.y, 0.0f);

        projectileForce = castSpawn.transform.forward;
    }

    void FireProjectile()
    {
        if (projectilePrefab == null) return;

        GameObject bullet = Instantiate(projectilePrefab, castSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = projectileForce * 10.0f;
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.GetContact(0).normal == Vector3.up)
        {
            isGrounded = true;

            jumpVal = 0.0f;
        }
    }

    private void OnCollisionStay(Collision col)
    {
       
    }

    private void OnCollisionExit(Collision col)
    {

    }

    IEnumerator PauseMoveCoroutine(float pauseTime)
    {
        float timeElapsed = 0.0f;
        float timeInc = 0.1f;

        while (true)
        {
            if (timeElapsed >= pauseTime)
            {
                stopMoving = false;
                break;
            }

            if (timeElapsed == pauseTime / 2.0f)
            {
                FireProjectile();
            }

            timeElapsed += timeInc;
            yield return new WaitForSeconds(timeInc);
        }
    }

    IEnumerator CastHoldCoroutine()
    {
        float timeInc = 0.1f;

        while (isCastHold)
        {
            if (castTimeElapsed < castTime)
            {
                castTimeElapsed += timeInc;
                yield return new WaitForSeconds(timeInc);
            }
            else
            {
                isCastingSpell = true;

                ani.SetBool("Spell01", isCastingSpell);

                rb.velocity = Vector3.zero;
                ani.SetBool("isMoving", false);

                stopMoving = true;

                castSpawn.SetActive(true);
                break;
            }
        }

        castTimeElapsed = 0.0f;
    }

}
