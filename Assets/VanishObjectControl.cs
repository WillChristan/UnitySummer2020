using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishObjectControl : MonoBehaviour
{
    Material Mat;
    float visibility;

    // Start is called before the first frame update
    void Start()
    {
        Mat = GetComponent<MeshRenderer>().material;

        visibility = 0.0f;

        Color colour = new Color(Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f));

        Mat.SetFloat("Vector1_4E689299", visibility);
        Mat.SetColor("Color_66566301", colour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Projectile")
        {
            StartCoroutine(ShiftVisibility());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            this.GetComponent<BoxCollider>().isTrigger = false;
            StartCoroutine(ShiftVisibility());
        }
    }

    IEnumerator ShiftVisibility()
    {
        while (true)
        {
            if (visibility >= 1.0f) break;

            visibility += 0.01f;
            Mat.SetFloat("Vector1_4E689299", visibility);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
