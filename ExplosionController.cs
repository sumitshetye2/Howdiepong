using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
  public GameObject flash;
  public GameObject sparks;
  public GameObject smoke;
  public GameObject fire;

  ParticleSystem _flash;
  ParticleSystem _sparks;
  ParticleSystem _smoke;
  ParticleSystem _fire;

    // Start is called before the first frame update
    void Start()
    {
     _flash = flash.GetComponent<ParticleSystem>();
     _sparks = sparks.GetComponent<ParticleSystem>();
     _smoke = smoke.GetComponent<ParticleSystem>();
     _fire = fire.GetComponent<ParticleSystem>();
  }

    // Update is called once per frame
    void Update()
    {
        
    }

  private void OnTriggerEnter(Collider other)
  {
    if(other.gameObject.CompareTag("Asteroid"))
    {
      _flash.Play();
      _sparks.Play();
      _smoke.Play();
      _fire.Play();
    }
  }
}
