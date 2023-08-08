using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance = null;
    private static readonly Queue<Action> actionQueue = new Queue<Action>();



    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            //Debug.Log("Not present in scene");
        }
        return instance;
    }

    private void Awake()
    {
        //Debug.Log("UnityMainThreadDispatcher instance created.");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        while (actionQueue.Count > 0)
        {
            Action action;
            lock (actionQueue)
            {
                action = actionQueue.Dequeue();
            }
            action.Invoke();
        }
    }

    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }
}