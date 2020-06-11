using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCaustics : MonoBehaviour
{
    Rigidbody rb;
    Vector3 cVelocity;
    float timeElapsed;
    float lifespan;
    int spawnNum;

    public GameObject bullet;
    public Vector3 scaleChange = new Vector3(0.2f, 0.2f, 0.2f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        this.GetComponent<Transform>().localScale = scaleChange;

        cVelocity = Vector3.zero;
        lifespan = 5f;
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifespan)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Tile")
        {
            SpawnCaustics();
        }
    }

    void SpawnCaustics()
    {
        scaleChange /= 2f;
        spawnNum = Random.Range(6, 12);

        if (scaleChange.x < 0.05f)
        {
            return;
        }

        if (bullet != null)
        {
            for (int i = 0; i < spawnNum; i++)
            {
                cVelocity = new Vector3(Random.Range(-4f, 4f), 8.0f, Random.Range(-4f, 4f));

                GameObject newBullet = Instantiate(bullet, rb.position, Quaternion.identity);

                newBullet.GetComponent<BulletCaustics>().scaleChange = scaleChange;
                newBullet.GetComponent<Rigidbody>().velocity = cVelocity;
            }
        }
    }
}
