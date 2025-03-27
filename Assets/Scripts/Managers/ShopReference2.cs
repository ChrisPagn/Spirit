using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopUIReference", menuName = "ScriptableObjects/ShopUIReference", order = 1)]
public class ShopReference2 : ScriptableObject
{
    public TextMeshProUGUI npcNameReference;
    public Animator animatorReference;
    public Transform sellButtonsParentReference;
}
