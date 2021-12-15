using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDelete : MonoBehaviour
{
    float time = 2.0f;

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
            Destroy(this.gameObject);
    }
}
