using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NjihaloMovement : MonoBehaviour
{
    #region Public Variables
    public Rigidbody2D body2d;
    public float leftPushRange;
    public float rightPushRange;
    public float velocityThreshold;
    public enum GibanjeSmjer { Lijevo, Desno };
    public GibanjeSmjer pocetniSmjer;
    #endregion

    #region Main Methods

    void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
        body2d.angularVelocity = velocityThreshold;

        if (pocetniSmjer == GibanjeSmjer.Lijevo)
        {
            body2d.angularVelocity = velocityThreshold * -1;
        }
    }

    void Update()
    {
        Push();
    }

    public void Push()
    {
        if (transform.rotation.z > 0
            && transform.rotation.z < rightPushRange
            && (body2d.angularVelocity > 0)
            && body2d.angularVelocity < velocityThreshold)
        {
            body2d.angularVelocity = velocityThreshold;
        }
        else if (transform.rotation.z < 0
                 && transform.rotation.z > rightPushRange
                 && (body2d.angularVelocity < 0)
                 && body2d.angularVelocity > velocityThreshold * -1)
        {
            body2d.angularVelocity = velocityThreshold * -1;
        }
    }
    #endregion
}
