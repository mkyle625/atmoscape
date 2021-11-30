using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreditPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _creditAmount;
    [SerializeField] private float _chanceForDoubleCredit;
    [SerializeField] private Color32 _doubleCreditColor;
    [SerializeField] private Color32 _singleCreditColor;
    
    [Header("Unity Setup")]
    [SerializeField] private GameObject _regularPickupEffect;
    [SerializeField] private GameObject _doublePickupEffect;
    [SerializeField] private SpriteRenderer _creditGlow;
    [SerializeField] private SpriteRenderer _creditOutlineOuter;
    [SerializeField] private SpriteRenderer _creditSymbol;
    [SerializeField] private ParticleSystem _creditParticleSystem;

    private bool isDoubleCredit;
    
    private void Start()
    {
        //Randomly decide if a double credit will spawn given the chance (0.3 by default)
        isDoubleCredit = Random.value > 1 - _chanceForDoubleCredit && SaveManager.Instance.state.creditUpgradePurchased;

        //If we got a double credit, set the colors to display that and update the amount
        if (isDoubleCredit)
        {
            _creditAmount = 2;
            
            _creditGlow.color = _doubleCreditColor;
            _creditOutlineOuter.color = _doubleCreditColor;
            _creditSymbol.color = _doubleCreditColor;
            _creditParticleSystem.startColor = _doubleCreditColor;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        SaveManager.Instance.state.playerCredits += _creditAmount;
        SaveManager.Instance.state.totalCreditsEarned += _creditAmount;
        ParticleSystem[] vParticleSystems;

        // Check if double or regular credit
        if (!isDoubleCredit)
        {
            vParticleSystems = Instantiate(_regularPickupEffect, transform.position, Quaternion.identity).GetComponentsInChildren<ParticleSystem>();
        }
        else
        {
            vParticleSystems = Instantiate(_doublePickupEffect, transform.position, Quaternion.identity).GetComponentsInChildren<ParticleSystem>();
        }
        
        if (isDoubleCredit)
        {
            foreach (var vParticleSystem in vParticleSystems)
            {
                vParticleSystem.startColor = _doubleCreditColor;
                vParticleSystem.Play();
            }
        }
        else
        {
            foreach (var vParticleSystem in vParticleSystems)
            {
                vParticleSystem.startColor = _singleCreditColor;
                vParticleSystem.Play();
            }
        }
        
        Destroy(gameObject);
    }
}
