
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    // the event need to be public

    // method one : 
    // public static event EventHandler OnDied;

    // method two :
    public delegate void DeadEventHandler();
    public static event DeadEventHandler OnDied;
    public delegate void StartEventHandler();
    public static event StartEventHandler OnStart;

    private Transform bird;
    public float JUMP_AMOUNT = 50f;
    private Rigidbody2D birdRigidBody2D;

    private State state;

    private enum State
    {
        WaitingToStart,
        Playing,
        Dead,
    }


    //[System.Obsolete]
    //public Bird(Transform bird)
    //{
    //    this.bird = bird;
    //}

    /**
     * cannot use constructor method because monobehaviour cannot be called using new keyword 
     * and when using AddComponent method, no arguments is passed 
     * Hence, create this static factory method 
     */
    public static Bird CreateBird(GameObject where, Transform bird)
    {
        Bird temp = where.AddComponent<Bird>();
        temp.bird = bird;
        return temp;
    }

    private void Awake()
    {
        birdRigidBody2D = GetComponent<Rigidbody2D>();
        state = State.WaitingToStart;
        ChangeRigidBodyType(state);
    }

    private void ChangeRigidBodyType(State state)
    {
        switch (state)
        {
            case State.WaitingToStart:
            case State.Dead:
                birdRigidBody2D.bodyType = RigidbodyType2D.Static;
                break;
            case State.Playing:
                birdRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                break;
            default:
                throw new Exception("no such bird state");
        }
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                {
                    state = State.Playing;
                    ChangeRigidBodyType(state);
                    OnStart?.Invoke();
                    Jump();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                    Jump();
                break;
            case State.Dead:
                break;
        }

    }

    private void Jump()
    {
        if (state == State.Dead)
            return;
        birdRigidBody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // CMDebug.TextPopupMouse("bird dead");
        // OnDied?.Invoke(this, EventArgs.Empty); // indicate no event arguments is passed

        Debug.Log(collision.name);
        state = State.Dead;
        ChangeRigidBodyType(state);
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        OnDied?.Invoke();
    }

    private void DestroySelf()
    {
        Destroy(bird.gameObject);
    }
}
