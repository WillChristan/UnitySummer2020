using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BulletHybridECS : MonoBehaviour
{
    public float speed;
}

class BulletHybridSystem: ComponentSystem
{
    struct Components
    {
        public Transform transform;
    }


    protected override void OnUpdate()
    {
        
    }
}
