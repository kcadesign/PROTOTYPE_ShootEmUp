using UnityEngine;

public class Move : MonoBehaviour
{
    public bool CanMove;
    public float Speed = 5f;
    public bool MoveX = false;
    public bool MoveY = false;
    public bool InvertMove = false;
    private Vector2 _moveDirection = Vector2.zero;

    private void Start()
    {
        if (MoveX) _moveDirection.x = 1f;
        if (MoveY) _moveDirection.y = 1f;
        if (InvertMove) _moveDirection *= -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove) return;
        gameObject.transform.Translate(_moveDirection * Speed * Time.deltaTime);




    }

    public void SetCanMove(bool canMove)
    {
        CanMove = canMove;
    }
}
