using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
  private Rigidbody _rigidbody;
  private Vector3 _velocity = Vector3.forward;
  private System.Random _random;

  private List<Transform> listOfDirections = new List<Transform>();

  [SerializeField] private float _speed;
  [SerializeField] private float _accelaration;
  [SerializeField] private float _maxSpeed;

  private void OnEnable()
  {
    GameManager.setMotionDetails += SetMotionDetails;
  }

  private void Start()
  {
    _rigidbody = GetComponent<Rigidbody>();
    //_velocity = Vector3.forward * _speed;
    //_rigidbody.velocity = _velocity;
    _random = new System.Random();
    foreach (Transform ch in GameManager.instance.GetAllPossibleDirections())
    {
      listOfDirections.Add(ch);
    }
  }

  public void SetMotionDetails(float speed, float accelaration, float maxSpeed)
  {
    //Debug.Log("Called this method");
    _speed = speed;
    _accelaration = accelaration;
    _maxSpeed = maxSpeed;
  }

  public void InitialPush()
  {
    _rigidbody = GetComponent<Rigidbody>();
    _velocity = Vector3.back * _speed;
    _rigidbody.velocity = _velocity;
    _random = new System.Random();
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.tag.Equals("BackWall"))
    {
      int dirIndex = _random.Next(0, listOfDirections.Count);
      Vector3 dirToUse = listOfDirections[dirIndex].forward;
      _rigidbody.velocity = dirToUse * _speed;
    }

    if (collision.collider.tag.Equals("Player"))
    {
      int dirIndex = _random.Next(0, listOfDirections.Count);
      Vector3 dirToUse = listOfDirections[dirIndex].forward;
      _rigidbody.velocity = dirToUse * _speed;
      GameManager.instance.BallHit();
    }

    if (collision.collider.tag.Equals("KillWall"))
    {
      //Debug.Log("Time to Kill the ball");
      gameObject.SetActive(false);
      _rigidbody.velocity = Vector3.zero;
      transform.position = Vector3.zero;
      GameManager.instance.BallMissed();
    }

    if (collision.gameObject.tag.Equals("Ball"))
    {
      Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
    }
  }

  public void IncreaseSpeed()
  {
    _speed += _accelaration;
    if (_speed > _maxSpeed)
    {
      _speed = _maxSpeed;
    }
  }

  private void Update()
  {
    //Debug.Log("Current Speed : " + _rigidbody.velocity.magnitude);
    if (_rigidbody.velocity.magnitude != _speed)
    {
      var velocietyToSet = _rigidbody.velocity.normalized.Equals(Vector3.zero) ?
        Vector3.forward : _rigidbody.velocity.normalized;
      _rigidbody.velocity = velocietyToSet * _speed;
    }
  }

  private void OnDisable()
  {
    GameManager.setMotionDetails -= SetMotionDetails;
  }
}
