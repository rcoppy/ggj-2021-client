using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel; // leantween

public class PlayerLogic : MonoBehaviour
{
	public Vector3 targetPosition;

	public float moveSpeed = 2.0f; // two tiles per second
	public Queue<Vector3> movementQueue = new Queue<Vector3>();


	private bool isMoving = false;
	private LTDescr myTween; 


	public bool getIsMoving()
	{
		return isMoving;
	}

    // Start is called before the first frame update
    void Start()
    {
		targetPosition = transform.position;
    }

	public void setNewTarget(Vector3 finalPosition, Queue<Vector3> queue) {
		targetPosition = finalPosition;
		movementQueue = queue;
	}


    // Update is called once per frame
    void Update()
    {
        if (transform.position != targetPosition && !isMoving)
		{
			isMoving = true;
			LTDescr myTween = tweenToNewPosition(movementQueue.Dequeue());
		}
    }

	LTDescr tweenToNewPosition(Vector3 position)
	{
		return LeanTween.move(gameObject, position, (position - transform.position).magnitude / moveSpeed).setEase(LeanTweenType.easeOutQuad)
						.setOnComplete(() => {
							if (movementQueue.Count > 0)
							{
								myTween = tweenToNewPosition(movementQueue.Dequeue());
							}
							else
							{
								isMoving = false;
							}
						});
	}
}
