using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer frightened_blue;
    public SpriteRenderer frightened_blueandwhite;

    public bool eaten {get; private set;}

    public override void Enable(float duration)
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.eyes.enabled = false;
        this.frightened_blue.enabled = true;
        this.frightened_blueandwhite.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    private void Flash()
    {
        if (!this.eaten)
        {
            this.frightened_blue.enabled = false;
            this.frightened_blueandwhite.enabled = true;  
            this.frightened_blueandwhite.GetComponent<AnimatedSprite>().Restart();

        }
    }

    private void Eaten()
    {
        this.eaten = true;

        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.home.inside.position.z;

        this.ghost.transform.position = position;
        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.frightened_blue.enabled = false;
        this.frightened_blueandwhite.enabled = false;
    }

    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.eyes.enabled = true;
        this.frightened_blue.enabled = false;
        this.frightened_blueandwhite.enabled = false;
    }

    private void OnEnable()
    {
        this.ghost.movement.speedMultiplier = 0.5f;
    }

    private void OnDisable() 
    {
        this.ghost.movement.speedMultiplier = 1.0f;
        ghost.scatter.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if(this.enabled){ Eaten();}
        }
    }

     private void OnTriggerEnter2D(Collider2D other) 
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach(Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance  > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            this.ghost.movement.SetDirection(direction);
        }
    }

}
