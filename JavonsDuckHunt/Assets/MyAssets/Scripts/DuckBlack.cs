using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckBlack : Enemy
{

    protected override void Start()
    {
        baseScore = 100;
        Speed = 5;
    }

    protected override void Update()
    {
        base.Update();
    }
}
