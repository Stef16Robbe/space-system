using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    Rigidbody rb;
    const float G = 667.4f;

    public static List<Gravity> GVRobj;
    public bool star = false;
    public int lateralSpeed = 1000;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (GVRobj == null) GVRobj = new List<Gravity>();
        GVRobj.Add(this);

        if (!star) rb.AddForce(Vector3.left * lateralSpeed);
    }

    private void FixedUpdate()
    {
        foreach (Gravity obj in GVRobj)
        {
            if (obj != this)
                Attract(obj);
        }
    }


    private void Attract(Gravity objToAttract)
    {
        Rigidbody rbObjToAttract = objToAttract.rb;
        Vector3 direction = rb.position - rbObjToAttract.position;
        float distance = direction.magnitude;
        if (distance == 0) return;
        float forceMagnitude = G * (rb.mass * rbObjToAttract.mass) / Mathf.Pow(distance, 2);

        Vector3 force = direction.normalized * forceMagnitude;

        rbObjToAttract.AddForce(force);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Gravity otherObject = collision.collider.GetComponent<Gravity>();
        if (!star && otherObject.star) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GVRobj.Remove(this);
    }
}
