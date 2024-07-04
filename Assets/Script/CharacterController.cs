using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private GameObject m_SmallShinmyoumaru;
    [SerializeField] private GameObject m_HugeShinmyoumaru;
    [SerializeField] private float m_DefaultSpeed;
    [SerializeField] private float m_SmallJumpSpeed;
    [SerializeField] private float m_BigJumpSpeed;


    private bool m_SmallStatus = true;
    [SerializeField] private bool m_IsJumping = false;
    private Rigidbody2D rb;
    private float horizontal;

    public Vector2 bigLeftCenter;
    public Vector2 bigRightCenter;
    public Vector2 bigLeftSize;
    public Vector2 bigRightSize;
    public float m_Offset;
    public LayerMask LayerMask;


    public const float G = 9.8f;

    private float m_CurrentVSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
            throw new System.Exception("Rigidbody is null!");
        m_SmallStatus = true;
        m_IsJumping = false;
        SetStyle();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        m_IsJumping = !CheckArriveGround();

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeStyle();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        float delta_horizontal = Time.deltaTime * m_DefaultSpeed * horizontal;

        // Vector2 pos = rb.transform.position;
        // pos.x = pos.x + delta_horizontal;
        // rb.MovePosition(pos);
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = horizontal * m_DefaultSpeed;
        rb.velocity = newVelocity;
    }

    private void Jump()
    {
        if (!CheckArriveGround() || m_IsJumping)
        {
            return;
        }

        m_IsJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, m_SmallJumpSpeed);
    }

    private void ChangeStyle()
    {
        if (!CheckCouldChange() && m_SmallStatus)
        {
            return;
        }

        m_SmallStatus = !m_SmallStatus;
        SetStyle();
    }

    private void SetStyle()
    {
        if (m_SmallStatus)
        {
            m_SmallShinmyoumaru.SetActive(true);
            m_HugeShinmyoumaru.SetActive(false);
        }
        else
        {
            m_SmallShinmyoumaru.SetActive(false);
            m_HugeShinmyoumaru.SetActive(true);
        }
    }

    private bool CheckCouldChange()
    {
        var left = Physics2D.OverlapBox(transform.position + (Vector3)bigLeftCenter, bigLeftSize, 0, LayerMask);
        var right = Physics2D.OverlapBox(transform.position + (Vector3)bigRightCenter, bigRightSize, 0, LayerMask);
        return !(left && right);
    }

    private bool CheckArriveGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, m_Offset, LayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)bigLeftCenter, bigLeftSize);
        Gizmos.DrawWireCube(transform.position + (Vector3)bigRightCenter, bigRightSize);
        Handles.Label(
            transform.position + new Vector3(-0.25f, 1f),
            $"{m_IsJumping}"
        );
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * m_Offset);
    }
}