using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    public static MomentumManager instance;

    [SerializeField]
    private Transform trackedTransform;
    private Vector3 lastPoint;

    private Queue<Vector3> characterPositions = new Queue<Vector3>();
    
    // Using a reference for the amount of queue insertions prevents us from having to look up the O(n) count every frame.
    private int queueInsertions = 0;

    void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        
        else 
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.moveState == PlayerMoveState.CLIMBING)
        {
            UpdateVectorQueue(trackedTransform.position);
        }
    }

    private void UpdateVectorQueue(Vector3 v)
    {
        characterPositions.Enqueue(v);

        if (++queueInsertions > 20)
        {
            lastPoint = characterPositions.Dequeue();
        }
    }

    public Vector3 CalculateMomentumFromPositions()
    {
        Vector3[] testArray = characterPositions.ToArray();

        Vector3 startPoint = testArray[0];
        lastPoint = testArray[testArray.Length - 1];
        if (lastPoint == null || lastPoint == Vector3.zero)
        {
            return Vector3.zero;
        }

        float distance = Vector3.Distance(startPoint, lastPoint) * 20f;
        if (distance > 20f) {
            Vector3 additionalVelocity = (startPoint - lastPoint) * distance;
            Debug.Log("Add this much upward velocity! " + additionalVelocity);
            return additionalVelocity;
        }

        else {
            Debug.Log("Distance seems kinda low, what's the distance? " + distance);
            Debug.Log("Start Point: " + startPoint);
            Debug.Log("End Point: " + lastPoint);
        }

        return Vector3.zero;
    }

    public void ResetQueue()
    {
        characterPositions = new Queue<Vector3>();
        lastPoint = Vector3.zero;
        queueInsertions = 0;
    }
}
