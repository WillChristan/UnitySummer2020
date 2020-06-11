using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class BulletSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        base.OnCreate();

    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Translation trans, ref BulletComponent data) =>
        {
            data.velocity.y = 1f * deltaTime;
            trans.Value.y -= data.velocity.y;

        }).Run();

        return default;
    }

}
