using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // �arpan objeyi yok eder
        Destroy(collision.gameObject);
    }

}
