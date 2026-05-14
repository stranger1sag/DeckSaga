using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image image;
    public Perk perk {get; private set;}
    public void Setup(Perk perk)
    {
        this.perk = perk;
        image.sprite = perk.Image;
    }
}
