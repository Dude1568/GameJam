using System.CodeDom.Compiler;
using UnityEngine;
using UnityEngine.UI;
public class ShopIconImage : MonoBehaviour
{
    public Image image;
    void Awake()
    {
        image=GetComponent<Image>();
    }
}