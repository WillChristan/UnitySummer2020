using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct BulletComponent : IComponentData
{
    public Vector3 velocity;
    public float lifetime;
}
