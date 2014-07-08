using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public bool onGround, gliding, dashing, glideAllowDeFace, allowBoost, allowInput, allowCameraFollowing;
	public float moveSpeed, moveAccel, moveDecel, jumpStrength, boostStrength, airFriction, deadZone;
	public float dashLength, dashSpeed, glideControl, glideSteepness, glideDeFaceThreshold, glideGravityResistance, glideHitWallPenalty;
	
	float dashTimer, glideZeroAcc, glideHitWallTimer, idleTimer;
	Vector3 faceDirection, glideDefaceDirection;
	Rigidbody2D rigid;
	Vector2 padInput;
	Animator ator;
	Transform animT;
	Stamina stamina;
	CameraFollowing camFollow;
	GameObject lastCheckpoint;

    //audio
    SoundHandler soundHandler;
    bool soundDelay;

	ParticleGenerator doubleJumpParticleGen, dustParticleGen, dashParticleGen;
	//public GameObject parentObject;
	//public Vector3 offsetFromPlatform;
	//GameObject playerFlowerCollider;

	void Start ()
	{
		rigid = transform.rigidbody2D;
		animT = transform.FindChild("Animator");
		ator = animT.GetComponent<Animator>();
		stamina = GetComponent<Stamina>();
		camFollow = GetComponent<CameraFollowing>();
		doubleJumpParticleGen = GameObject.Find("Double Jump Particle Generator").GetComponent<ParticleGenerator>();
		dustParticleGen = GameObject.Find("Dust Particle Generator").GetComponent<ParticleGenerator>();
		dashParticleGen = GameObject.Find("Dash Particle Generator").GetComponent<ParticleGenerator>();
        soundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();

		animT.localPosition = new Vector3(-0.18f, 0.04f, 1.0f);
		padInput = Vector2.zero;
		onGround = false;
		idleTimer = 0.0f;


		//basic
		moveAccel = 35.0f; //movement accel. and decel.
		moveDecel = 40.0f;
		moveSpeed = 4.0f; //max move speed
		jumpStrength = 5.8f;
		airFriction = 9.0f; //velocity.x slow down in air
		deadZone = 0.3f;
		allowInput = true;
		allowCameraFollowing = true;


		//while gliding
		gliding = false; //gliding state
		glideDefaceDirection = Vector3.zero;
		glideDeFaceThreshold = 3.99f;		//direction can't be changed when (-glideDeFaceThreshold < velocity.x < glideDeFaceThreshold)
		glideAllowDeFace = true;			//allow changing direction
		glideSteepness = 0.25f;				//how much the player descends while gliding
		glideControl = 8.0f;				//how much the gliding speed can be affected by input
		glideZeroAcc = 6.0f;				//acceleration from zero velocity
		glideGravityResistance = 15.0f;		//how much gravity resistance when starting to glide while falling
		glideHitWallTimer = 0.0f;
		glideHitWallPenalty = 0.6f;


		//dashing
		dashing = false;
		dashTimer = 0.0f;
		dashLength = 0.3f;
		dashSpeed = 8.0f;
		faceDirection = new Vector3(1.0f, 0.0f, 0.0f);


		//boost
		allowBoost = true; //clears after boosting, sets when touching ground or ?
		boostStrength = 5.5f;
	}

	void Update()
	{
		if (allowInput)
			padInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		else
			padInput = Vector2.zero;
		//GameObject.Find("UI/Debug Text/Label").GetComponent<UILabel>().text = "velocity: " + rigid.velocity + "\ndeface: " + glideAllowDeFace + "\ndefaceDir: " + glideDefaceDirection + "\nfaceDirection: " + faceDirection;

		float colliderWidth = gameObject.GetComponent<BoxCollider2D>().size.x; //startiin?
		float colliderHeight = gameObject.GetComponent<BoxCollider2D>().size.y;

		//fix slope sliding
		if (onGround)
		{
			rigid.gravityScale = 0.0f;
		}
		else
		{
			rigid.gravityScale = 1.0f;
		}

		//animation
		Animation();

        //sounds
        if (onGround)
        {
            if (rigid.velocity.x != 0)
            {
                if (!soundDelay)
                {
                    soundHandler.PlaySound("movement");
                    soundDelay = true;
                    Invoke ("ResetSoundDelay", 0.4f);
                }
            }
        }

		//glide
		if (Input.GetButton("Glide") && allowInput)
		{
			if (!onGround && !dashing && rigid.velocity.y < 0.0f && glideHitWallTimer == 0.0f)
			{
				if (stamina.Remaining())
				{
					gliding = true;
				}
			}
		}
		else
		{
			gliding = false;
			glideAllowDeFace = true;
		}
		
		//oneway
		OneWayPlatformCollision(colliderWidth, colliderHeight);

		//stop gliding if we slow down or hit a wall
		GlideWallInteract(colliderWidth, colliderHeight);

		//movement
		if (dashing && !gliding) //dash movement
		{
			MovementDash();
		}
		else if (gliding && !dashing) //glide movement
		{
			MovementGlide();
		}
		else //normal movement
		{
			MovementNormal();
		}

		//jump and boost
		if (Input.GetButtonDown("Jump") && allowInput)
		{
			if (onGround) //normal jump
			{
				ator.SetTrigger("jump");
				Jump();
			}
			else if (allowBoost &&/*!gliding &&*/ !dashing && !onGround)
			{
				if (stamina.Use())
				{
					ator.SetTrigger("doubleJump");
					doubleJumpParticleGen.Trigger();
					if (rigid.velocity.y > 0.0f) //boost jump
					{
						JumpBoost();
					}
					else //weaker boost
					{
						JumpBoostWeak();
					}
				}
			}
		}

		if (gliding && !onGround)
		{
			if (stamina.GetRegen())
			{
				stamina.Use();
				stamina.SetRegen(false);
				
			}
		}
		else
		{
			if (!stamina.GetRegen())
			{
				stamina.SetRegen(true);
			}
		}

		//dash
		if (Input.GetButton("Dash") && allowInput)
		{
			if (onGround && !dashing && stamina.Use())
			{
				dashParticleGen.Trigger();
				dashing = true;
				dashTimer = dashLength;
			}
		}

		//limit speed
		float maxX = 10.0f;
		float maxY = 10.0f;
		rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -maxX, maxX), Mathf.Clamp(rigid.velocity.y, -maxY, maxY), 0.0f);

		//for debugging only
		//free movement
		if (Input.GetKey(KeyCode.Keypad8))
		{
			rigid.velocity = new Vector2(0.0f, 15.0f);
		}
		if (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(KeyCode.Keypad2))
		{
			rigid.velocity = new Vector2(0.0f, -15.0f);
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			rigid.velocity = new Vector2(-15.0f, 0.0f);
		}
		if (Input.GetKey(KeyCode.Keypad6))
		{
			rigid.velocity = new Vector2(15.0f, 0.0f);
		}

		TerrainCollision(colliderWidth, colliderHeight);
		CheckIllegalPosition();

		//camera following and limiting
		if (allowCameraFollowing)
			camFollow.UpdateCameraPosition();
	}

	public void GotoLastCheckpoint()
	{
		if (lastCheckpoint)
		{
			transform.position = lastCheckpoint.transform.position;
			rigid.velocity = Vector3.zero;
		}
		else
		{
			GotoPlayerStart();
		}
	}

	public void SetCurrentCheckpoint(GameObject obj)
	{
		if (obj)
		{
			lastCheckpoint = obj;
		}
	}

	void OneWayPlatformCollision(float colliderWidth, float colliderHeight)
	{
		Vector3 poos = transform.position - new Vector3(0.0f, colliderHeight / 2.0f);
		string theTag = "One Way";
		for (int tag = 0; tag < 2; tag++)
		{
			GameObject[] plats = GameObject.FindGameObjectsWithTag(theTag);
			for (int i = 0; i < plats.Length; i++)
			{
				Vector3 thick = new Vector3(0.0f, plats[i].GetComponent<BoxCollider2D>().size.y / 2.0f);
				if (poos.y < plats[i].transform.position.y + thick.y)
				{
					plats[i].layer = 11; //one way
				}
				else
				{
					plats[i].layer = 8; //terraincollision
				}
			}
			theTag = "Moving and One Way";
		}
	}

	void Animation()
	{
		if (padInput.x <= deadZone && padInput.x >= -deadZone)
		{
			//idle
			ator.SetBool("running", false);
		}
		else if (!dashing)
		{
			//run
			ator.SetBool("running", true);

			//facing direction
			if (rigid.velocity.x > deadZone)
			{
				faceDirection = new Vector3(1.0f, 0.0f, 0.0f);
			}
			else if (rigid.velocity.x < -deadZone)
			{
				faceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
			}
		}

		if (faceDirection.x > 0.0f)
		{
			animT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			animT.localPosition = new Vector3(-0.18f, 0.04f, 1.0f);
		}
		else if (faceDirection.x < 0.0f)
		{
			animT.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			animT.localPosition = new Vector3(0.18f, 0.04f, 1.0f);
		}

		ator.SetBool("dashing", dashing);
		ator.SetBool("onGround", onGround);
		ator.SetBool("gliding", gliding);

		//idle alternating
		if (idleTimer <= 0.0f)
		{
			ator.SetTrigger("altIdle");
			idleTimer = Random.value * 1.65f + 1.8f;
		}
		else
		{
			idleTimer -= Time.deltaTime;
		}
	}

	public void Kill()
	{
		Debug.Log("Kill");
		//rigid.isKinematic = true;
		allowInput = false;
		rigid.velocity = Vector3.zero;

		//turn player invisible
		SpriteRenderer spr = GameObject.Find("Player/Animator").GetComponent<SpriteRenderer>();
		Color col = spr.color;
		col.a = 0.0f;
		spr.color = col;
	}
	public void Resurrect()
	{
		Debug.Log("Resurrect");
		if (!allowInput)
		{
            stamina.ResetStamina();
			allowInput = true;
			//rigid.isKinematic = false;

			//turn player visible
			SpriteRenderer spr = GameObject.Find("Player/Animator").GetComponent<SpriteRenderer>();
			Color col = spr.color;
			col.a = 1.0f;
			spr.color = col;
		}
	}

	void CheckIllegalPosition()
	{
		float x = transform.position.x;
		float y = transform.position.y;
		if (y < -15.0f)
		{
			GotoPlayerStart();
		}
	}

	void GotoPlayerStart()
	{
		GameObject start = GameObject.Find("Player Start");

		if (start != null)
		{
			transform.position = start.transform.position;
		}
		else
		{
			transform.position = Vector3.zero;
		}
		rigid.velocity = Vector3.zero;
	}

	void MovementNormal()
	{
		if (padInput.x < -deadZone)
		{
			rigid.velocity -= new Vector2(moveAccel * Time.deltaTime, 0.0f);
		}
		else if (padInput.x > deadZone)
		{
			rigid.velocity += new Vector2(moveAccel * Time.deltaTime, 0.0f);
		}
		else
		{
			if (onGround)
			{
				if (rigid.velocity.x > 0.0f)
				{
					rigid.velocity -= new Vector2(moveDecel * Time.deltaTime, 0.0f);

					rigid.velocity = new Vector2(Mathf.Max(0.0f, rigid.velocity.x), rigid.velocity.y);
				}
				else if (rigid.velocity.x < 0.0f)
				{
					rigid.velocity += new Vector2(moveDecel * Time.deltaTime, 0.0f);

					rigid.velocity = new Vector2(Mathf.Min(0.0f, rigid.velocity.x), rigid.velocity.y);
				}
			}
			else
			{
				if (rigid.velocity.x > 0.0f)
				{
					rigid.velocity -= new Vector2(airFriction * Time.deltaTime, 0.0f);

					rigid.velocity = new Vector2(Mathf.Max(0.0f, rigid.velocity.x), rigid.velocity.y);
				}
				else if (rigid.velocity.x < 0.0f)
				{
					rigid.velocity += new Vector2(airFriction * Time.deltaTime, 0.0f);

					rigid.velocity = new Vector2(Mathf.Min(0.0f, rigid.velocity.x), rigid.velocity.y);
				}
			}
		}

		float maxSpeed = moveSpeed;
		rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y, 0.0f);
	}

	void MovementDash()
	{
		rigid.velocity = new Vector3(faceDirection.x * dashSpeed, rigid.velocity.y, 0.0f);

		if (dashTimer > 0.0f)
		{
			dashTimer -= Time.deltaTime;
		}
		if (dashTimer <= 0.0f)
		{
			dashing = false;
			dashTimer = 0.0f;
			rigid.velocity = new Vector3(0.0f, rigid.velocity.y, 0.0f);
		}
	}

	void MovementGlide()
	{
		float maxSpeed = moveSpeed;

		//if falling, slow down the falling
		if (rigid.velocity.y < -glideSteepness)
		{
			rigid.velocity += new Vector2(0.0f, glideGravityResistance * Time.deltaTime);
		}

		else if (rigid.velocity.y < 0.0f)
		{
			rigid.velocity = new Vector3(rigid.velocity.x, -glideSteepness);
		}

		if (padInput.x < -0.5f)
		{
			if (glideAllowDeFace) //allowed to turn around
			{
				glideDefaceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
				glideAllowDeFace = false;
			}
		}
		else if (padInput.x > 0.5f)
		{
			if (glideAllowDeFace) //allowed to turn around
			{
				glideDefaceDirection = new Vector3(1.0f, 0.0f, 0.0f);
				glideAllowDeFace = false;

			}
		}
		else
		{
			//minimum gliding speed
			if (glideAllowDeFace)
			{
				if (faceDirection.x > 0.0f)
				{
					if (rigid.velocity.x < maxSpeed) //glideMinSpeed
					{
						rigid.velocity += new Vector2(glideZeroAcc * Time.deltaTime, 0.0f);
					}
				}
				else
				{
					if (rigid.velocity.x > -maxSpeed) //glideMinSpeed
					{
						rigid.velocity += new Vector2(-glideZeroAcc * Time.deltaTime, 0.0f);
					}
				}
			}
		}

		//gliding threshold

		if (!glideAllowDeFace && glideDefaceDirection.x < 0.0f)
		{
			rigid.velocity += new Vector2(-glideControl * Time.deltaTime, 0.0f);
			if (rigid.velocity.x < 0.0f)
			{
				faceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
			}
			if (rigid.velocity.x < -glideDeFaceThreshold)
			{
				glideAllowDeFace = true;
				glideDefaceDirection = new Vector3(1.0f, 0.0f, 0.0f);
			}
		}
		else if (!glideAllowDeFace && glideDefaceDirection.x > 0.0f)
		{
			rigid.velocity += new Vector2(glideControl * Time.deltaTime, 0.0f);
			if (rigid.velocity.x > 0.0f)
			{
				faceDirection = new Vector3(1.0f, 0.0f, 0.0f);
			}
			if (rigid.velocity.x > glideDeFaceThreshold)
			{
				glideAllowDeFace = true;
				glideDefaceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
			}
		}		

		//limit gliding to normal speed
		rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y, 0.0f);
	}

	void GlideWallInteract(float colliderWidth, float colliderHeight)
	{
		/*bool tests = false;
		if (GameObject.Find("Test Point(Clone)") == null)
		{
			tests = true;
		}*/

		Vector3 point = transform.position + new Vector3(colliderWidth / 2.0f + 0.005f, 0.0f);

		if (faceDirection.x < 0.0f)
		{
			point -= new Vector3(colliderWidth - 0.01f, 0.0f);
		}
		float gap = colliderHeight / 2.0f;
		
		for (float offset = -gap; offset <= gap; offset += gap)
		{
			Vector3 pos = point + new Vector3(0.0f, offset);

			/*if (tests)
			{
				GameObject tp = Instantiate(Resources.Load("Test Point", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
				tp.transform.parent = this.transform;
			}*/

			if (Raycast(pos, faceDirection, 0.03f) != -1)
			{
				if (gliding)
				{
					glideHitWallTimer = glideHitWallPenalty;
				}
				gliding = false;
			}
		}


		if (glideHitWallTimer > 0.0f)
		{
			glideHitWallTimer -= Time.deltaTime;
			if (glideHitWallTimer < 0.0f)
			{
				glideHitWallTimer = 0.0f;
			}
		}

		if (rigid.velocity.x < 0.1f && rigid.velocity.x > -0.1f && glideAllowDeFace)
		{
			//Debug.Log("asd");
			gliding = false;
			glideAllowDeFace = true;
		}
	}

	void JumpBoost()
	{
		
		rigid.velocity = new Vector2(rigid.velocity.x, boostStrength);
		allowBoost = false;
	}

	void JumpBoostWeak()
	{
		rigid.velocity += new Vector2(0.0f, boostStrength * 0.8f);
		allowBoost = false;
	}

	void Jump()
	{
		rigid.velocity = new Vector2(rigid.velocity.x, jumpStrength);
		onGround = false;

        soundHandler.PlaySound("movement");
	}

	void TerrainCollision(float colliderWidth, float colliderHeight)
	{
		Vector3 bottomPoint = transform.position + new Vector3(0.0f, -colliderHeight / 2.0f - 0.005f);
		float gap = colliderWidth / 2.0f;

		bool onGroundBefore = onGround;

		//Debug.Log("landcheck");
		onGround = false;
		for (float offset = -gap; offset <= gap; offset += gap)
		{
			//Debug.Log("offset " + offset);
			int result = Raycast(bottomPoint + new Vector3(offset, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), 0.03f);
			if (result != -1)
			{
				//touching ground
				onGround = true;
				gliding = false;
				allowBoost = true;
				glideHitWallTimer = 0.0f;
			}
		}

		if (onGroundBefore == false && onGround == true)
		{
			dustParticleGen.Trigger();
            soundHandler.PlaySound("movement");
		}
		//Debug.Log("landcheck end");
	}

	int Raycast(Vector3 pos, Vector3 direction, float length) //returns collider's layer if collision occurs, -1 when no collision occurs
	{
		//user layer 8  is terraincollision
		//user layer 12 is slope
		int layer = (1 << 8) | (1 << 12);

		RaycastHit2D hit = Physics2D.Raycast(pos, direction, length, layer);

		if (hit != null)
		{
			if (hit.collider != null)
			{
				return hit.collider.gameObject.layer;
			}
		}
		return -1;
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Dog")
        {

                if (col.gameObject.GetComponent<DogAI>().Alerted || col.gameObject.GetComponent<DogAI>().ReturnIfPlayerInsideEnemyFOV())
                {
                    //Debug.Log("Player is killed in a horrible dogfighting accident");
                    //Vector3 spawn = GameObject.Find("Player Start").transform.position;
                    //transform.position = spawn;
                    rigid.velocity = Vector3.zero;
                }
        }
		
    }

	void OnTriggerEnter2D(Collider2D c)
	{
	}

    void ResetSoundDelay()
    {
        soundDelay = false;
    }
}
