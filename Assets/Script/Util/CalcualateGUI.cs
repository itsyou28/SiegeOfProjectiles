using UnityEngine;
using System.Collections;


public static class CalculateGUI
{
    //dpi : dot per inch = 1인치에 들어가는 점의 개수 = 1인치 당 Pixel
    //틀림(1픽셀의 인치 단위 실제 사이즈)
    //맞음(실제 단위 1인치에 들어가는 현재 기기의 픽셀 수)

    public const float mmToInch = 0.0393700787f;
    public const float InchTomm = 25.4f;
    public const float BK_Monitor_DPI = 98;
    public const float BK_Monitor_Test_DPI = 217.5f;

    /// <summary>
    /// mm 단위로 수치를 입력하면 기기 DPI를 이용해 해당기기의 픽셀단위 사이즈를 반환한다. 
    /// </summary>
    public static float CalFixedSizeWithDPI(float mm)
    {
#if !UNITY_EDITOR
        return Screen.dpi * (mm * mmToInch);
#else
        return BK_Monitor_DPI * (mm * mmToInch);
#endif
    }

    /// <summary>
    /// 현재 기기의 픽셀크기를 입력하면 실제 화면상의 크기를 mm 단위로 반환한다. 
    /// </summary>
    /// <param name="pixel"></param>
    /// <returns></returns>
    public static float CalRealSizeWithPixel(float pixel)
    {
#if !UNITY_EDITOR
        return pixel / Screen.dpi / mmToInch;
#else
        return pixel / BK_Monitor_DPI / mmToInch;
#endif
    }

    /// <summary>
    /// 타겟 디바이스를 기준으로 너비를 픽셀값으로 입력하면 
    /// 실제 너비(mm)를 반환한다. 
    /// </summary>
    public static float CalWidthPixelTo_mm(float pixel)
    {
        return BK_Function.ConvertRange(0, StandardScreen.standardScreenWidth,
            0, StandardScreen.standardDeviceRealWidth_mm, pixel);
    }
    
    /// <summary>
    /// 타겟 디바이스를 기준으로 높이를 픽셀값으로 입력하면 
    /// 실제 너비(mm)를 반환한다. 
    /// </summary>
    public static float CalHeightPixelTo_mm(float pixel)
    {
        return BK_Function.ConvertRange(0, StandardScreen.standardScreenHeight,
            0, StandardScreen.standardDeviceRealHeight_mm, pixel);
    }

    /// <summary>
    /// 타겟 디바이스를 기준으로 너비를 픽셀값으로 입력하면
    /// 타겟 디바이스에서의 실제 크기와 동일한 크기의 현재 기기의 픽셀값을 반환한다. 
    /// </summary>
    public static float CalDPI_width(float pixel)
    {
        return CalFixedSizeWithDPI(CalWidthPixelTo_mm(pixel));
    }

    /// <summary>
    /// 타겟 디바이스를 기준으로 높이를 픽셀값으로 입력하면
    /// 타겟 디바이스에서의 실제 크기와 동일한 크기의 현재 기기의 픽셀값을 반환한다. 
    /// </summary>
    public static float CalDPI_height(float pixel)
    {
        return CalFixedSizeWithDPI(CalWidthPixelTo_mm(pixel));
    }

    public static float CalRES_width(float pixel)
    {
        return pixel * StandardScreen.Inst.ratioX;
    }

    public static float CalRES_height(float pixel)
    {
        return pixel * StandardScreen.Inst.ratioY;
    }
}