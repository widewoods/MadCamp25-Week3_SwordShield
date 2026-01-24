using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
  [SerializeField] private int maxHealth = 3;
  [SerializeField] private int currentHealth;
  [SerializeField] private GolemController golemController;
  public enum BossPhase { Phase1, Phase2 }
  public BossPhase phase;

  public
  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
    phase = BossPhase.Phase1;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void DamageBoss(int damage)
  {
    if (!golemController.ProtectionBroken) return;

    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      golemController.Die();
    }
    if (currentHealth <= maxHealth / 2)
    {
      phase = BossPhase.Phase2;
    }
  }
}
