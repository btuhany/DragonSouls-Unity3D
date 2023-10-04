using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
