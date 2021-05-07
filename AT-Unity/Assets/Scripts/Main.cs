using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XCharts;

public class Main : MonoBehaviour
{
    public Slider sld1;
    public Slider sld2;
    public Button btn1;
    public Button btn2;
    public InputField Coefficient;  // 拟合数据系数
    public InputField AuxiliaryLine;    // 辅助线
    private LineChart chart;

    private Dictionary<string, List<Dictionary<string, double>>> _database =
        new Dictionary<string, List<Dictionary<string, double>>>();
    
    List<float> hg1 = new  List<float>();
    List<float> hg2 = new  List<float>();
    // Start is called before the first frame update
    void Start()
    {
        sld1.onValueChanged.AddListener(OnSld1Change);
        sld2.onValueChanged.AddListener(OnSld2Change);
        Init();
    }

    public void Init()
    {
        chart = gameObject.GetComponent<LineChart>();
        if (chart == null) return;

        chart.title.show = false;
        chart.title.text = "";
        chart.tooltip.show = true;
        chart.legend.show = true;
        chart.xAxises[0].show = true;
        chart.xAxises[1].show = false;
        chart.yAxises[0].show = true;
        chart.yAxises[1].show = false;
        // chart
        chart.xAxises[0].type = Axis.AxisType.Value;
        chart.yAxises[0].type = Axis.AxisType.Value;
        chart.yAxises[0].minMaxType = Axis.AxisMinMaxType.Default;
        chart.xAxises[0].axisName.name = "Time";
        int dataCount = 30;
        chart.xAxises[0].splitNumber = dataCount;
        chart.xAxises[0].boundaryGap = true;
        chart.RemoveData();
        Debug.Log("Done");
    }

    public void AddData(string key, List<Dictionary<string, double>> value)
    {
        _database.Add(key, value);
    }
    public void OpenExperimental(string type)
    {
        string path = LocalDialog.OpenDirectory(type);
        if (!string.IsNullOrEmpty(path)) //点击系统对话框框保存按钮
        {
            Debug.Log("OpenExperimental:");
            Debug.Log(path);
            string json = XD.FileUtil.LoadFile(path);
            try
            {
                var data = LitJson.JsonMapper.ToObject<List<Dictionary<string, List<Dictionary<string, double>>>>>(json);
                _database.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    foreach (var item in data[i])
                    {
                        AddData(item.Key, item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
            refreshData();
        }
    }
    
    public void OpenCSV(string type)
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "/光照-表格1.csv";//LocalDialog.OpenDirectory(type);
#else
         string path = LocalDialog.OpenDirectory(type);
#endif
        Debug.Log(path);
        if (!string.IsNullOrEmpty(path)) //点击系统对话框框保存按钮
        {
            hg1 = new  List<float>();
            hg2 = new  List<float>();
            List<List<string>> data = fgCSVReader.LoadCSV(path);
            Debug.Log("csv");
            data.RemoveAt(0);
            foreach (var line in data)
            {
                if (float.TryParse(line[1], out float v_hg1))
                    hg1.Add(v_hg1);
                else
                    hg1.Add(0);
                    
                if (float.TryParse(line[2], out float v_hg2))
                    hg2.Add(v_hg2);
                else
                    hg2.Add(0);
            }
            this.refreshData();
        }
    }

    private void setExperimentalData()
    {
        var hg1s = chart.AddSerie("Hg1-实验", SerieType.Line);
        var hg2s = chart.AddSerie("Hg2-实验", SerieType.Line);
        hg2s.symbol.size = 15;
        hg1s.symbol.size = 15;
        hg1s.symbol.type = SerieSymbolType.Circle;
        hg2s.symbol.type = SerieSymbolType.Rect;
        for (int i = 0; i < hg1.Count; i++)
        {
            chart.AddData(hg1s.index, i, (float)hg1[i]);
            chart.AddData(hg2s.index, i, (float)hg2[i]);
        }
    }

    public void setAuxiliaryLine()
    {
        var auxiliary = chart.AddSerie("参考线", SerieType.Line);
        int value = 150;
        int.TryParse(AuxiliaryLine.text, out value);
        for (int i = 0; i < 10; i++)
        {
            chart.AddData(auxiliary.index, i, value);
        }
    }

    private void setData(string key)
    {
        chart.RemoveData();
        setExperimentalData();
        setAuxiliaryLine();
        if (_database.ContainsKey(key))
        {
            var hg1s = chart.AddSerie("Hg1-拟合", SerieType.Line);
            var hg2s = chart.AddSerie("Hg2-拟合", SerieType.Line);
            hg1s.symbol.type = SerieSymbolType.Diamond;
            hg2s.symbol.type = SerieSymbolType.Triangle;
            hg2s.symbol.size = 7;
            hg1s.symbol.size = 7;
            List<Dictionary<string, double>> data = _database[key];
            int CoefficientNum = 300;
            int.TryParse(Coefficient.text, out CoefficientNum);
            
            for (int i = 0; i < data.Count; i++)
            {
                float time = second2Hour((float) data[i]["Time"]);
                var num = data[i]["Hg1"] * CoefficientNum;// * 10000000000000;
                chart.AddData(hg1s.index, time, (float)num);
                var hg2 = data[i]["Hg2"] * CoefficientNum;
                chart.AddData(hg2s.index, time, (float)hg2);
                Debug.Log(hg2);
            }
        }
    }

    float second2Hour(float s)
    {
        return s / 3600;
    }

    private void reset()
    {
        chart.RemoveData();
    }

    public void refreshData()
    {
        var key1 = sld1.value.ToString("0.#").Replace("0.", ".");
        var key2 = sld2.value.ToString("0.#").Replace("0.", ".");
        string key = $"result_K1={key1}_K2={key2}";
        this.btn1.GetComponentInChildren<Text>().text = "K1=" +  sld1.value.ToString("0.#");
        this.btn2.GetComponentInChildren<Text>().text = "K2=" +  sld2.value.ToString("0.#");
        setData(key);
    }

    void OnSld1Change(float value)
    {
        refreshData();
    }
    
    void OnSld2Change(float value)
    {
        refreshData();
    }

    public void OnClickChoseData()
    {
        OpenExperimental("json");
    }
    
    public void OnClickChoseCSV()
    {
        OpenCSV("csv");
    }
    
    public void OnAuxiliaryChange(){refreshData();}
    
    public void OnCoefficientChange(){refreshData();}
}
