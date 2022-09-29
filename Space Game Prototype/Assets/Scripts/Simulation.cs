﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public static float physicsTimeStep = 0.01f;
    public string name = "default";

    public List<StellarBody> bodies = new List<StellarBody>();
    public static Simulation instance;

    void Awake()
    {

        StellarBody[] bodies_arr = FindObjectsOfType<StellarBody>();
        
        foreach( StellarBody s in bodies_arr )
        {
            bodies.Add( s );
        }
        
        Time.fixedDeltaTime = physicsTimeStep;
        Debug.Log("Setting fixedDeltaTime to: " + physicsTimeStep);
    }

    public void AddBody(StellarBody s)
    {
        // bodies.Append(body);

        Debug.Log("before: " + bodies.Count);
        foreach (StellarBody body in bodies)
        {
            Debug.Log("before: " + body.bodyName);
        }

        bodies.Add(s);

        Debug.Log("after: " + bodies.Count);
        foreach (StellarBody body in bodies)
        {
            Debug.Log("after: " + body.bodyName);
        }

    }

    public void RemoveBody( StellarBody s )
    {
        List<StellarBody> bodies_temp = new List<StellarBody>();

        Debug.Log("before: " + bodies.Count);
        foreach( StellarBody body in bodies )
        {
            Debug.Log("before: " + body.bodyName);
        }

        foreach (StellarBody body in bodies)
        {
            if (body.bodyName != s.bodyName) bodies_temp.Add(body);
        }

        Debug.Log("Removed " + (bodies.Count - bodies_temp.Count) );

        bodies = bodies_temp;

    }
    


    void FixedUpdate()
    {
        Debug.Log("Sim name: " + name + " body count: " + bodies.Count);
        foreach (StellarBody body in bodies)
        {
            Debug.Log("bodies: " + body.bodyName);
        }
        
        for (int i = 0; i < bodies.Count; i++)
        {
            Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity(acceleration, physicsTimeStep);
            //bodies[i].UpdateVelocity (bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].UpdatePosition(physicsTimeStep);
        }

    }

    public static Vector3 CalculateAcceleration(Vector3 point, StellarBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * StellarBody.gravConstant * body.mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static List<StellarBody> Bodies
    {
        get
        {
            return Instance.bodies;
        }
    }

    public static Simulation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Simulation>();
            }
            return instance;
        }
    }
}