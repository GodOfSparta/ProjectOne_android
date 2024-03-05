using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;


    void Update()
    {
        transform.Rotate(0,0, rotateSpeed * Time.deltaTime);
    }
}
