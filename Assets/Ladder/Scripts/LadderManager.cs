using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderManager : MonoBehaviour {

    [Header("Основные настройки:")]
    [SerializeField] private AnimHelper playerAnimHelper;
    [SerializeField] private Animator playerAnimator;
	[SerializeField] private Rigidbody2D playerRigidbody; // компонент персонажа
	[SerializeField] private Transform playerCenterPoint; // дочерний пустой объект, который размещается по центру модели/спрайта персонажа
	[SerializeField] private float playerRaySize = 1f; // луч из центра и до точки касания "земли", настраивается визуально
	[Header("Управление персонажем:")]
	[SerializeField] private string verticalAxis = "Vertical";
	[SerializeField] private float speed = 4.5f;
    [SerializeField] private float forceToCenter = 5f;
    [SerializeField] private float impSpeedToCenter = 0.5f;
    [SerializeField] private float downMovingBoost = 1.5f;
	public static bool isLadder { get; private set; } // true - если персонаж на лестнице, можно использовать для переключения анимации
	public static bool isMove { get; private set; } // true - если персонаж движется, находясь на лестнице
	private static LadderManager _internal;
	private bool isTrigger;
	private Bounds ladderBounds;
	private int layerMask;

	void OnDrawGizmosSelected()
	{
		if(playerCenterPoint == null) return;
		Gizmos.color = Color.green;
		Gizmos.DrawRay(playerCenterPoint.position, Vector3.down * playerRaySize);
	}

	void Awake()
	{
		isMove = false;
		isLadder = false;
		layerMask = 1 << playerRigidbody.gameObject.layer | 1 << 2;
		layerMask = ~layerMask;
		_internal = this;
	}

	public static void SetLadderBounds(Bounds bounds)
	{
		_internal.SetLadderBounds_internal(bounds);
	}

	public static void ResetStatus()
	{
		_internal.ResetStatus_internal();
	}

	void SetLadderBounds_internal(Bounds bounds)
	{
		ladderBounds = bounds;
		isTrigger = true;
	}

	void ResetStatus_internal()
	{
		isTrigger = false;
		isMove = false;
	}

	bool IsGround()
	{
		RaycastHit2D hit = Physics2D.Raycast(playerCenterPoint.position, Vector3.down, playerRaySize, layerMask);
		if(hit.collider) return true;
		return false;
	}

	void MoveUp()
	{
        if (!playerAnimHelper.isTurned) return;
		if(playerCenterPoint.position.y > ladderBounds.max.y + (playerRaySize/2))
		{
			UnLock();
			return;
		}

		if(!isLadder) Lock();
		playerRigidbody.transform.Translate(Vector3.up * speed * Time.deltaTime);
		isMove = true;
        playerAnimator.SetBool("ladderDownBool", false);
        playerAnimator.SetBool("ladderUpBool", true);
    }

	void MoveDown()
	{
        if (!playerAnimHelper.isTurned) return;
        if (playerCenterPoint.position.y < ladderBounds.center.y && IsGround())
		{
			UnLock();
			return;
		}

		if(!isLadder) Lock();
        playerRigidbody.transform.Translate(Vector3.down * speed * Time.deltaTime * downMovingBoost);
		isMove = true;
        playerAnimator.SetBool("ladderDownBool", true);
        playerAnimator.SetBool("ladderUpBool", false);
	}

	void Lock()
	{
		isLadder = true;
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.isKinematic = true;
        StartCoroutine(addForceCor(forceToCenter, impSpeedToCenter, ladderBounds.center)); //for smooth climb on ladder
        //playerRigidbody.transform.position = new Vector3(ladderBounds.center.x, playerRigidbody.transform.position.y, playerRigidbody.transform.position.z);
        playerAnimator.SetBool("jumpBool", false);
        playerAnimator.SetBool("isOnLadder", true);
    }
    IEnumerator addForceCor(float force, float impSpeed, Vector3 destination)
    {

        float startTime = Time.time;
        
        while (Mathf.Abs(playerRigidbody.transform.position.x - destination.x) > 0.01f)
        {
            playerRigidbody.transform.position = new Vector3(Mathf.Lerp(playerRigidbody.transform.position.x, destination.x, (Time.time - startTime) / impSpeed), playerRigidbody.transform.position.y, playerRigidbody.transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
    void UnLock()
	{
		isMove = false;
		isLadder = false;
		playerRigidbody.isKinematic = false;
        playerAnimator.SetBool("isOnLadder", false);
	}

	void Update()
	{
		if(!isTrigger) return;

        if (Input.GetAxis(verticalAxis) > 0) MoveUp();
        else if (Input.GetAxis(verticalAxis) < 0) MoveDown();
        else if (Input.GetAxis(verticalAxis) == 0)
        {
            playerAnimator.SetBool("ladderUpBool", false);
            playerAnimator.SetBool("ladderDownBool", false);

            isMove = false;
        }
	}
}
