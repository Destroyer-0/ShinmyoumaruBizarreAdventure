using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public GameObject pendulum;

    public float pendulumLength;
    public float pendulumMass;
    public float pendulumBaseForce;
    private Pendulum.PendulumData m_PendulumData;


    public float force = 0.0f; // 施加的外力
    private float delta = 0;

    private void OnEnable()
    {
        m_PendulumData = Pendulum.BuildSimplePendulum(pendulumLength, pendulumMass, pendulumBaseForce);
    }

    // Update is called once per frame
    void Update()
    {
        m_PendulumData.UpdatePendulumSimulation(force, Time.deltaTime);
        //Debug.LogWarning(m_PendulumData.offset);
        lineRenderer.SetPosition(1, new Vector3(m_PendulumData.GetPendulumXOffset(), m_PendulumData.GetPendulumYOffset()));
        pendulum.transform.position = lineRenderer.GetPosition(1) + lineRenderer.transform.position - new Vector3(0, 0.5f);
        delta += Time.deltaTime;
    }
}

public static class Pendulum
{
    public struct PendulumData
    {
        public float pendulumLength;
        public float pendulumMass;
        public float pendulumBaseForce;


        public float offset;
        public float m_lastForce;
        public float m_lastTheta;
        public float m_timeSinceChange;

        public PendulumData(float pendulumLength, float pendulumMass, float pendulumBaseForce)
        {
            this.pendulumLength = pendulumLength;
            this.pendulumMass = pendulumMass;
            this.pendulumBaseForce = pendulumBaseForce;
            offset = 0;
            m_lastForce = pendulumBaseForce;
            m_lastTheta = 0;
            m_timeSinceChange = 0;
        }
    }


    //private static float m_lastForce = 0;

    public const float G = 9.8f;

    public static PendulumData BuildSimplePendulum(
        float pendulumLength,
        float pendulumMass,
        float pendulumBaseForce
    )
        => new(pendulumLength, pendulumMass, pendulumBaseForce);

    public static float GetPendulumXOffset(this ref PendulumData pendulumData)
    {
        return pendulumData.pendulumLength * Mathf.Sin(pendulumData.offset);
    }

    public static float GetPendulumYOffset(this ref PendulumData pendulumData)
    {
        return -pendulumData.pendulumLength * Mathf.Cos(pendulumData.offset);
    }

    public static void UpdatePendulumSimulation(this ref PendulumData pendulumData, float force, float delta)
    {
        // var theta = Mathf.Atan(m_lastForce / (pendulumData.pendulumMass * G));
        // if (!Mathf.Approximately(force, m_lastForce))
        // {
        //     m_lastForce = force;
        //     var thetaNew = Mathf.Atan(force / (pendulumData.pendulumMass * G));
        //     offset = thetaNew + (theta - thetaNew) * Mathf.Cos(Mathf.Sqrt(G / pendulumData.pendulumLength) * delta);
        // }
        // else
        // {
        //     offset = theta * Mathf.Cos(delta * Mathf.Sqrt(G / pendulumData.pendulumLength));
        // }

        var thetaNew = Mathf.Atan(force / (pendulumData.pendulumMass * G));
        var oldTheta = Mathf.Atan(pendulumData.pendulumBaseForce / (pendulumData.pendulumMass * G));

        // 如果风力发生变化，记录时间并更新角度
        if (!Mathf.Approximately(force, pendulumData.m_lastForce))
        {
            pendulumData.m_lastForce = force;
            pendulumData.m_lastTheta = thetaNew; // 更新为新的平衡角度
            pendulumData.m_timeSinceChange = 0f; // 重置时间
        }

        // 更新时间
        pendulumData.m_timeSinceChange += delta;

        // 计算新的偏移量，简谐运动围绕新的风力平衡角度 thetaNew
        float angularFrequency = Mathf.Sqrt(G / pendulumData.pendulumLength);
        pendulumData.offset = thetaNew + (oldTheta - thetaNew) * Mathf.Cos(angularFrequency * pendulumData.m_timeSinceChange);
        Debug.LogWarning(pendulumData.offset + " / " + pendulumData.m_timeSinceChange);
    }
}