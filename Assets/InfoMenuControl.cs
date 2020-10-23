using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenuControl : MonoBehaviour
{
    public GameObject infoMenu;

    // Start is called before the first frame update
    void Start()
    {
        if(infoMenu != null)
        {
            infoMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (infoMenu == null) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (infoMenu.activeInHierarchy)
            {
                infoMenu.SetActive(false);
            }
            else
            {
                infoMenu.SetActive(true);
            }
        }


    }

}
