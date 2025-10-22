using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("이동 제한")]
    [SerializeField] private float minX = 8f;
    [SerializeField] private float maxX = 63f;
    [SerializeField] private float minY = 4f;
    [SerializeField] private float maxY = 56f;

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
            moveY = 1f;
        if (Input.GetKey(KeyCode.S))
            moveY = -1f;
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        Vector3 moveDir = new Vector3(moveX, moveY, 0f).normalized;
        Vector3 newPos = transform.position + moveDir * moveSpeed * Time.deltaTime;

        
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;
    }
}
