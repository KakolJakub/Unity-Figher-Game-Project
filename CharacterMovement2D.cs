using UnityEngine;
using UnityEngine.Events;

public enum Direction
{
	Left,
	Right
}

public class CharacterMovement2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.1f;	// How much to smooth out the movement
	[SerializeField] private LayerMask whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;							// A position marking where to check if the player is grounded.

	[SerializeField] private Transform rayFirePoint; // A point from which Raycast can be fired.
	
	[SerializeField] private float minDodgePassingDistance; // The minimum distance the player needs to be in when dodging, in order to pass through the other player.
	
	const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D _Rigidbody2D;
	private bool _FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _Velocity = Vector3.zero;
	
	public PlayerStats playerStats;
	
	public Animator animate;
	
	private int dodgeAmount;
	private int currentDodgeAmount;
	private float dodgeRegenTime;
	private float currentDodgeRegenTime;
	
	private float dodgeTimer;
	private float currentDodgeTimer;
	private int leftClickAmount;
	private int rightClickAmount;
	
	private Direction dodgeDirection;
	
	private float defaultGravityScale;
	
	
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}
	
	private void Start()
	{
		defaultGravityScale = _Rigidbody2D.gravityScale;
		
		dodgeRegenTime = playerStats.dodgeRegenTime;
		dodgeTimer = playerStats.dodgeTimer;
		dodgeAmount = playerStats.dodgeAmount;
		currentDodgeAmount = dodgeAmount;
	}
	
	private void Update()
	{
		
		//TODO: Dodge method and timer
		if(currentDodgeTimer > 0)
		{
			currentDodgeTimer -= Time.deltaTime;
		}
		if(currentDodgeTimer <= 0)
		{
			currentDodgeTimer = 0;
			leftClickAmount = 0;
			rightClickAmount = 0;
		}
		
		if(currentDodgeAmount >= dodgeAmount)
		{
			currentDodgeAmount = dodgeAmount;
		}
		else
		{
			if(currentDodgeRegenTime > 0)
			{
				currentDodgeRegenTime -= Time.deltaTime;
			}
			if(currentDodgeRegenTime <= 0)
			{
				currentDodgeRegenTime = dodgeRegenTime;
				currentDodgeAmount++;
			}
		}
		
		
		//Debug.Log("currentDodgeTimer: " + currentDodgeTimer+"clicks:"+clickAmount);
	}
	
	private void FixedUpdate()
	{
		bool wasGrounded = _Grounded;
		_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GroundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}
	
	//TESTING ONLY:
	/*private void OnCollisionEnter2D(Collision2D enemy)
	{
		if(playerStats.dodging)
		{
			float addedDistance = 0;
			
			//Debug.Log(enemy.gameObject.name); //Player collides with the platform
			
			if(enemy.gameObject.GetComponent<CircleCollider2D>())
			{
				addedDistance = enemy.gameObject.GetComponent<CircleCollider2D>().radius * 2;
				//ActualDodge(Direction.Right);
			}
			
			_Rigidbody2D.velocity = transform.right * addedDistance; //that's velocity, not distance
			Debug.Log("You moved by additional distance: " + addedDistance);
		}
	}*/
	
	public void MovementAvailable()
	{
		playerStats.canMove = true;
	}
	
	public void Move(Direction direction)
	{

		//only control the player if grounded or airControl is turned on
		if (_Grounded && playerStats.canMove)
		{
			float move;
			float moveSpeed;
			
			if(direction == Direction.Right)
			{
				moveSpeed = playerStats.movementSpeed;
				
			}
			else
			{
				moveSpeed = playerStats.movementSpeed * (-1);
			}
			
			//Get movement value
			move = moveSpeed * Time.fixedDeltaTime;
			//Animate the character
			animate.SetBool("Moving",true);
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, _Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			_Rigidbody2D.velocity = Vector3.SmoothDamp(_Rigidbody2D.velocity, targetVelocity, ref _Velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}
	
	public void DontMove()
	{
		if (_Grounded)
		{
			//Animate the character
			animate.SetBool("Moving", false);
		}
	}
	
	//Used via animation events on knockback animation
	public void SlideBack()
	{
		if(_FacingRight)
		{
			_Rigidbody2D.velocity = Vector2.left * playerStats.knockbackForce;
		}
		if(!_FacingRight)
		{
			_Rigidbody2D.velocity = Vector2.right * playerStats.knockbackForce;
		}
		else
		{
			return;
		}
	}
	
	//Used via animation events on dodge animation
	public void SlideForward()
	{
		if(dodgeDirection == Direction.Right)
		{
			_Rigidbody2D.velocity = Vector2.right * playerStats.dodgeRange;
		}
		else
		{
			_Rigidbody2D.velocity = Vector2.left * playerStats.dodgeRange;
		}
	}

	public void MovePlayerForward(float speed)
	{
		if(!_FacingRight)
		{
			_Rigidbody2D.velocity = Vector2.left * speed;
		}
		if(_FacingRight)
		{
			_Rigidbody2D.velocity = Vector2.right * speed;
		}
		else
		{
			return;
		}
	}
	
	//Used via animation events on knockback and dodge animation
	public void ResetVelocity()
	{
		_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
	}
	
	//Used via animation events on dodge animation
	public void StopDodging()
	{
		DisableCharacterCollision(false);
		playerStats.dodging = false;
		playerStats.canDodge = true;
	}
	
	public void Dodge(Direction direction)
	{
		if(playerStats.canDodge)
		{
			bool canPassThrough;
		
			if(AllowPassingThrough())
			{
				canPassThrough = true;
			}
			else
			{
				canPassThrough = false;
			}
			
			if(currentDodgeAmount == 0)
			{
				return;
			}
		
			if(direction == Direction.Left)
			{
				leftClickAmount++;
			}
			if(direction == Direction.Right)
			{
				rightClickAmount++;
			}
		
			if((leftClickAmount == 1)||(rightClickAmount == 1))
			{
				currentDodgeTimer = dodgeTimer;
			}
		
			if((currentDodgeTimer > 0 && leftClickAmount >= 2) || (currentDodgeTimer > 0 && rightClickAmount >= 2))
			{
				ActualDodge(direction, canPassThrough);
				currentDodgeAmount--;
				leftClickAmount = 0;
				rightClickAmount = 0;
			}
		}
	}
	
	private void ActualDodge(Direction direction, bool canPassThrough)
	{
		playerStats.canMove = false;
		playerStats.canDodge = false;
		playerStats.canAttack = false;
		playerStats.canCastAbility = false;
		
		playerStats.dodging = true;
		
		dodgeDirection = direction;
		
		if(canPassThrough)
		{
			DisableCharacterCollision(true);
		}
		else
		{
			DisableCharacterHitBox();
		}
		
		if((direction == Direction.Left && _FacingRight) || (direction == Direction.Right && !_FacingRight))
		{
			Flip();
		}
		
		animate.SetTrigger("Dodge");
	}
	
	private void DisableCharacterCollision(bool isActive)
	{
		if(isActive)
		{
			_Rigidbody2D.gravityScale = 0;
			GetComponent<CircleCollider2D>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			GetComponent<CircleCollider2D>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;
			_Rigidbody2D.gravityScale = defaultGravityScale;
		}	
	}
	
	private void DisableCharacterHitBox()
	{
		GetComponent<BoxCollider2D>().enabled = false;
	}
	
	private bool AllowPassingThrough()
	{
		bool allow;
		
		RaycastHit2D dodgeInformator = Physics2D.Raycast(rayFirePoint.transform.position, rayFirePoint.transform.right);		
		
		if(dodgeInformator.collider != null)
		{
			Debug.Log(dodgeInformator.collider.gameObject.name);
			
			if(dodgeInformator.distance < minDodgePassingDistance)
			{
				allow = true;
				Debug.Log("If you dodge, you will pass through the enemy.");
			}
			else
			{
				allow = false;
				Debug.Log("If you dodge, you won't pass through the enemy.");
			}
		}
		else
		{
			allow = false;
			Debug.Log("No enemy in sight.");
		}
		
		return allow;
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_FacingRight = !_FacingRight;
		
		//New flip:
		transform.Rotate(0f, 180f, 0f);
		
		//Old flip:
		// Multiply the player's x local scale by -1.
		/*Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;*/
	}
	
	//TESTING ONLY:
	public void TestFlip()
	{
		Flip();
	}
	
	public int GetCurrentDodgeAmount()
	{
		return currentDodgeAmount;
	}
}
