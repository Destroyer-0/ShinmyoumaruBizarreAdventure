using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private GameObject m_SmallShinmyoumaru;
    [SerializeField] private GameObject m_HugeShinmyoumaru;
    [SerializeField] private float m_DefaultSpeed;


    private bool m_SmallStatus = true;
    private Rigidbody2D rb;
    private float horizontal;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
            throw new System.Exception("Rigidbody is null!");
        m_SmallStatus = true;
        SetStyle();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeStyle();
        }
    }

    private void FixedUpdate()
    {
        float delta_horizontal = Time.deltaTime * m_DefaultSpeed * horizontal;

        Vector2 pos = rb.transform.position;
        pos.x = pos.x + delta_horizontal;
        rb.MovePosition(pos);
    }

    private void ChangeStyle()
    {
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
}