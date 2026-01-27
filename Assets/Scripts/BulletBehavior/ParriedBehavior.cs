using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParriedBehavior : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  protected virtual void HandleBulletDestroy()
  {
    Destroy(gameObject);
  }

  public virtual void HandleTrigger(Collider2D other)
  {
    if (other.gameObject.CompareTag("Wall"))
    {
      HandleBulletDestroy();
    }
    else if (other.gameObject.CompareTag("Enemy"))
    {
      other.gameObject.GetComponent<EnemyHealth>().Stunned();
    }
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    HandleTrigger(collision);
  }
}
