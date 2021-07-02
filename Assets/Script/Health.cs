using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float P1Health = 1;

    public Image HealthBar1;


    private void OnCollisionEnter(Collision other)
    {
        if(P1Health > 0)
        {
            P1Health = P1Health - 0.2f;

            HealthBar1.fillAmount = P1Health;

        }
        if (other.transform.tag == "Point")
        {
            P1Health = P1Health + 0.2f;

            HealthBar1.fillAmount = P1Health;
        }
        else
        {

        }
    }
}
