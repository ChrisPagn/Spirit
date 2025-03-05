using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    public void AddSpeed(int speedGiven, float speedDuration)
    {
        PlayerMovement.instance.moveSpeed += speedGiven;
        StartCoroutine(RemoveSpeed(speedGiven, speedDuration));
    }

    public IEnumerator RemoveSpeed(int speedGiven, float speedDuration)
    {
        yield return new WaitForSeconds(speedDuration);
        PlayerMovement.instance.moveSpeed -= speedGiven;
    }
}
