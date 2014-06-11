﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	//Vector3 velocity = Vector3.zero;
	public Vector3 lastPos, faceDirection, glideVelocity, glideFall, glideDefaceDirection;
	public bool onGround, gliding, dashing, glideAllowDeFace, allowBoost;
	public float moveSpeed, dashTimer, dashLength, dashSpeed, glideControl, glideSteepness, glideMinSpeed, glideZeroAcc, glideDeFaceThreshold, glideGravityResistance, boostStrength, jumpStrength, moveAccel, moveDecel, glideHitWallTimer, glideHitWallPenalty;
	Rigidbody2D rigid;
	Vector2 padInput;

	void Start ()
	{
		rigid = transform.rigidbody2D;
		lastPos = Vector3.zero;
		padInput = Vector2.zero;
		onGround = false;
		
		//basic
		moveAccel = 35.0f; //movement accel. and decel.
		moveDecel = 40.0f;
		moveSpeed = 4.0f; //max move speed
		jumpStrength = 7.0f;

		//while gliding
		gliding = false; //gliding state
		glideDefaceDirection = Vector3.zero;
		glideDeFaceThreshold = 3.0f; //direction can't be changed when (-glideDeFaceThreshold < velocity.x < glideDeFaceThreshold)
		glideAllowDeFace = true; //allow changing direction
		glideSteepness = 0.25f; //how much the player descends while gliding
		glideControl = 8.0f; //how much the gliding speed can be affected by input
		glideMinSpeed = 2.0f; //minimum gliding speed (unused?)
		glideZeroAcc = 6.0f; //acceleration from zero velocity
		glideGravityResistance = 15.0f; //how much gravity resistance when starting to glide while falling
		glideHitWallTimer = 0.0f;
		glideHitWallPenalty = 0.6f; //


		//dashing
		dashing = false;
		dashTimer = 0.0f;
		dashLength = 0.15f;
		dashSpeed = 10.0f;
		faceDirection = new Vector3(1.0f, 0.0f, 0.0f);


		//boost
		allowBoost = true; //clears after boosting, sets when touching ground or ?
		boostStrength = 6.0f;
	}

	void Update()
	{
		padInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		//Debug.Log("inpu " + padInput);
		//Debug.Log("vel " + rigid.velocity);

		//facing direction
		if (rigid.velocity.x > 0.0f)
		{
			faceDirection = new Vector3(1.0f, 0.0f, 0.0f);
		}
		else if (rigid.velocity.x < 0.0f)
		{
			faceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
		}

		//stop gliding if we slow down or hit a wall
		GlideWallInteract();

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
		if (Input.GetButtonDown("Jump"))
		{
			if (onGround) //normal jump
			{
				Jump();
			}
			else if (allowBoost &&/*!gliding &&*/ !dashing && !onGround)
			{
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

		//glide
		if (Input.GetButton("Glide"))
		{
			if (!onGround && !dashing && rigid.velocity.y < 0.0f && glideHitWallTimer == 0.0f)
			{
				gliding = true;
			}
		}
		else
		{
			gliding = false;
			glideAllowDeFace = true;
		}

		//dash
		if (Input.GetButton("Dash"))
		{
			if (onGround)
			{
				dashing = true;
				dashTimer = dashLength;
			}
		}

		//limit speed
		float maxX = 10.0f;
		float maxY = 10.0f;
		rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -maxX, maxX), Mathf.Clamp(rigid.velocity.y, -maxY, maxY), 0.0f);

		TerrainCollision();
	}

	void MovementNormal()
	{
		if (padInput.x < -0.5f)
		{
			//rigid.velocity = new Vector2(-speed, rigid.velocity.y);
			rigid.velocity -= new Vector2(moveAccel * Time.deltaTime, 0.0f); //alt
		}
		else if (padInput.x > 0.5f)
		{
			//rigid.velocity = new Vector2(speed, rigid.velocity.y);
			rigid.velocity += new Vector2(moveAccel * Time.deltaTime, 0.0f); //alt
		}
		else
		{
			//rigid.velocity = new Vector2(0.0f, rigid.velocity.y);


			if (rigid.velocity.x > 0.0f) //alt
			{
				rigid.velocity -= new Vector2(moveDecel * Time.deltaTime, 0.0f);

				rigid.velocity = new Vector2(Mathf.Max(0.0f, rigid.velocity.x), rigid.velocity.y);
			}
			else if (rigid.velocity.x < 0.0f) //alt
			{
				rigid.velocity += new Vector2(moveDecel * Time.deltaTime, 0.0f);

				rigid.velocity = new Vector2(Mathf.Min(0.0f, rigid.velocity.x), rigid.velocity.y);
			}
		}

		//limit movement speed //alt
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
		//if falling, slow down the falling
		if (rigid.velocity.y < -glideSteepness)
		{
			//float aa = rigid.velocity.y / 10.0f;
			rigid.velocity += new Vector2(0.0f, glideGravityResistance * Time.deltaTime);
		}
		else if (rigid.velocity.y < 0.0f) //0.0f > velY > -glideSteep
		{
			rigid.velocity = new Vector3(rigid.velocity.x, -glideSteepness);
		}

		if (padInput.x < -0.5f)
		{
			if (glideAllowDeFace)
			{
				//faceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
				rigid.velocity += new Vector2(-glideControl * Time.deltaTime, 0.0f);

				if (rigid.velocity.x < glideDeFaceThreshold)
				{
					glideAllowDeFace = false;
					glideDefaceDirection = new Vector3(-1.0f, 0.0f, 0.0f);
				}
			}
		}
		else if (padInput.x > 0.5f)
		{
			if (glideAllowDeFace)
			{
				//faceDirection = new Vector3(1.0f, 0.0f, 0.0f);
				rigid.velocity += new Vector2(glideControl * Time.deltaTime, 0.0f);

				if (rigid.velocity.x > -glideDeFaceThreshold)
				{
					glideAllowDeFace = false;
					glideDefaceDirection = new Vector3(1.0f, 0.0f, 0.0f);
				}
			}
		}
		else
		{
			//minimum gliding speed
			if (faceDirection.x > 0.0f)
			{
				if (rigid.velocity.x < glideMinSpeed)
				{
					rigid.velocity += new Vector2(glideZeroAcc * Time.deltaTime, 0.0f);
				}
			}
			else
			{
				if (rigid.velocity.x > -glideMinSpeed)
				{
					rigid.velocity += new Vector2(-glideZeroAcc * Time.deltaTime, 0.0f);
				}
			}
		}

		//gliding threshold
		if (!glideAllowDeFace)
		{
			if (glideDefaceDirection.x < 0.0f)
			{
				rigid.velocity += new Vector2(-glideControl * Time.deltaTime, 0.0f);
			}
			else if (glideDefaceDirection.x > 0.0f)
			{
				rigid.velocity += new Vector2(glideControl * Time.deltaTime, 0.0f);
			}
		}

		if (rigid.velocity.x > -glideDeFaceThreshold && rigid.velocity.x < glideDeFaceThreshold)
		{
			//glideAllowDeFace = false;
		}
		else
		{
			glideAllowDeFace = true;
		}

		//limit gliding to normal speed
		float maxSpeed = moveSpeed;
		rigid.velocity = new Vector3(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y, 0.0f);
	}

	void GlideWallInteract()
	{
		if (glideAllowDeFace)
		{
			if (rigid.velocity.x < 0.2f && rigid.velocity.x > -0.2f)
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
		
	}

	void JumpBoost()
	{
		rigid.velocity = new Vector2(rigid.velocity.x, boostStrength);
		allowBoost = false;
		Debug.Log("boost");
	}

	void JumpBoostWeak()
	{
		rigid.velocity += new Vector2(0.0f, boostStrength * 0.8f);
		allowBoost = false;
		Debug.Log("boost weak");
	}

	void Jump()
	{
		rigid.velocity = new Vector2(rigid.velocity.x, jumpStrength);
		onGround = false;
	}

	void TerrainCollision()
	{
		float colliderWidth = gameObject.GetComponent<BoxCollider2D>().size.x;
		float colliderHeight = gameObject.GetComponent<BoxCollider2D>().size.y;

		Vector3 bottomPoint = transform.position + new Vector3(0.0f, -colliderHeight / 2.0f - 0.005f);
		float gap = colliderWidth / 2.0f;

		//Debug.Log("landcheck");
		onGround = false;
		for (float offset = -gap; offset <= gap; offset += gap)
		{
			//Debug.Log("offset " + offset);
			if (Raycast(bottomPoint + new Vector3(offset, 0.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), 0.03f))
			{
				//touching ground
				onGround = true;
				gliding = false;
				allowBoost = true;
				glideHitWallTimer = 0.0f;
			}
			/*else
			{
				
			}*/
		}
		//Debug.Log("landcheck end");
	}

	bool Raycast(Vector3 pos, Vector3 direction, float length) //terrain
	{
		//user layer 8 on terraincollision
		RaycastHit2D hit = Physics2D.Raycast(pos, direction, length, 1 << 8);

		if (hit != null)
		{
			if (hit.collider != null)
			{
				return true;
			}
		}
		return false;
	}
}
