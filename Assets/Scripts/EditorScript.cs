using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(EditorTools))]
public class EditorScript : Editor
{
    float thumbnailWidth = 70;
    float thumbnailHeight = 70;
    float labelWidth = 150f;

    EditorTools eTools;

    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        // Call base class method
        base.OnInspectorGUI();

        GUILayout.Space(20f);
        GUILayout.Label("Button Tools", EditorStyles.boldLabel);

        //Load Prefabs Automatically
        GUILayout.BeginHorizontal();

        if (GUILayout.Button(new GUIContent("Auto Get Prefabs", "Automatically load all towers & enemies prefabs.")))
        {
            eTools = GameObject.Find("GameEditor").GetComponent<EditorTools>();
            eTools.AutoGetPrefabs();
        }
        GUILayout.EndHorizontal();

        EditorTools edTools = (EditorTools)target;

        //Stats Reset buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Reset Tower Stats", "Apply the default stats to tower units")))
        {
            eTools = GameObject.Find("GameEditor").GetComponent<EditorTools>();
        }

        GUILayout.EndHorizontal();


        // Custom Buttons with Image as Thumbnail
        // Tower Buttons
        GUILayout.Label("Spawn Tower");
        GUILayout.BeginHorizontal();

        CreateButton(ref edTools, "Prefabs/Tower01", "Tower01");

    }

    void CreateButton(ref EditorTools edTools_, string prefabPathName_, string spawnCall_)
    {
        Object obj = Resources.Load(prefabPathName_);
        Texture2D image = AssetPreview.GetAssetPreview(obj);

        if (GUILayout.Button(image, GUILayout.Width(thumbnailWidth), GUILayout.Height(thumbnailHeight)))
            edTools_.SpawnProp(spawnCall_);
    }

}
