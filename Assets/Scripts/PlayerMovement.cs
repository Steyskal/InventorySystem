using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed = 5.0f;

    private float _moveH;
    private float _moveV;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

	void Awake ()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _moveH = 0.0f;
        _moveV = 0.0f;
	}

	void Update ()
    {
        _moveH = Input.GetAxisRaw("Horizontal");
        _moveV = Input.GetAxisRaw("Vertical");
	}

    void FixedUpdate()
    {
        Move();
        Animate();
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(_moveH, _moveV, 0.0f);
        _rigidbody2D.velocity = movementDirection * movementSpeed;
    }

    private void Animate()
    {
        _animator.SetFloat("MoveH", _moveH);
        _animator.SetFloat("MoveV", _moveV);
    }
}
