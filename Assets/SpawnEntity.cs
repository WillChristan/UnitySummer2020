using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class SpawnEntity : MonoBehaviour
{
    Entity en;
    EntityManager enManager;
    BlobAssetStore bStore;

    Vector3 minBound;
    Vector3 maxBound;

    float timeElapsed;
    float killTime;

    List<Entity> entities;

    public GameObject prefab;
    public int num;
    
    // Start is called before the first frame update
    void Start()
    {
        enManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        bStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, bStore);
        en = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);

        entities = new List<Entity>();

        minBound = new Vector3(-30f, 15f, 10f);
        maxBound = new Vector3(20f, 15f, 60f);

        timeElapsed = 0.0f;
        killTime = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        for(int i = 0; i < num; i++)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Entity enObj = enManager.Instantiate(en);

                enManager.SetComponentData(en, new Translation
                {
                    Value = new Vector3(Random.Range(minBound.x, maxBound.x), maxBound.y, Random.Range(minBound.z, maxBound.z))
                });

                entities.Add(enObj);
            }    
        }

        if (timeElapsed > killTime)
        {
            if (entities.Count == 0)
            {
                timeElapsed = 0.0f;
                return;
            }

            foreach(Entity en in entities)
            {
                enManager.DestroyEntity(en);
            }
            entities.Clear();

            timeElapsed = 0.0f;
        }
    }

    private void OnDestroy()
    {
        foreach (Entity en in entities)
        {
            enManager.DestroyEntity(en);
        }

        entities.Clear();
        bStore.Dispose();
    }

}
