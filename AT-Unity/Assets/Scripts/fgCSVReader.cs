// (c) Francois GUIBERT, Frozax Games
//
// Free to use for personal and commercial uses.
// Tweet @Frozax if you like it.
//

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class fgCSVReader
{
    public delegate void ReadLineDelegate(int line_index, List<string> line);

    public static List<List<string>> trim(List<List<string>> ll)
    {
        List<List<string>> result = new List<List<string>>();
        for (int i = 0; i < ll.Count; i++)
        {
            List<string> line = ll[i];
            List<string> Newline = new List<string>();
            for (int j = 0; j < line.Count; j++)
            {
                if (string.IsNullOrEmpty(line[j]))
                    continue;
                else
                    Newline.Add(line[j]);
            }
            if (Newline.Count > 0)
                result.Add(Newline);
        }

        return result;
    }
    
	public static List<List<string>> LoadCSV(string file_name){
		CsvInfo ci = new CsvInfo ();
		LoadFromFile (file_name, ci.line_reader);

        int lineNum = getFileLineNum(file_name);

        // 检查CSV是否加载成功
        var data = ci.getData();
        if (data.Count < lineNum)
        {
            UnityEngine.Debug.LogErrorFormat("CSV读取不完整,读取{0}行，文件{1}行, path:{2}", data.Count, lineNum, file_name);
        }
        return trim(data);
    }

    class CsvInfo{
		List<List<string>> data = new List<List<string>> ();
		public void line_reader(int line_index, List<string> line){
		    if (line.Count > 0)
			    data.Add (new List<string>(line));
		}

		public List<List<string>> getData(){
			return this.data;
		}
	}

    public static void LoadFromFile(string file_name, ReadLineDelegate line_reader)
    {
		LoadFromString(File.ReadAllText(file_name, Encoding.UTF8), line_reader);
    }

    public static void LoadFromString(string file_contents, ReadLineDelegate line_reader)
    {
        int file_length = file_contents.Length;

        // read char by char and when a , or \n, perform appropriate action
        int cur_file_index = 0; // index in the file
        List<string> cur_line = new List<string>(); // current line of data
        int cur_line_number = 0;
        StringBuilder cur_item = new StringBuilder("");
        bool inside_quotes = false; // managing quotes
        while (cur_file_index < file_length)
        {
            char c = file_contents[cur_file_index++];

            switch (c)
            {
            case '"':
                if (!inside_quotes)
                {
                    inside_quotes = true;
                }
                else
                {
                    if (cur_file_index == file_length)
                    {
                        // end of file
                        inside_quotes = false;
                        goto case '\n';
                    }
                    else if (file_contents[cur_file_index] == '"')
                    {
                        // double quote, save one
                        cur_item.Append("\"");
                        cur_file_index++;
						if (cur_file_index == file_length - 1)
							UnityEngine.Debug.Log ("end");
                    }
                    else
                    {
                        // leaving quotes section
                        inside_quotes = false;
                    }
                }
                break;
            case '\r':
                // ignore it completely
                break;
            case ',':
                goto case '\n';
            case '\n':
                if (inside_quotes)
                {
                    // inside quotes, this characters must be included
                    cur_item.Append(c);
                }
                else
                {
                    // end of current item
                    cur_line.Add(cur_item.ToString());
                    cur_item.Length = 0;
                    if (c == '\n' || cur_file_index == file_length)
                    {
                        // also end of line, call line reader
                        line_reader(cur_line_number++, cur_line);
                        cur_line.Clear();
                    }
                }
                break;
            default:
                // other cases, add char
                cur_item.Append(c);
                break;
            }
        }
    }

    public static int getFileLineNum(string path)
    {
        int num = 0;
        //使用流的形式读取
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return num;
        }

        string line;
        System.Collections.ArrayList arrlist = new System.Collections.ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            num++;
        }

        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return num;
    }
}
