using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{
    CharacterController con;
    Animator anim;

    float normalSpeed = 3f;
    float sprintSpeed = 5f;
    float jump = 8f;
    float gravity = 10f;

    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;

    private void Start()
    {
        con = GetComponent & lt;CharacterController&gt;();
        anim = GetComponent & lt;Animator & gt;();

        // マウスカーソルを非表示にし、位置を固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;
    }

    private void Update()
    {
        // 移動速度を取得
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;

        // カメラの向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 前後左右の入力（WASDキー）から、移動のためのベクトルを計算
        // Input.GetAxis("Vertical")は前後(WSキー)の入力値
        // Input.GetAxis("Horizontal")は左右(ADキー)の入力値
        Vector3 moveZ = cameraForward * Input.GetAxis("Vertical") * speed;
        Vector3 moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed;

        // isGroundedは地面にいるかどうかを判定
        // 地面にいるときはジャンプを可能に
        if(con.isGrounded)
        {
            moveDirection = moveZ + moveX;
            if(Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }
        else
        {
            // 重力を効かせる
            moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // 移動のアニメーション
        anim.SetFloat("MoveSpeed",(moveZ + moveX).magnitude);

        // プレイヤーの向きを入力の向きに変更
        transform.LookAt(transform.position + moveZ + moveX);

        // Moveは指定したベクトルだけ移動させる命令
        con.Move(moveDirection * Time.deltaTime);
    }
}
