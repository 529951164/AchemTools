package com.ld;

import com.alibaba.fastjson.JSON;

import java.io.*;
import java.text.DecimalFormat;
import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        ClearFolder("csv");
        ClearFolder("excel");
        ReadParameter();
    }

    public static void ReadParameter()
    {
        String jsonString = ReadFile("config.json");
        Group group2 = JSON.parseObject(jsonString, Group.class);
        System.out.println("parameter:(" + jsonString);
        RangeCommand(group2.start_k1, group2.end_k1, group2.start_k2, group2.end_k2, group2.start_k3, group2.end_k3, group2.start_k4, group2.end_k4, group2.offset);
    }

    public static void RangeCommand(float start_k1, float end_k1, float start_k2, float end_k2, float start_k3, float end_k3, float start_k4, float end_k5, float pass)
    {
        judeDirExists(new File("csv"));
        String result = "";
        for (float i = start_k1; i < end_k1; i+= pass) {
            for (float j = start_k2; j < end_k2; j+= pass) {
                for (float k = start_k2; k < end_k2; k+= pass) {
                    for (float l = start_k2; l < end_k2; l+= pass) {
                        DecimalFormat decimalFormat=new DecimalFormat(".0");//构造方法的字符格式这里如果小数不足2位,会以0补足.
                        String k1=decimalFormat.format(i);//format 返回的是字符串
                        String k2=decimalFormat.format(j);
                        String k3=decimalFormat.format(k);
                        String k4=decimalFormat.format(l);

                        result = getResult(k1, k2, k3, k4);
                        try {
                            new OutputToExcel("result_K1=" + k1 + "_K2=" + k2 + "_K3=" + k3 + "_K4=" + k4, result);
                        }
                        catch (Exception ex)
                        {
                            ex.printStackTrace();
                            System.err.println("OutputToExcel." + ex.getMessage());
                        }
                        System.out.println(result);
                    }
                }
            }
        }
    }
    private static String endOfLine = System.getProperty("line.separator");
    public static String getResult(String k1, String k2, String k3, String k4)
    {
        Runtime r = Runtime.getRuntime();
             try {
               String root = System.getProperty("user.dir");
               String acuchemPath = root + "\\Achem2.0\\acuchemWIN.exe";
               System.out.println("acuchemPath:" + acuchemPath);
               File achem = new File(acuchemPath);
               if (achem == null)
                     throw new NullPointerException("file achem not set in runCommand");
               System.out.println(achem.getCanonicalPath() + " " + achem.getParentFile());
               Process p = r.exec(achem.getCanonicalPath(), (String[])null, achem.getParentFile());
               OutputStream stdout = p.getOutputStream();
               OutputStreamWriter osw = new OutputStreamWriter(stdout);
               BufferedWriter bw = new BufferedWriter(osw);
               String str = getTemplate().replace("{k1}", k1).replace("{k2}", k2).replace("{k3}", k3).replace("{k4}", k4);
               Scanner scan = new Scanner(str);
               while (scan.hasNextLine()) {
                     String line = scan.nextLine();
                     bw.write(line + endOfLine);
                   }
               bw.newLine();
               bw.flush();
               bw.close();
               Scanner outputStream = new Scanner(p.getInputStream());
               String result = "";
               while (outputStream.hasNext())
                   result += outputStream.nextLine() + endOfLine;

               p.waitFor();
               int exitValue = p.exitValue();
               if (exitValue != 0)
                     throw new Exception("Exit value = " + exitValue);

//               System.out.println(result);
               return result;
             } catch (Exception ex) {
               ex.printStackTrace();
               System.err.println("runCommand: Cannot run file." + ex.getMessage());
               return "error";
             }
    }

    public static String getTemplate()
    {
        return ReadFile("irradiation_oxidation");
    }

    public static String ReadFile(String strFile){
        String content = "";
        try{
            InputStream is = new FileInputStream(strFile);
            int iAvail = is.available();
            byte[] bytes = new byte[iAvail];
            is.read(bytes);
            content = new String(bytes);
            System.out.println("JsonConfig:" + content);
            is.close();
        }catch(Exception e){
            e.printStackTrace();
        }
        return content;
    }

    public static void judeDirExists(File file) {
        if (file.exists()) {
            if (file.isDirectory()) {
                System.out.println("dir exists");
            } else {
                System.out.println("the same name file exists, can not create dir");
            }
        } else {
            System.out.println("dir not exists, create it ...");
            file.mkdir();
        }

    }

    public static void ClearFolder(String str){
        File file = new File(str);
        if (file.exists())
        {
            if (file.isDirectory()) {
                for (File f : file.listFiles()) {
                    f.delete();
                }
            } else {
                file.delete();
            }
        }
    }
}
