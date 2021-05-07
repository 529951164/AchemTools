using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XD
{
    /**/
    public class FileUtil
    {

        public void test()
        {
            //CreateFile(Application.persistentDataPath, "FileName.txt", "宣雨松MOMO");
        }

        /**
       * path：文件创建目录
       * name：文件的名称
       *  info：写入的内容
       */
        public static void CreateFile(string path, string name, string info = "")
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
                //Debug.Log("创建文件：" + path + "//" + name);
                //EditManager.showLog("创建文件：" + path+"//"+ name);
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();
                //Debug.Log("打开文件：" + path + "//" + name);
                //EditManager.showLog("打开文件：" + path+"//"+ name);
            }
            //以行的形式写入信息
            if (!string.IsNullOrEmpty(info))
                sw.WriteLine(info);
            //		sw.Write(info, false);

            //关闭流
            sw.Close();

            //销毁流
            sw.Dispose();
            //Debug.Log("end");
        }

        public static void CreateFile(string path, string info = "")
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path);
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
                UnityEngine.Debug.Log("创建文件：" + path);
                //EditManager.showLog("创建文件：" + path+"//"+ name);
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();
                //Debug.Log("打开文件：" + path);
                //EditManager.showLog("打开文件：" + path+"//"+ name);
            }
            //以行的形式写入信息
            if (!string.IsNullOrEmpty(info))
                sw.WriteLine(info);
            //		sw.Write(info, false);

            //关闭流
            sw.Close();

            //销毁流
            sw.Dispose();
            UnityEngine.Debug.Log("end");
        }

        /**
       * path：读取文件的路径
       * name：读取文件的名称
       */
        public static ArrayList LoadFile(string path, string name)
        {
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空
                return null;
            }
            string line;
            ArrayList arrlist = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                //一行一行的读取
                //将每一行的内容存入数组链表容器中
                arrlist.Add(line);
            }
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            //将数组链表容器返回
            return arrlist;
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
            ArrayList arrlist = new ArrayList();
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

        public static string LoadFile(string path)
        {
            string str = "";
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(string.Format("读取失败:{0}", path));
                return str;
            }

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                str += line;
            }
            //关闭流
            sr.Close();
            //销毁流
            sr.Dispose();
            return str;
        }

        public static List<string> searchFileNames(string path, string searchPattern)
        {
            List<string> names = new List<string>();
            string[] file = Directory.GetFiles(path, searchPattern);
            for (int i = 0; i < file.Length; i++)
            {
                string str = file[i];
                str = str.Replace(path, "");
                names.Add(str);
            }
            return names;
        }

        public static void Write(string path, string content)
        {
            // 检查文件是否存在
            FileInfo t = new FileInfo(path);
            if (!t.Exists)
                CreateFile(path);

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.SetLength(0);
            StreamWriter sw = new StreamWriter(fs);

            //开始写入
            sw.Write(content);

            //清空缓冲区
            sw.Flush();

            //关闭流
            sw.Close();
            fs.Close();
            //EditManager.showLog("写入文件：" + path);
        }

        /**
       * path：删除文件的路径
       * name：删除文件的名称
       */

        public static void DeleteFile(string path, string name)
        {
            File.Delete(path + "//" + name);
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static void DeleteAllFileInDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                string[] strFiles = Directory.GetFiles(path);
                for (int j = 0; j < strFiles.Length; j++)
                {
                    File.Delete(strFiles[j]);
                }
            }
        }

        public static bool checkFile(string path)
        {
            path = path.Replace("jar:file://", "");
            FileInfo t = new FileInfo(path);
            //		DirectoryInfo mydir = new DirectoryInfo(path);
            return t.Exists;
        }

        public static string getImageSuffixPath(string path)
        {
            string str = path + ".png";
            if (!FileUtil.checkFile(str))
            {
                str = path + ".jpg";
            }
            return str;
        }

        public static string getImageSuffix(string path)
        {
            if (FileUtil.checkFile(path + ".png"))
                return ".png";

            if (FileUtil.checkFile(path + ".jpg"))
                return ".jpg";

            return "";
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "GetMD5HashFromFile() fail, error:" + ex.Message;
                //throw new Exception("GetMD5HashFromFile() fail, error:" +ex.Message);
            }
        }

        public static void getAllFiles(string path, ref List<string> files)
        {
            string[] file = Directory.GetFiles(path);
            for (int i = 0; i < file.Length; i++)
            {
                string str = file[i];
                str = str.Replace(path, "");
                files.Add(str);
            }
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
                getAllFiles(dirs[i], ref files, path);
        }

        public static void getAllFiles(string path, ref List<string> files, string searchPattern)
        {
            string[] file = Directory.GetFiles(path, searchPattern);
            for (int i = 0; i < file.Length; i++)
                files.Add(file[i]);

            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
                getAllFiles(dirs[i], ref files, searchPattern);
        }

        public static void getAllFiles(string path, ref List<string> files, string searchPattern, string pathReplace = null)
        {
            if (string.IsNullOrEmpty(pathReplace))
                pathReplace = path;

            string[] file = Directory.GetFiles(path, searchPattern);
            for (int i = 0; i < file.Length; i++)
            {
                string str = file[i];
                str = str.Replace(pathReplace, "");
                files.Add(str);
            }
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
                getAllFiles(dirs[i], ref files, searchPattern, pathReplace);
        }

        public static void getAllFileNames(string path, ref List<string> files, string searchPattern)
        {
            string[] file = Directory.GetFiles(path, searchPattern);
            for (int i = 0; i < file.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(file[i]);
                files.Add(fileName);
            }
            string[] dirs = Directory.GetDirectories(path);
            for (int i = 0; i < dirs.Length; i++)
                getAllFileNames(dirs[i], ref files, searchPattern);
        }

        public static string suchFilePath(string folder, string fileName, string searchPattern = null)
        {
            List<string> files = new List<string>();
            getAllFiles(folder, ref files, searchPattern, "不需要");
            for (int i = 0; i < files.Count; i++)
            {
                string name = System.IO.Path.GetFileName(files[i]);
                if (name == fileName)
                {
                    return files[i];
                }
            }
            return null;
        }

        public static string getProjectPath()
        {
            return Application.dataPath.Replace("/Assets", "");
        }

        public static long getFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
        
        public static long GetDirectorySize(String path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            long length = 0;
            foreach (FileSystemInfo fsi in directoryInfo.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    length += ((FileInfo)fsi).Length;
                }
                else
                {
                    length += GetDirectorySize(fsi.FullName);
                }
            }
            return length;
        }
    }
}