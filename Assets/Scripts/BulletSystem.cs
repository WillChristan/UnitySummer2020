using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BulletSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref BulletComponent bulletComponent) =>
        {
            bulletComponent.val += 1.0f * Time.DeltaTime;
        });
    }

    void Shoot()
    {
        
    }

}
