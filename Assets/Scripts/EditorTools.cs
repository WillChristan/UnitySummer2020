using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools : MonoBehaviour
{
    List<GameObject> unitsList;

    [Header("Tower Units")]
    public GameObject tower01;


    public void AutoGetPrefabs()
    {
        unitsList = new List<GameObject>();

        SetupPrefab(ref tower01, "Prefabs/Tower01");
    }

    void SetupPrefab(ref GameObject obj_, string prefabPath_)
    {
        obj_ = Resources.Load(prefabPath_) as GameObject;
        unitsList.Add(obj_);
    }

    //GUIButton Functions
    public void SpawnProp(string propname)
    {
        switch (propname)
        {
            //Towers
            case "Tower01":
                InstantiateObject(propname);
                break;

            //Defaults
            default:
                Debug.Log("Invalid Object...");
                break;
        }
    }

    void InstantiateObject(string objName_)
    {
        GameObject tmp;
        Material mat;
        Vector3 spawnLocation = new Vector3(30.0f, 0.0f, -40.0f);

        if (objName_.Contains("Tower"))
        {
            mat = new Material(Resources.Load("Material/TowerColour") as Material);
        }
        else if (objName_.Contains("Enemy"))
        {
            mat = new Material(Resources.Load("Material/EnemyColour") as Material);
        }
        else
        {
            mat = new Material(Shader.Find("Standard"));
        }


        foreach (GameObject unit in unitsList)
        {
            if (unit.name == objName_)
            {
                tmp = Instantiate(unit, spawnLocation, Quaternion.identity);
                //tmp.GetComponent<Renderer>().material = mat;
                Debug.Log(unit.name + " Instantiated");

                return;
            }
        }

        //If it gets here...
        Debug.Log("Unit doesn't exists in the List, " +
            "Reload the objects by clicking the 'Auto Get Prefabs' button" +
            "EditorTools.Line 172");
    }
}
