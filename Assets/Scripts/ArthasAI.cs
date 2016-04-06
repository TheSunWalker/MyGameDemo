using UnityEngine;
using System.Collections;

public enum State
{
    Stand, Move
}

public class ArthasAI : MonoBehaviour
{
    public GameObject Sword;
    public Transform BackParent;
    public Transform HandParent;

    private Animator AnimCtrl;
    private int MoveSpeed;
    private bool bWalk;
    public float RotateSpeed = 15;
    public float WalkSpeed = 1;
    public State mState = State.Stand;

    void Start()
    {
        AnimCtrl = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MoveControl();
        AnimControl();
    }

    void AnimControl()
    {
        switch (mState)
        {
            case State.Stand:
                if (AnimCtrl.GetBool("Move"))
                    AnimCtrl.SetBool("Move", false);
                break;
            case State.Move:
                AnimCtrl.SetBool("Move", true);
                break;
        }
    }

    void MoveControl()
    {
        //Change between walk and run
        if (Input.GetKeyUp(KeyCode.KeypadDivide) || Input.GetButtonDown("ChangeMoveType"))
        {
            bWalk = !bWalk;
        }

        float delta = 0;
        //Change movespeed
        if (Input.GetKey(KeyCode.W))
        {
            mState = State.Move;
            MoveSpeed = bWalk ? 1 : 2;
            delta = MoveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mState = State.Move;
            MoveSpeed = -1;
            delta = -1;
        }
        //Joystick hang on
        //delta += Input.GetAxis("Joystick Vertical");
        if (delta < 0)
            MoveSpeed = -1;
        else if (delta > 0)
            MoveSpeed = bWalk ? 1 : 4;
        else
        {
            MoveSpeed = 0;
            mState = State.Stand;
        }
        AnimCtrl.SetFloat("Speed", MoveSpeed);
        FixPosition(MoveSpeed);

        RotateControl();
    }

    void RotateControl()
    {
        float delta = 0;
        if (Input.GetKey(KeyCode.A))
            delta = -1;
        if (Input.GetKey(KeyCode.D))
            delta = 1;
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            delta = Input.GetAxis("Mouse X");
        }
        else
            Cursor.visible = true;
        delta += Input.GetAxis("Joystick Horizontal");

        if (delta != 0)
            FixRotation(delta);
    }

    void FixPosition(float delta)
    {
        if (mState == State.Stand)
            return;
        transform.position += transform.forward * WalkSpeed * Time.deltaTime * delta;
        MoveLimit();
    }

    void MoveLimit()
    {
        Vector3 delta = transform.position;
        if (delta.z <= 0f)
            delta.z = 0f;
        if (delta.z >= 494)
            delta.z = 494;
        if (delta.x <= -189f)
            delta.x = 189f;
        if (delta.x >= 308)
            delta.x = 308;
        transform.position = delta;
    }

    void FixRotation(float delta)
    {
        Vector3 euler = transform.localRotation.eulerAngles;
        euler += new Vector3(0, delta * RotateSpeed * Time.deltaTime, 0);
        transform.localRotation = Quaternion.Euler(euler);
    }
}
