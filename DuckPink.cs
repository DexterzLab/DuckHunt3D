using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckPink : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        baseScore = 300;
        Speed = 10;
    }

    // Update is called once per frame

    protected override void Update()
    {
        base.Update();

    }

}
