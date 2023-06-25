using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] public string ignoredTag;
    [SerializeField] public float speed;
    [HideInInspector] public int damage;
}