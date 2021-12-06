using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    //public float max, current;
    public GameObject Fill;

    void Awake()
    {
        Fill = transform.GetChild(0).transform.GetChild(0).gameObject;
    }

    public void SetFill(float FillAmount)
    {
        if (FillAmount <= 0)
            Destroy(this.gameObject);

        Vector3 temp = Fill.transform.localScale;
        temp.Set(FillAmount, 1, 1);
        Fill.transform.localScale = temp;
    }
}
