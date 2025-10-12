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

    void FixedUpdate()
    {
        // Rigidbodyでの移動になるため、FixedUpdateに変更しMove関数を呼び出すだけに
        Move();
    }

    // 指定した速度で、このキャラクターを移動させます。
    public void Move()
    {
        if (Camera.main != null)
        {
            // メインカメラの前方と右方向を取得（カメラローカル座標でいうところのz軸とx軸）
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // カメラのy軸方向を無視して、地面に沿った移動にする
            cameraForward.y = 0;
            cameraRight.y = 0;

            // 正規化して、カメラの前方と右方向に基づいた移動ベクトルを計算
            Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

            // moveInputは2Dベクトルで、プレイヤーの移動入力を表します。
            // moveInput.x: 左右の移動入力（-1.0f は左、1.0f は右）
            // moveInput.y: 前後の移動入力（-1.0f は後退、1.0f は前進）
            // 注意: このmoveInput.yは、ジョイスティックやキーボード入力の前後の動きであり、
            //       3D空間のY軸（上下方向）とは異なります。
            //       3D空間のY軸は、物理的な上下移動（ジャンプや落下など）を示します。

            // 移動ベクトルに速度を掛けて移動
            rigidbody.linearVelocity = moveDirection * moveSpeed + new Vector3(0, rigidbody.linearVelocity.y, 0);
        }
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
