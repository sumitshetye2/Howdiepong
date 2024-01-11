using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Animator playerAnim;
  Vector3 oldPos;

  public GameObject vfxExplosion;

  // Start is called before the first frame update
  void Start()
  {
    playerAnim = GetComponent<Animator>();
    oldPos = gameObject.transform.parent.transform.position;
  }

  
  // Update is called once per frame
  void Update()
  {
    Vector3 currentPos = gameObject.transform.parent.transform.position;
    if (currentPos.x < (oldPos.x - 0.5))
    {
      playerAnim.SetBool("Stationary", false);
      playerAnim.SetBool("MoveLeft", true);
      Debug.Log("Moving Left");
    }
    else if(currentPos.x > (oldPos.x + 0.5))
    {
      playerAnim.SetBool("Stationary", false);
      playerAnim.SetBool("MoveRight", true);
      Debug.Log("Moving Right");
    }
    else
    {
      playerAnim.SetBool("MoveLeft", false);
      playerAnim.SetBool("MoveRight", false);
      playerAnim.SetBool("Stationary", true);
      Debug.Log("Stationary");
    }

    if (currentPos.y < (oldPos.y - 0.5))
    {
      playerAnim.SetBool("Stationary", false);
      playerAnim.SetBool("MoveDown", true);
      Debug.Log("Moving Left");
    }
    else if (currentPos.y > (oldPos.y + 0.5))
    {
      playerAnim.SetBool("Stationary", false);
      playerAnim.SetBool("MoveUp", true);
      Debug.Log("Moving Right");
    }
    else
    {
      playerAnim.SetBool("MoveDown", false);
      playerAnim.SetBool("MoveDown", false);
      playerAnim.SetBool("Stationary", true);
      Debug.Log("Stationary");
    }

    oldPos = currentPos;
  }

  private void OnCollisionEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Asteroid"))
    {
      PlayVFX();
    }
  }

  void PlayVFX()
  {
    // Check if vfxExplosion is assigned
    if (vfxExplosion != null)
    {
      // Iterate through all child objects of vfxExplosion
      foreach (Transform child in vfxExplosion.transform)
      {
        // Check if the child has a Particle System component
        ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();

        // If the child has a Particle System component, play it
        if (particleSystem != null)
        {
          particleSystem.Play();
          particleSystem.Play();
        }
      }
    }
  }

}
