using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State
{
    Stand, Move
}

public class ArthasAI : MonoBehaviour
{
    public GameObject Sword;
    public Transform BackParent;
    public Transform HandParent;
    public Transform Arthas;

    private Animator AnimCtrl;
    private int MoveSpeed;
    private bool bWalk = true;
    public float RotateSpeed = 15;
    public float WalkSpeed = 1;
    public State mState = State.Stand;
    private Dictionary<string, int> InputDictionary = new Dictionary<string, int>();//key for keycode, value: 0 for none, 1 for press, 2 for up

    void Start()
    {
        AnimCtrl = GetComponentInChildren<Animator>();
        InputDictionary.Add("0", 0);
        InputDictionary.Add("1", 0);
        InputDictionary.Add("W", 0);
        InputDictionary.Add("A", 0);
        InputDictionary.Add("S", 0);
        InputDictionary.Add("D", 0);
    }

    void Update()
    {
        GetInput();
        MouseControl();
        MoveControl();
        RotateControl();
        AttackControl();
    }

    void GetInput()
    {
        if (Input.GetMouseButton(0))
            InputDictionary["0"] = 1;
        else if (Input.GetMouseButtonUp(0))
            InputDictionary["0"] = 2;

        if (Input.GetMouseButton(1))
            InputDictionary["1"] = 1;
        else if (Input.GetMouseButtonUp(1))
            InputDictionary["1"] = 2;

        CheckMoveInput("w");
        CheckMoveInput("a");
        CheckMoveInput("s");
        CheckMoveInput("d");
    }

    void CheckMoveInput(string keycode)
    {
        if (Input.GetKey(keycode))
            InputDictionary[keycode] = 1;
        else if (Input.GetKeyUp(keycode))
            InputDictionary[keycode] = 2;
    }


    void MouseControl()
    {
        if (InputDictionary["1"] == 1)
        {
            if (InputDictionary["0"] == 1)
            {
                int delta = bWalk ? 1 : 4;
                AnimCtrl.SetInteger("Speed", delta);
                FixVerticalPosition(delta);
            }
            //else
            //{
            //    FixVerticalPosition(0);
            //}
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
            MoveSpeed = bWalk ? 1 : 4;
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
            mState = State.Stand;
            MoveSpeed = 0;
        }
        FixVerticalPosition(MoveSpeed);
    }

    void RotateControl()
    {
        float delta = 0;
        if (Input.GetKey(KeyCode.A))
            delta = -1;
        if (Input.GetKey(KeyCode.D))
            delta = 1;
        if (InputDictionary["1"] == 1)
        {
            Cursor.visible = false;
            delta = Input.GetAxis("Mouse X");
        }
        else
        {
            Cursor.visible = true;
        }
        delta += Input.GetAxis("Joystick Horizontal");

        if (delta != 0)
            FixRotation(delta);
    }

    void FixVerticalPosition(int delta)
    {
        AnimCtrl.SetInteger("Speed", delta);
        if (mState == State.Stand)
            return;
        transform.position += transform.forward * WalkSpeed * Time.deltaTime * delta;
        MoveLimit();
    }

    void FixHorizontalPosition(float delta)
    {
        if (mState == State.Stand)
            return;
        transform.position += transform.right * WalkSpeed * Time.deltaTime * delta;
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

    void AttackControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AnimCtrl.SetInteger("Die", 1);
            AnimCtrl.SetInteger("Die", 2);
        }
    }
}
