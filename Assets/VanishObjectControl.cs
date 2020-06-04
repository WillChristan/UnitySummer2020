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

        visibility = -1.0f;

        Mat.SetFloat("Vector1_4E689299", visibility);
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

    IEnumerator ShiftVisibility()
    {
        while (true)
        {
            if (visibility >= 1.0f) break;

            visibility += 0.05f;
            Mat.SetFloat("Vector1_4E689299", visibility);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
