using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS || UNITY_IPHONE
        #else
       
        #endif
        OpenDirectory("json");
    }

    public void LoadJsonPC()
    {
        
        
        // // 加载json
        // string json = Resources.Load<TextAsset>("all").text;
        // var data = LitJson.JsonMapper.ToObject<List<Dictionary<string, List<Dictionary<string, double>>>>>(json);
        // for (int i = 0; i < data.Count; i++)
        // {
        //     // var item = data[i];
        //     foreach (var item in data[i])
        //     {
        //         _database.Add(item.Key, item.Value);
        //     }
        // }
    }

    private OpenFileName openFileName;
    public void OpenDirectory(string type)
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
        {
            //TODO
            Debug.Log("openFileName.file");
            
        }
    }

}
