using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterTracker : MonoBehaviour
{
    public static CharachterTracker Instance;
    public int currentHealth, maxHealth, currentCoins;
    
    private void Awake()
    {
        Instance = this;
    }
}
