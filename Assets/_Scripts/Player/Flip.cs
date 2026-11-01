using UnityEngine;

public class Flip : MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody;
    private bool _facingRight = true;

    void Update()
    {
        FlipSelf();
    }

    public void FlipSelf()
    {
        if (_facingRight && PlayerRigidbody.linearVelocityX < 0f || !_facingRight && PlayerRigidbody.linearVelocityX > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

}
