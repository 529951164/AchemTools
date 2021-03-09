
import openpyxl
import csv
import os
from datetime import date

from openpyxl import Workbook
from openpyxl.chart import (
    LineChart,
    Reference,
)

def readCSV():
    with open('./result_K1-3_K2-1.csv', 'r', encoding='Shift-JIS') as f:
        read = csv.reader(f)
        wb = openpyxl.Workbook()
        ws = wb.active
        ws.title = "test"
        for line in read:
            print(line)
            ws.append(line)

    wb.save('example.xlsx')
    print('done')

def readExcel():
    from openpyxl import Workbook, load_workbook
    wb = load_workbook('./result_K1-3_K2-1.csv')
    ws = wb.active
    for row in ws:
        for cell in row:
            ws.append(cell)
            print(cell)

    wb.save('example.xlsx')
    print('done')

def add_chart(path, file_name):
    wb = Workbook()
    ws = wb.active
    if not os.path.exists('excel'):
        os.makedirs('excel')

    with open(path, 'r', encoding='Shift-JIS') as f:
        read = csv.reader(f)
        for line in read:
            ws.append(line)

        for row in ws:
            for cell in row:
                # cell.number_format = '0.00'
                if cell.value != None:
                    try:
                        print(float(cell.value))
                        cell.value = float(cell.value)
                    except Exception as e:
                        print('str:' + cell.value)


    c1 = LineChart()
    # c1.title = "k1=3,k2=1"
    c1.style = 13
    c1.y_axis.title = '生成物浓度'
    c1.x_axis.title = '时间'
    data = Reference(ws, min_col=5, min_row=3, max_col=6, max_row=41)
    cats = Reference(ws, min_col=2, min_row=4, max_row=41)
    c1.add_data(data, titles_from_data=True)
    c1.set_categories(cats)
    # Style the lines
    s1 = c1.series[0]
    s1.marker.symbol = "triangle"
    s1.marker.graphicalProperties.solidFill = "FF0000"  # Marker filling
    s1.marker.graphicalProperties.line.solidFill = "FF0000"  # Marker outline
    s1.graphicalProperties.line.noFill = True
    s2 = c1.series[1]
    s2.graphicalProperties.line.solidFill = "00AAAA"
    s2.graphicalProperties.line.dashStyle = "sysDot"
    s2.graphicalProperties.line.width = 100050  # width in EMUs

    ws.add_chart(c1, "i10")

    wb.save("excel/" + file_name + '.xlsx')


def file_name(file_dir):
    for root, dirs, files in os.walk(file_dir):
        print('files:', files)  # 当前路径下所有非目录子文件
        for file in files:
            add_chart('./csv/' + file, file.replace('.csv',''))


if __name__ == '__main__':
    file_name('csv')

