using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckGreen : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        baseScore = 200;
        Speed = 8;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
