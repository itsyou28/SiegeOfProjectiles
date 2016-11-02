using UnityEngine;
using System.Collections;

public class StandardScreen
{
    private static StandardScreen instance;

    public static StandardScreen Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new StandardScreen();
            }
            return instance;
        }
    }

    ///////////
    //아이언2
    public const float standardScreenWidth = 1080;
    public const float standardScreenHeight = 1920;
    public const float standardScreenHalfWidth = standardScreenWidth * 0.5f;
    public const float standardScreenHalfHeight = standardScreenHeight * 0.5f;
    public const float standardAspectRatio = standardScreenHeight / standardScreenWidth;

    //Potrait
    public const float standardDeviceRealWidth_mm = 66.5f;
    public const float standardDeviceRealHeight_mm = 117f;

    //Landscape
    //public const float standardDeviceRealWidth_mm = 93;
    //public const float standardDeviceRealHeight_mm = 56;
    ///////////

    public float aspectRatio;
    public float reverseAspectRatio;

    public float ratioX;
    public float ratioY;

    public float reverseRatioX;
    public float reverseRatioY;

    public float TouchPointOffset;

    private StandardScreen()
    {
        aspectRatio = (float)Screen.height / (float)Screen.width;
        reverseAspectRatio = aspectRatio / standardAspectRatio;

        ratioX = Screen.width / standardScreenWidth;
        ratioY = Screen.height / standardScreenHeight;

        reverseRatioX = standardScreenWidth / Screen.width;
        reverseRatioY = standardScreenHeight / Screen.height;

        TouchPointOffset = CalculateGUI.CalFixedSizeWithDPI(15);
    }
    
}
