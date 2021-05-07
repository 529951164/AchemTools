using System.Runtime.InteropServices;
using UnityEngine;

public class LocalDialog
{
    //链接指定系统函数       打开文件对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOFN([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);//执行打开文件的操作
    }
 
    //链接指定系统函数        另存为对话框
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool GetSFN([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);//执行保存选中文件的操作
    }
    private static OpenFileName openFileName;
    public static string OpenDirectory(string type)
    {
        openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "文件(*." + type + ")\0*." + type + "";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\'); //默认路径
        openFileName.title = "选择文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetSaveFileName(openFileName)) //点击系统对话框框保存按钮
            return openFileName.file;
        else
            return null;
    }
}