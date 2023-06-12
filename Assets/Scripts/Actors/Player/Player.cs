using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Header("Player Specific Settings")]
    public float dashingPower = 4f;
    public float dashingTime = 0.2f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
}
