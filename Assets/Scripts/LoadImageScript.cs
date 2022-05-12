using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImageScript : MonoBehaviour
{

    public enum ImageType // your custom enumeration
    {
        Logo,
        MainBG,
        CelerationScreenbBG,
        PrizeImage
    };
    public ImageType imageType;
    private RawImage image;
    public Texture2D defaultTexture;



    void Start()
    {
        DashboardManagerTemp.OnAdminSettingsUpdateEvent += OnAdminSettingsUpdate;
        image = GetComponentInParent<RawImage>();
        OnAdminSettingsUpdate();
    }

    void OnAdminSettingsUpdate()
    {
        //Debug.Log("OnAdminSettingsUpdate !!!");
        switch (imageType)
        {
            case ImageType.Logo:
                //image.texture = LoadPNG(GameManager.Instance.logoImagePath);
                image.texture = LoadPNG(PlayerPrefs.GetString("logoImagePath"));
                break;
            case ImageType.MainBG:
               // image.texture = LoadPNG(GameManager.Instance.bgImagePath);
                image.texture = LoadPNG(PlayerPrefs.GetString("bgImagePath"));
                break;
            case ImageType.CelerationScreenbBG:
               // image.texture = LoadPNG(GameManager.Instance.celebBGImagePath);
                image.texture = LoadPNG(PlayerPrefs.GetString("celebBGImagePath"));
                break;
            case ImageType.PrizeImage:
               // image.texture = LoadPNG(GameManager.Instance.prizeImagePath);
                image.texture = LoadPNG(PlayerPrefs.GetString("prizeImagePath"));
                break;
            default:
                break;
        }
       
    }


    public Texture2D LoadPNG(string filePath)
    {
       // Debug.Log("File Path: " + filePath);
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else
        {
            tex = defaultTexture;
        }
        return tex;
    }




}