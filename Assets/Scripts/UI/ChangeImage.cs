using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Image oldImage;
    public Sprite[] newImageArr;
    public void ChangeNumber(int number){
        oldImage.sprite = newImageArr[number];
    }
}
