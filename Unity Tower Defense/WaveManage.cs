using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManage : MonoBehaviour
{
    public static WaveManage main;

    public Transform startPoint;
    public Transform[] path;
    //public Transform[] endPoint;

    private void Awake()
    {
        main = this;
    }
    

 
    
}
