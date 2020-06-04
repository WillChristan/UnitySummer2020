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
    Vector3 forwardVector;
    Vector3 rightVector;
    Vector3 gravityVec;
    Vector3 projectileForce;

    float lastAngle = 0.0f;
    float speed;
    float castTime;
    float castTimeElapsed;

    bool isGrounded;
    bool isCastingSpell;
    bool isCastHold;
    bool stopMoving;

    public float maxSpeed;
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

        gravity = -9.81f;
        gravityVec = Vector3.zero;

        castTime = 0.2f;
        castTimeElapsed = 0.0f;

        castSpawn.SetActive(false);

        isCastHold = false;
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

    void MoveAnimation()
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

            lastAngle = angle;
        }
        else
        {
            body.transform.rotation = Quaternion.Euler(0.0f, lastAngle, 0.0f);
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

       
        //mult = Mathf.Clamp(angle.x - 45.0f + mult);
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
        Debug.Log(angleHeight);

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

    private void OnCollisionEnter(Collision col)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationY;
       
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
