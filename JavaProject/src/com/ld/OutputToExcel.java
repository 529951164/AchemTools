package com.ld;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.Scanner;

public class OutputToExcel
{
  public OutputToExcel(String outputName, String text) throws Exception {
/*  25 */     String desc = "";



    
/*  30 */     Scanner scan = null;
    
/*  32 */     scan = new Scanner(text);
    
/*  34 */     scan.nextLine();
/*  35 */     desc = scan.nextLine();
/*  36 */     scan.nextLine();
/*  37 */     scan.nextLine();
/*  38 */     int numOfVars = scan.nextInt();
/*  39 */     int numOfRates = scan.nextInt();
/*  40 */     scan.nextLine();




    
/*  46 */     double d = (numOfRates * 4) / 32.0D; int i;
/*  47 */     for (i = 0; i < d; i++) {
/*  48 */       scan.nextLine();
    }

    
/*  52 */     ArrayList<String> vars = new ArrayList<String>();
/*  53 */     vars.add("Iter");
/*  54 */     vars.add("Time");
    
/*  56 */     ArrayList<Double> rates = new ArrayList<Double>();

    
/*  59 */     for (i = 0; i < numOfVars; i++) {
/*  60 */       vars.add(scan.next());
    }
    
/*  63 */     for (i = 0; i < numOfRates; i++) {
/*  64 */       rates.add(Double.valueOf(scan.next()));
    }
    
/*  67 */     ArrayList<ArrayList> values = new ArrayList<ArrayList>();
    
/*  69 */     while (scan.hasNext()) {
/*  70 */       ArrayList<String> temp = new ArrayList<String>();
/*  71 */       for (int k = 0; k < numOfVars + 2; k++) {
/*  72 */         temp.add(scan.next());
      }
/*  74 */       values.add(temp);
    } 

    
/*  78 */     BufferedWriter out = new BufferedWriter(new FileWriter("csv/" + outputName + ".csv"));

    
/*  81 */     out.write(desc + "\n");



    
/*  86 */     for (int j = 0; j < numOfRates; j++) {
/*  87 */       int temp = j + 1;
/*  88 */       out.write("k" + temp + "=" + rates.get(j) + ",");
    } 
/*  90 */     out.write("\n");

    
/*  93 */     for (String s : vars) {
/*  94 */       out.write(s + ",");
    }
/*  96 */     out.write("\n");

    
/*  99 */     for (ArrayList<String> arrayList : values) {
/* 100 */       for (String s : arrayList) {
/* 101 */         out.write(s + ",");
      }
/* 103 */       out.write("\n");
    } 

    
/* 107 */     out.close();
  }
}


/* Location:              /Users/liangdong/Desktop/Achem.jar!/achem/OutputToExcel.class
 * Java compiler version: 6 (50.0)
 * JD-Core Version:       1.1.3
 */