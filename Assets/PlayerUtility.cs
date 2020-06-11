using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtility : MonoBehaviour
{
    GameObject CheckPoint;
    GameObject Player;

    public GameObject StartPoint;

    // Start is called before the first frame update
    void Start()
    {
        Player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(Vector3 newLocation)
    {
        Player.transform.position = newLocation;
    }

    public void ResetPlayer()
    {
        CheckPoint = StartPoint;
        MovePlayer(StartPoint.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillZone")
        {
            if (CheckPoint != null)
            {
                Player.transform.position = CheckPoint.transform.position;
            }
            else
            {
                Player.transform.position = StartPoint.transform.position;
                Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        if (other.tag == "Respawn")
        {
            CheckPoint = other.gameObject;
        }
    }

}
