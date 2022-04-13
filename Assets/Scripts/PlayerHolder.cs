using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{
    public static PlayerHolder Instance;

    public bool canMove = false;
    public float speed;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            transform.Translate(Vector3.forward * speed);

        }
    }
}
