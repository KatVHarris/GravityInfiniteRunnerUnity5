﻿using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class PlayerHealthController : MonoBehaviour, IDamagable<int>
{
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public Image damageImageTop;
    public Image damageImageLeft;
    public Image damageImageRight; 
//	public AudioClip deathClip;     //Implement Later           // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    public Text healthIntText; 

    public static bool instantDeath = false; 
	Animator anim;
    // Reference to the Animator component.
    //	AudioSource playerAudio;                                    // Reference to the AudioSource component.

    //	PlayerMovement playerMovement; //Code disabling movement    // Reference to the player's movement.
    //	PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.


    FirstPersonCharacter firstPersonCharacter; 
    bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.
	
	
	void Start ()
	{
		// Setting up the references.
		anim = GetComponent <Animator> ();
        
//		playerAudio = GetComponent <AudioSource> ();
//		playerMovement = GetComponent <PlayerMovement> ();
//		playerShooting = GetComponentInChildren <PlayerShooting> ();
		
		// Set the initial health of the player.
		currentHealth = startingHealth;
	}
	
	
	void Update ()
	{
		// If the player has just been damaged...
		if(damaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			/*damageImage.color = flashColour;
            damageImageTop.color = flashColour;
            damageImageLeft.color = flashColour;
            damageImageRight.color = flashColour; */
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			/*damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            damageImageTop.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            damageImageLeft.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            damageImageRight.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);*/
        }
		
		// Reset the damaged flag.
		damaged = false;
        healthIntText.text = currentHealth.ToString(); 
	}
	
	
	public void TakeDamage (int amount)
	{
		// Set the damaged flag so the screen will flash.
		damaged = true;
		
		// Reduce the current health by the damage amount.
		currentHealth -= amount;
		
		// Set the health bar's value to the current health.
		healthSlider.value = currentHealth;
		
		// Play the hurt sound effect.
		//playerAudio.Play ();
		
		// If the player has lost all it's health and the death flag hasn't been set yet...
		if(currentHealth <= 0 && !isDead)
		{
			// ... it should die.
			Death ();
		}
	}
	
	
	public void Death ()
	{
        // Set the death flag so this function won't be called again.
        currentHealth = 0; 
		isDead = true;

    }      
}