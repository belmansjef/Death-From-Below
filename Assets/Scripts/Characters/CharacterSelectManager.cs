using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    // Singleton
    public static CharacterSelectManager instance;

    public PlayerController activePlayer;
    public CharacterSelector activeCharacterSelector;
    
    private void Awake()
    {
        instance = this;
    }
}
