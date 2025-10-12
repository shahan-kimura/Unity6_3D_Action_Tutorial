using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//参考にしやすくするため、InputSystemTutorialとできるだけ変数名等を同一に作成します

public class Player : MonoBehaviour
{
    //移動速度
    [SerializeField]
    private float moveSpeed = 5f;
    // ジャンプ力
    [SerializeField]
    private Vector3 jumpForce = new(0, 5f, 0);

    // Move アクションの入力値[-1.0, 1.0f]
    Vector2 moveInput = Vector2.zero;

    // コンポーネントを事前に参照しておく変数
    new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var speed = Vector3.zero;
        speed = new Vector3(moveInput.x, 0, moveInput.y);
        Move(speed);
    }

    // 指定した速度で、このキャラクターを移動させます。
    public void Move(Vector3 normalizedSpeed)
    {
        // 等速度運動
        var velocity = rigidbody.linearVelocity;
        velocity.x = normalizedSpeed.x * moveSpeed;
        velocity.z = normalizedSpeed.z * moveSpeed;
        rigidbody.linearVelocity = velocity;
    }

    // このキャラクターをジャンプさせます。
    public void Jump()
    {
        rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        Debug.Log("jump");
    }

    // Move アクションによって呼び出されます。
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Jump アクションによって呼び出されます。
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
    }
}
