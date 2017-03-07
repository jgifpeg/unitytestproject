using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2D;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f/moveTime;
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit){
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null){
			StartCoroutine(SmoothMovement(end));
			return true;
		}
		return false;
	}

	protected IEnumerator SmoothMovement(Vector3 end){
		/*
		Vector3 velocity = Vector3.zero;
		while (transform.position != end){
			transform.position = Vector3.SmoothDamp(transform.position, end, ref velocity, moveTime);
			yield return null;
		}
		*/
		
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		float previousTime = Time.realtimeSinceStartup;
		float currentTime = Time.realtimeSinceStartup;
		float deltaTime = currentTime - previousTime;
		while (sqrRemainingDistance > float.Epsilon){
			currentTime = Time.realtimeSinceStartup;
			deltaTime = currentTime - previousTime;
			previousTime = currentTime;
			
			Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime*deltaTime);
			rb2D.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
		
	}

	protected virtual void AttemptMove<T>(int xDir, int yDir)
		where T:Component{
			RaycastHit2D hit;
			bool canMove = Move(xDir, yDir, out hit);
			if (hit.transform == null)
				return;

			T hitComponent = hit.transform.GetComponent<T>();

			if (!canMove && hitComponent != null){
				OnCantMove(hitComponent);
			}
	}

	protected abstract void OnCantMove<T>(T component)
		where T : Component;
}
