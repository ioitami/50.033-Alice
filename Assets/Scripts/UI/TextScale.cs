using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScale : MonoBehaviour
{
    public float scaleUpAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScaleUp()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(scaleUpAmount, scaleUpAmount, scaleUpAmount);
    }

    public void ScaleNormal()
    {
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
    }
}
