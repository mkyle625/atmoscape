using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinButtonController : MonoBehaviour
{
    [Header("Unity Setup")]
    public int skinIndex; //The index of the button, to know what skin to apply
    public ShopController shopController;

    public void Click_Skin()
    {
        shopController = FindObjectOfType<ShopController>();
        shopController.Click_Skin(skinIndex);
    }
}
