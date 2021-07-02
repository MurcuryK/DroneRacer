using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour {

    public Text pointText;
    private int PointNum;


    void OnCollisionEnter(Collision other)
    {
     if (other.transform.tag == "Point")
        {
            PointNum++;
            pointText.text = "Point: " + (int)PointNum + "/16";
            Destroy(other.gameObject);
        }
        else
        {

        }
        
    }
}
