using UnityEngine;
using UnityEngine.Events;

public enum Direction
{
	Left,
	Right
}

public class CharacterMovement2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	
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
	
	private float defaultGravityScale;
	
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}
	
	private void Start()
	{
		defaultGravityScale = m_Rigidbody2D.gravityScale;
		
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
			currentDodgeTimer-=Time.deltaTime;
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
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}
	
	//TESTING ONLY:
	private void OnCollisionEnter2D(Collision2D enemy)
	{
		if(playerStats.dodging)
		{
			float addedDistance = 0;
			
			//Debug.Log(enemy.gameObject.name); //Player collides with the platform
			
			if(enemy.gameObject.GetComponent<CircleCollider2D>())
			{
				addedDistance = enemy.gameObject.GetComponent<CircleCollider2D>().radius * 2;
				ActualDodge(Direction.Right);
			}
			
			m_Rigidbody2D.velocity = transform.right * addedDistance; //that's velocity, not distance
			Debug.Log("You moved by additional distance: " + addedDistance);
		}
	}
	
	public void MovementAvailable()
	{
		playerStats.canMove=true;
	}
	
	public void Move(Direction direction)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded && playerStats.canMove)
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
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}
	
	public void DontMove()
	{
		if (m_Grounded)
		{
			//Animate the character
			animate.SetBool("Moving", false);
		}
	}
	
	//Used via animation events on knockback animation
	public void SlideBack()
	{
		if(m_FacingRight)
		{
			m_Rigidbody2D.velocity = Vector2.left * playerStats.knockbackForce;
		}
		if(!m_FacingRight)
		{
			m_Rigidbody2D.velocity = Vector2.right * playerStats.knockbackForce;
		}
		else
		{
			return;
		}
	}
	
	public void MovePlayerForward(float speed)
	{
		if(!m_FacingRight)
		{
			m_Rigidbody2D.velocity = Vector2.left * speed;
		}
		if(m_FacingRight)
		{
			m_Rigidbody2D.velocity = Vector2.right * speed;
		}
		else
		{
			return;
		}
	}
	
	//Used via animation events on knockback animation
	public void ResetVelocity()
	{
		m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
	}
	
	public void Dodge(Direction direction) //TODO: Consider making an Enum
	{
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
		if((leftClickAmount==1)||(rightClickAmount==1))
		{
			currentDodgeTimer = dodgeTimer;
		}
		//Debug.Log("clickamount:" +clickAmount);
		if((currentDodgeTimer>0&&leftClickAmount>=2) || (currentDodgeTimer>0&&rightClickAmount>=2))
		{
			ActualDodge(direction);
			currentDodgeAmount--;
			leftClickAmount=0;
			rightClickAmount=0;
		}
	}
	
	private void ActualDodge(Direction direction)
	{
		//TODO: Remove testing methods and create the final ones
		//TODO: Find solution to characters colliding after a dodge (research: overriding what happens on collision, raycasts before dodging)
		
		playerStats.dodging = true;
		DisableCharacterCollision(true);
		//TESTING ONLY:
		Invoke("ResetCharacterCollision", 0.5f);
		//TESTING ONLY:
		Invoke("StopDodging", 0.6f);
		
		if(direction == Direction.Left)
		{
			m_Rigidbody2D.velocity = Vector2.left * playerStats.dodgeRange;
			Invoke("ResetVelocity", 0.1f);
		}
		if(direction == Direction.Right)
		{
			m_Rigidbody2D.velocity = Vector2.right * playerStats.dodgeRange;
			Invoke("ResetVelocity", 0.1f);
		}
		else
		{
			return;
		}
	}
	
	/*
	private void ActualDodge(float move)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 250f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}
	*/
	private void StopDodging()
	{
		playerStats.dodging = false;
	}
	
	private void ResetCharacterCollision()
	{
		DisableCharacterCollision(false);
	}
	
	private void DisableCharacterCollision(bool isActive)
	{
		if(isActive)
		{
			m_Rigidbody2D.gravityScale = 0;
			GetComponent<CircleCollider2D>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			GetComponent<CircleCollider2D>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;
			m_Rigidbody2D.gravityScale = defaultGravityScale;
		}
		
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
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
