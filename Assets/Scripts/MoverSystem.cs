using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class MoverSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation trans, ref MoverComponent moverComponent) =>
        {
            trans.Value.y += moverComponent.moveSpeed * Time.DeltaTime;

            if (trans.Value.y > 5f)
            {
                moverComponent.moveSpeed = -Mathf.Abs(moverComponent.moveSpeed);
            }
            if (trans.Value.y < -5f)
            {
                moverComponent.moveSpeed = +Mathf.Abs(moverComponent.moveSpeed);
            }

        });
    }
}
