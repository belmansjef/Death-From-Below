using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public float deceleration = 5f;
    public float lifetime = 3f;
    public float fadeTime = 1f;
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
        
    }

    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);
        lifetime -= Time.deltaTime;
        
        if (lifetime < 0)
        {
            if(iTween.Count(gameObject) == 0)
            {
                iTween.FadeTo(gameObject, iTween.Hash("alpha", 0f, "oncomplete", "DestroyPiece")); 
            }
        }
    }

    private void DestroyPiece()
    {
        Destroy(gameObject);
    }

}
