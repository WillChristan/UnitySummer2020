using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

public class BulletEntity : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        EntityManager enManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype enArchetype = enManager.CreateArchetype(
            typeof(BulletComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(MoverComponent)
            );

        NativeArray<Entity> enArray = new NativeArray<Entity>(2, Allocator.Temp);

        enManager.CreateEntity(enArchetype, enArray);

        for (int i = 0; i < enArray.Length; i++)
        {
            Entity en = enArray[i];

            enManager.SetComponentData(en, new BulletComponent { val = Random.Range(10, 20) });
            enManager.SetComponentData(en, new MoverComponent { moveSpeed = Random.Range(1f, 2f) });
            enManager.SetComponentData(en, new Translation { Value = new Vector3(Random.Range(-8f, 8f), Random.Range(-5f, 5f), 0f)});


            enManager.SetSharedComponentData(en, new RenderMesh
            {
                mesh = mesh,
                material = mat
            });
        }

        enArray.Dispose();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
