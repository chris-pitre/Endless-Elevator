using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Universal Actor Settings")]
    public int maxHealth;
    public int speed;
    public int jumpForce;
    public int attack;
    public int defense;
    [Header("Actor References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletSpawn;
    [SerializeField] public BoxCollider2D boxCollider;
    [SerializeField] public LayerMask worldLayer;
    [SerializeField] public Animator animator;
}
