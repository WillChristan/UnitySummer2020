using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject EndMenu;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Unlock mouse
    //Reset level and player for Restart
    //Freeze game?

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (EndMenu != null)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                EndMenu.SetActive(true);
            }
        }
    }
}
