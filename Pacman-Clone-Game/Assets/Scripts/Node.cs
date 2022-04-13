using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set; }

    private void Start()
    {
        availableDirections = new List<Vector2>();

        CheckAvaliableDriection(Vector2.up);
        CheckAvaliableDriection(Vector2.down);
        CheckAvaliableDriection(Vector2.left);
        CheckAvaliableDriection(Vector2.right);

    }

    private void CheckAvaliableDriection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);
        if (hit.collider == null)
        {
            availableDirections.Add(direction);
        }
    }
}
