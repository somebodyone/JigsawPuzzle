# -*- coding: utf-8 -*-
import time
import sys
import os
import platform

try:
    import openpyxl
except Exception as e:
    os.system("pip install openpyxl")
    import openpyxl
from openpyxl.cell.read_only import EMPTY_CELL
from decimal import Decimal
Workdir = ""
IsFromUnity = False
isWindows = False
ListMode = [2,8,9,12,13,14,15,16,17,18,20,1,1111]
SubListMode = [10]
OneLineMode = [10]
KeepRowKeyMode = [14,15,16,17,18]
NotAllowMergeKey = [14,15,16,17,18]
if platform.system().lower() == "windows":
    isWindows = True

def toString(s):
    if isWindows:
        s = s.decode("utf-8").encode("gbk")
    return s

def toLogString(s):
    if IsFromUnity:
        return s
    if isWindows:
        s = s.decode("utf-8").encode("gbk")
    return s

def formatCellValue(cell):
    if cell is EMPTY_CELL or cell._value is None:
        return None
    data_type = cell.data_type
    if data_type == 'n':
        number_format = cell.number_format

        if number_format == 'General':
            value = cell._value
            if int(value) == value:
                return int(value)
            else:
                value = float("%.6f" % value)
                return int(value) if int(value) == value else value
        elif number_format.endswith("%"):
            digitn = 2 if number_format == "0%" else len(number_format) - 1
            value = float(("%." + str(digitn) + "f") % cell._value)
            return int(value) if int(value) == value else value
        elif number_format.endswith("_ "):
            digitn = 0 if number_format == "0_ " else len(number_format) - 4
            value = float(("%." + str(digitn) + "f") % cell._value)
            return int(value) if int(value) == value else value
        elif number_format == "@":
            return str(cell.value)
        else:
            return cell.value
    elif data_type == 's' or data_type == 'str':
        value = cell.value
        return None if value == u"" else value.encode('utf-8')
    elif data_type == 'e':
        return None
    else:
        return cell.value

def getValueStr(value):
    if value == "" or value == None:
        return "nil"
    elif isinstance(value, str):
        return value if value[0] == "{" and value[-1] == "}" else '"'+ value + '"'
    elif isinstance(value, list):
        valueStr = '{'
        for i in xrange(0,len(value)):
            if value[i] != None:
                if i != 0:
                    valueStr = valueStr + ","
                valueStr = valueStr + getValueStr(value[i])
        valueStr = valueStr + "}"
        return valueStr
    else:
        return str(value)

def getKeyStr(value):
    return value if isinstance(value, str) else '[' + str(value) + ']'

def isAllNumber(value):
    vlist = value.strip().replace("[","").replace("]","").split(',')
    result = True
    for v in vlist:
        try:
            x = float(v)
        except:
            result = False
            break
    return result

def isAllString(value):
    vlist = value.strip().replace("[","").replace("]","").split(',')
    result = True
    for v in vlist:
        if not v.startswith('"') or not v.endswith('"'):
            result = False
    return result

def isArrayValueStr(value):
    if value == "[]":
        return True
    if  value[0] == "[" and value[-1] == "]":
        if isAllNumber(value[1:-1]):
            return True
        if isAllString(value[1:-1]):
            return True
    return False

def getValueJsonStr(value):
    if value == "" or value == None:
        return '""'
    elif isinstance(value, str):
        return value if isArrayValueStr(value) else '"'+ value.replace("\n", "\\n") + '"'
    elif isinstance(value, list):
        valueStr = '['
        for i in xrange(0,len(value)):
            if value[i] != None:
                if i != 0:
                    valueStr = valueStr + ","
                valueStr = valueStr + getValueJsonStr(value[i])
        valueStr = valueStr + "]"
        return valueStr
    else:
        return str(value)

def getKeyJsonStr(value):
    return '"' + str(value).strip() + '"'

def rowValueToLuaStr(rowValue, keyValue, mode, rowIndex, spaceCount):
    rowStr = ""
    if mode == 0:
        rowStr = "[%s]={" % getValueStr(rowValue[0])
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyStr(keyValue[i]) + "=" + getValueStr(rowValue[i])
                index = index + 1
        rowStr = rowStr + "},"
    elif mode == 1:
        rowStr = "{"
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 1:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyStr(keyValue[i]) + "=" + getValueStr(rowValue[i])
                index = index + 1
        rowStr = rowStr + "},"
    elif mode == 2:
        rowStr = getValueStr(rowValue[1]) + ","
    elif mode == 3:
        if rowIndex == 0:
            rowStr = rowStr + "[0]={"
            for i in xrange(1,len(keyValue)):
                if i != 1:rowStr = rowStr + ", "
                rowStr = rowStr + getValueStr(keyValue[i])
            rowStr = rowStr + "},\n" + "%*s" % (spaceCount, "")

        rowStr = rowStr + "[%s]={" % getValueStr(rowValue[0])
        for i in xrange(1,len(rowValue)):
            if i != 1:rowStr = rowStr + ", "
            rowStr = rowStr + getValueStr(rowValue[i])
        rowStr = rowStr + "},"
    elif mode == 4:
        rowStr = getKeyStr(rowValue[0]) + "=" + getValueStr(rowValue[1]) + ","
    elif mode == 5:
        if len(rowValue) == 2:
            rowStr = "[%s]=%s," % (getValueStr(rowValue[0]), getValueStr(rowValue[1]))
        else:
            rowStr = "[%s]={" % getValueStr(rowValue[0])
            index = 0
            for i in xrange(1,len(rowValue)):
                if rowValue[i] != None:
                    if index != 0:rowStr = rowStr + ", "
                    rowStr = rowStr + getValueStr(rowValue[i])
                    index = index + 1
            rowStr = rowStr + "},"
    elif mode == 6:
        rowStr = "[%s]={" % getValueStr(rowValue[0])
        for i in xrange(1,len(rowValue)):
            if i != 1:rowStr = rowStr + ", "
            rowStr =  rowStr + getValueStr(keyValue[i]) + "," + getValueStr(rowValue[i])
        rowStr = rowStr + "},"
    elif mode == 7:
        rowStr = "[%s]={" % getValueStr(rowValue[0])
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr =  rowStr + "{" + getValueStr(keyValue[i]) + "," + getValueStr(rowValue[i]) +"}"
                index = index + 1
        rowStr = rowStr + "},"
    elif mode == 8:
        if rowIndex == 0:
            for i in xrange(1, len(keyValue)):
                rowStr = rowStr + getValueStr(keyValue[i]) + ", "
        else:
            rowStr = None
    elif mode == 9:
        rowStr = "["
        index = 0
        for i in xrange(0, len(rowValue)):
            if rowValue[i] != None:
                if index != 0: rowStr = rowStr + ", "
                rowStr = rowStr + getValueStr(rowValue[i])
                index = index + 1
        rowStr = rowStr+"],"
    elif mode == 10:
        if rowIndex != 0:rowStr = ", "
        for i in xrange(0, len(rowValue)):
            rowStr = rowStr + getValueStr(rowValue[i])
    elif mode == 11 or mode == 12:
        rowStr = "[%s]={" % getValueStr(rowIndex + 1)
        index = 0
        for i in xrange(0,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyStr(keyValue[i]) + " = " +getValueStr(rowValue[i])
                index = index + 1
        rowStr = rowStr + "},"
    return rowStr

def rowValueToJsonStr(rowValueList, rowValue, keyValue, mode, rowIndex, spaceCount, table):
    rowStr = ""
    isLastRow = len(rowValueList)-1 == rowIndex
    if mode == 0:
        rowStr = "%s:{" % getKeyJsonStr(rowValue[0])
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 1000:
        rowStr = "%s:{" % getKeyJsonStr(rowValue[0])+"\n%*s" % (spaceCount+4, "")
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "+"\n%*s" % (spaceCount+4, "")
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        rowStr = rowStr +"\n%*s" % (spaceCount, "")
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 1100:
        cloIndex = 0
        for i in xrange(0,len(keyValue)):
            if keyValue[i] == table["colName"]:
                cloIndex = i
        rowStr = rowStr + getKeyJsonStr(rowValue[1]) + ":" + getValueJsonStr(rowValue[cloIndex])
        if isLastRow:
            rowStr = rowStr + ""
        else:
            rowStr = rowStr + ","
    elif mode == 100:
        rowStr = "%s:{" % getKeyJsonStr(rowValue[0])
        index = 0
        for i in xrange(0,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 1:
        rowStr = "{"
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 1111:
        rowStr = "{"+"\n%*s" % (spaceCount+4, "")
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "+"\n%*s" % (spaceCount+4, "")
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        rowStr = rowStr +"\n%*s" % (spaceCount, "")
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 2:
        rowStr = "{"
        index = 0
        for i in xrange(0,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" + getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
        # rowStr = getValueJsonStr(rowValue[1])
        # if not isLastRow:
        #     rowStr += ","
    elif mode == 3:
        if rowIndex == 0:
            rowStr = rowStr + '"0":['
            for i in xrange(1,len(keyValue)):
                if i != 1:rowStr = rowStr + ", "
                rowStr = rowStr + getValueJsonStr(keyValue[i])
            rowStr = rowStr + "],\n" + "%*s" % (spaceCount, "")

        rowStr = rowStr + "%s:[" % getKeyJsonStr(rowValue[0])
        for i in xrange(1,len(rowValue)):
            if i != 1:rowStr = rowStr + ", "
            rowStr = rowStr + getValueJsonStr(rowValue[i])
        if isLastRow:
            rowStr = rowStr + "]"
        else:
            rowStr = rowStr + "],"
    elif mode == 4:
        rowStr = getKeyJsonStr(rowValue[0]) + ":" + getValueJsonStr(rowValue[1])
        if not isLastRow:
            rowStr += ","
    elif mode == 5:
        if len(rowValue) == 2:
            rowStr = getKeyJsonStr(rowValue[0]) +":"+ getValueJsonStr(rowValue[1])
            if not isLastRow:
                rowStr += ","
        else:
            rowStr = "%s:[" % getKeyJsonStr(rowValue[0])
            index = 0
            for i in xrange(1,len(rowValue)):
                if rowValue[i] != None:
                    if index != 0:rowStr = rowStr + ", "
                    rowStr = rowStr + getValueJsonStr(rowValue[i])
                    index = index + 1
            if isLastRow:
                rowStr = rowStr + "]"
            else:
                rowStr = rowStr + "],"
    elif mode == 6:
        rowStr = getKeyJsonStr(rowValue[0]) + ":["
        for i in xrange(1,len(rowValue)):
            if i != 1:rowStr = rowStr + ", "
            rowStr =  rowStr + getValueJsonStr(keyValue[i]) + "," + getValueJsonStr(rowValue[i])
        if isLastRow:
            rowStr = rowStr + "]"
        else:
            rowStr = rowStr + "],"
    elif mode == 7:
        rowStr = getKeyJsonStr(rowValue[0]) + ":["
        index = 0
        for i in xrange(1,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr =  rowStr + "[" + getValueJsonStr(keyValue[i]) + "," + getValueJsonStr(rowValue[i]) +"]"
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "]"
        else:
            rowStr = rowStr + "],"
    elif mode == 8:
        if rowIndex == 0:
            for i in xrange(1, len(keyValue)):
                rowStr = rowStr + getValueJsonStr(keyValue[i]) + ", "
        else:
            rowStr = None
    elif mode == 9 or mode == 16 or mode == 17 or mode == 18:
        rowStr = "["
        index = 0
        startCol = 1 if mode==18 else 0
        for i in xrange(startCol, len(rowValue)):
            if rowValue[i] != None:
                if index != 0: rowStr = rowStr + ", "
                rowStr = rowStr + getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "]"
        else:
            rowStr = rowStr + "],"
    elif mode == 10:
        if rowIndex != 0:rowStr = ", "
        for i in xrange(0, len(rowValue)):
            rowStr = rowStr + getValueJsonStr(rowValue[i])
            if i != len(rowValue) - 1:
                rowStr += ","
    elif mode == 11:
        rowStr = getKeyJsonStr(rowIndex + 1) + ":{"
        index = 0
        for i in xrange(0,len(rowValue)):
            if rowValue[i] != None:
                if index != 0:rowStr = rowStr + ", "
                rowStr = rowStr + getKeyJsonStr(keyValue[i]) + ":" +getValueJsonStr(rowValue[i])
                index = index + 1
        if isLastRow:
            rowStr = rowStr + "}"
        else:
            rowStr = rowStr + "},"
    elif mode == 12:
        # if rowIndex != 0:rowStr = ", "
        rowStr = getValueJsonStr(rowValue[1])
        if not isLastRow:
            rowStr += ","
    elif mode == 13:
        # if rowIndex != 0:rowStr = ", "
        rowStr = getValueJsonStr(rowValue[0])
        if not isLastRow:
            rowStr += ","
    elif mode == 14:
        # if rowIndex != 0:rowStr = ", "
        rowStr = getValueJsonStr(rowValue[0])
        if not isLastRow:
            rowStr += ","
    elif mode == 15:
        finalRowValue = []
        for i in xrange(0, len(rowValue)):
            if rowValue[i] == None:continue
            if isinstance(rowValue[i], list):
                for j in xrange(0, len(rowValue[i])):
                    finalRowValue.append(rowValue[i][j])
            else:
                finalRowValue.append(rowValue[i])
        for i in xrange(0, len(finalRowValue)):
            if i != 0:
                rowStr = rowStr + ", "
            rowStr += getValueJsonStr(finalRowValue[i])
    elif mode == 20:
        # if rowIndex != 0:rowStr = ", "
        finalRowValue = []
        for i in xrange(0, len(rowValue)):
            if rowValue[i] == None:continue
            if isinstance(rowValue[i], list):
                for j in xrange(0, len(rowValue[i])):
                    finalRowValue.append(rowValue[i][j])
            else:
                finalRowValue.append(rowValue[i])
        for i in xrange(0, len(finalRowValue)):
            if i != 0:
                rowStr = rowStr + ", "
            rowStr += getValueJsonStr(finalRowValue[i])
    return rowStr


def mergeKey(valueList, keyList):
    while len(keyList) > 0:
        mergeList = []
        keyName = None
        for i in xrange(0,len(keyList)):
            if keyName != None and keyList[i] != keyName:
                break
            for x in xrange(0,i):
                if keyList[i] == keyList[x]:
                    if keyName == None:
                        keyName = keyList[x]
                    if len(mergeList) == 0:
                        mergeList.append(x)
                    mergeList.append(i)
                    break
        if len(mergeList) > 0:
            delIndex = 0
            for i in xrange(1,len(mergeList)):
                del keyList[mergeList[i] - delIndex]
                delIndex = delIndex + 1

            for rowValue in valueList:
                mergeValues = [rowValue[mergeIndex] for mergeIndex in mergeList]
                delIndex = 0
                for mergeIndex in mergeList:
                    del rowValue[mergeIndex - delIndex]
                    delIndex = delIndex + 1
                rowValue.insert(mergeList[0], mergeValues)
        else:
            break

def getRelPath(name, d):
    if not name:
        name = ""
    return toString(os.path.normpath(os.path.join(d, name)))

def getOutPath(name, d):
    if not name:
        name = ""
    return toString(os.path.normpath(os.path.join(d, name)))

openBooks={}
def getSheetRowValueList(bookName, sheetName):
    book = openBooks.get(bookName)
    if not book:
        try:
            workbook = openpyxl.load_workbook(getRelPath(bookName, Workdir), read_only = True, data_only = True, keep_links = False)
            openBooks[bookName] = book = {"workbook":workbook,"sheets":{}}
        except Exception as e:
            print(e)
    rowValueList = book["sheets"].get(sheetName)
    if not rowValueList:
        sheetIndex = None
        if isinstance(sheetName, int):
            sheetIndex = sheetName - 1
            sheetName = book["workbook"].sheetnames[sheetIndex].encode("utf-8")
        sheet = book["workbook"][sheetName.decode("utf-8")]
        rowValueList = [[formatCellValue(cell) for cell in rowCells] for rowCells in [r for r in sheet.rows]]
        book["sheets"][sheetName] = rowValueList
        if sheetIndex : book["sheets"][sheetIndex] = rowValueList
    return rowValueList

def isEmptyRow(rowValue, colRange):
    if len(rowValue) <= 0:
        return True
    emptyCount = 0
    for col in colRange:
        if rowValue[col] == None:
            emptyCount+=1
    return emptyCount == len(colRange)


def convertTableToStr(luaStrList, table):
    spaceCount = 4
    if table["keyName"] == "*":
        spaceCount = 4
    else:
        spaceCount = 8
        luaStrList.append('    ' + getKeyStr(table["keyName"]) + '={\n')
    allowMergeKey = table["allowMergeKey"]
    isSwitchLine = not table.get("oneLine")
    if table.get("rowValueDict"):
        rowValueDict = table.get("rowValueDict")
        keyList = rowValueDict[0]
        del rowValueDict[0]
        noneKeyIndexList = []
        for i in xrange(len(keyList)-1, -1, -1):
            if keyList[i] == None:
                noneKeyIndexList.append(i)
        if len(noneKeyIndexList) > 0:
            for i in noneKeyIndexList:
                del keyList[i]
            for i in noneKeyIndexList:
                for key, rowValueList in rowValueDict.items():
                    for rowValue in rowValueList:
                        del rowValue[i]

        if allowMergeKey:
            lastKeyList = []
            for key, rowValueList in rowValueDict.items():
                cloneKeyList = keyList[:]
                mergeKey(rowValueList, cloneKeyList)
                lastKeyList = cloneKeyList
            keyList = lastKeyList

        for key in table.get("dictKeyList"):
            luaStrList.append("%*s={%s" % (len(getKeyStr(key)) + spaceCount, getKeyStr(key), "\n" if isSwitchLine else ""))
            rowIndex = 0
            for rowValue in rowValueDict[key]:
                luaStr = rowValueToLuaStr(rowValue, keyList, table["mode"], rowIndex, spaceCount)
                if luaStr:
                    luaStrList.append("%*s%s" % (len(luaStr) + (spaceCount + 4 if isSwitchLine else 0), luaStr, "\n" if isSwitchLine else ""))
                rowIndex += 1
            luaStrList.append("%*s},\n" % (spaceCount if isSwitchLine else 0, ""))
    else:
        rowValueList = table.get("rowValueList")
        keyList = rowValueList[0]
        if not table.get("keepRowKey"):
            del rowValueList[0]

        noneKeyIndexList = []
        for i in xrange(len(keyList)-1, -1, -1):
            if keyList[i] == None:
                noneKeyIndexList.append(i)
        if len(noneKeyIndexList) > 0:
            for i in noneKeyIndexList:
                del keyList[i]
            for i in noneKeyIndexList:
                for rowValue in rowValueList:
                    del rowValue[i]
        if allowMergeKey:
            mergeKey(rowValueList, keyList)
        rowIndex = 0
        for rowValue in rowValueList:
            luaStr = rowValueToLuaStr(rowValue, keyList, table["mode"], rowIndex, spaceCount)
            if luaStr:
                luaStrList.append("%*s\n" % (len(luaStr) + spaceCount, luaStr))
            rowIndex += 1
    if table["keyName"] != "*":
        luaStrList.append('    },\n')


def convertTableToJson(jsonStrList, table, subKey):
    spaceCount = 4
    headSpaceCount = 0
    if subKey != None:
        headSpaceCount += 4
    if table["keyName"] == "*":
        if len(jsonStrList) > 1:
            if jsonStrList[-1].endswith("\n"):
                if not jsonStrList[-1].endswith(":{\n") and not jsonStrList[-1].endswith(":[\n"):
                    jsonStrList[-1] = jsonStrList[-1][0:-1] +",\n"
            else:
                jsonStrList[-1] = jsonStrList[-1] +","
        spaceCount = 4
    else:
        if len(jsonStrList) > 1:
            if jsonStrList[-1].endswith("\n"):
                if not jsonStrList[-1].endswith(":{\n") and not jsonStrList[-1].endswith(":[\n"):
                    jsonStrList[-1] = jsonStrList[-1][0:-1] +",\n"
            else:
                jsonStrList[-1] = jsonStrList[-1] +","

        spaceCount = 8
        if table["isListObj"]:
            jsonStrList.append('%*s"%s":[\n' % (headSpaceCount+4, ' ', getKeyStr(table["keyName"])))
        else:
            jsonStrList.append('%*s"%s":{\n' % (headSpaceCount+4, ' ', getKeyStr(table["keyName"])))

    if subKey != None:
        spaceCount += headSpaceCount
    allowMergeKey = table["allowMergeKey"]
    isSwitchLine = not table.get("oneLine")
    if table.get("rowValueDict"):
        rowValueDict = table.get("rowValueDict")
        keyList = rowValueDict[0]
        del rowValueDict[0]
        noneKeyIndexList = []
        for i in xrange(len(keyList)-1, -1, -1):
            if keyList[i] == None:
                noneKeyIndexList.append(i)
        if len(noneKeyIndexList) > 0:
            for i in noneKeyIndexList:
                del keyList[i]
            for i in noneKeyIndexList:
                for key, rowValueList in rowValueDict.items():
                    for rowValue in rowValueList:
                        del rowValue[i]

        if allowMergeKey:
            lastKeyList = []
            for key, rowValueList in rowValueDict.items():
                cloneKeyList = keyList[:]
                mergeKey(rowValueList, cloneKeyList)
                lastKeyList = cloneKeyList
            keyList = lastKeyList

        for key in table.get("dictKeyList"):
            if table["isListObj"] or table["isSubList"]:
                jsonStrList.append("%*s:[%s" % (len(getKeyJsonStr(key)) + spaceCount, getKeyJsonStr(key), "\n" if isSwitchLine else ""))
            else:
                jsonStrList.append("%*s:{%s" % (len(getKeyJsonStr(key)) + spaceCount, getKeyJsonStr(key), "\n" if isSwitchLine else ""))
            rowIndex = 0
            if key in rowValueDict:
                rowValueList = rowValueDict[key]
                fromRow = 0
                if "fromRow" in table:
                    fromRow = table["fromRow"]
                for ri in xrange(fromRow, len(rowValueList)):
                    rowValue = rowValueList[ri]
                    luaStr = rowValueToJsonStr(rowValueList, rowValue, keyList, table["mode"], rowIndex, spaceCount, table)
                    if luaStr:
                        jsonStrList.append("%*s%s" % (len(luaStr) + (spaceCount + 4 if isSwitchLine else 0), luaStr, "\n" if isSwitchLine else ""))
                    rowIndex += 1
                if table["isListObj"] or table["isSubList"]:
                    jsonStrList.append("%*s],\n" % (spaceCount if isSwitchLine else 0, ""))
                else:
                    jsonStrList.append("%*s},\n" % (spaceCount if isSwitchLine else 0, ""))
        jsonStrList[-1] = jsonStrList[-1][0:-2] + "\n"
    else:
        rowValueList = table.get("rowValueList")
        keyList = rowValueList[0]
        if not table.get("keepRowKey"):
            del rowValueList[0]

        noneKeyIndexList = []
        for i in xrange(len(keyList)-1, -1, -1):
            if keyList[i] == None:
                noneKeyIndexList.append(i)
        if len(noneKeyIndexList) > 0:
            for i in noneKeyIndexList:
                del keyList[i]
            for i in noneKeyIndexList:
                for rowValue in rowValueList:
                    if i < len(rowValue):del rowValue[i]
        if allowMergeKey:
            mergeKey(rowValueList, keyList)
        rowIndex = 0
        fromRow = 0
        if "fromRow" in table:
            fromRow = table["fromRow"]
        while fromRow > 0:
            del rowValueList[0]
            fromRow = fromRow-1
        if table["mode"] == 20:
            rowValueList = rowValueList[0:1]
        if table["mode"] == 15:
            rowValueList = [table["rowHeadValueList"]]
            del rowValueList[0][0]
        if table["mode"] == 16:
            for rowValue in rowValueList:
                del rowValue[0]
        for rowValue in rowValueList:
            luaStr = rowValueToJsonStr(rowValueList, rowValue, keyList, table["mode"], rowIndex, spaceCount, table)
            if luaStr:
                jsonStrList.append("%*s\n" % (len(luaStr) + spaceCount, luaStr))
            rowIndex += 1
    if table["keyName"] != "*":
        if table["isListObj"]:
            jsonStrList.append('%*s]\n' % (4+headSpaceCount, ' '))
        else:
            jsonStrList.append('%*s}\n' % (4+headSpaceCount, ' '))

def formatTable(table):
    keyCounts = {}
    dictKeyList = []
    rowValueList = table["rowValueList"]
    needMerge = False
    if table["mode"] > 90 and table["mode"] <= 99:
        table["fromRow"] = table["mode"] - 90
        table["mode"] = 9
    if table["mode"] > 20 and table["mode"] <= 29:
        table["fromRow"] = table["mode"] - 20
        table["mode"] = 2
    if table["mode"] != 2:
        for i in xrange(1,len(rowValueList)):
            key = rowValueList[i][0]
            if key in keyCounts:
                needMerge = True
                keyCounts[key] += 1
            else:
                keyCounts[key] = 1
                dictKeyList.append(key)
    needMerge = False
    if needMerge or table["mode"] == 10 or table["mode"] == 11:
        rowValueDict = {}
        for key in dictKeyList:
            rowValueDict[key] = []
        for i in xrange(1,len(rowValueList)):
            rowValue = rowValueList[i]
            key = rowValue[0]
            del rowValue[0]
            rowValueDict[key].append(rowValue)

        del rowValueList[0][0]
        rowValueDict[0] = rowValueList[0]
        table["rowValueDict"] = rowValueDict
        table["dictKeyList"] = dictKeyList
    if not table.get("mode"):
        table["mode"] = 0
    table["oneLine"] = table["mode"] in OneLineMode
    table["allowMergeKey"] = table["mode"] not in NotAllowMergeKey
    table["keepRowKey"] = table["mode"] in KeepRowKeyMode
    table["isSubList"] = table["mode"] in SubListMode

    return table

def getExcelValue(bookName, sheetName, colName, isSwitchRowCol):
    sheetRowValueList = getSheetRowValueList(bookName, sheetName)
    firstRowValues = sheetRowValueList[0]
    colRange = []
    startCol = 0
    if isinstance(colName, str):
        for col in xrange(0,len(firstRowValues)):
            if firstRowValues[col] == colName:
                startCol = col + 1
                break
        if startCol == 0:
            raise(Exception(toString("找不到列名："+colName + "  在表中 <"+bookName+" . "+sheetName+"> ")))
    for col in xrange(startCol,len(firstRowValues)):
        if firstRowValues[col] != None and "//" not in str(firstRowValues[col]):
            colRange.append(col)
        else:
            break

    rowValueList = []
    rowHeadValueList = []
    sheetRowValue = sheetRowValueList[0]
    if not isEmptyRow(sheetRowValue, colRange):
        rowHeadValueList = [sheetRowValue[col] for col in colRange]

    for row in xrange(1,len(sheetRowValueList)):
        sheetRowValue = sheetRowValueList[row]
        if not isEmptyRow(sheetRowValue, colRange) and len(sheetRowValue) > 0:
            rowValue = [sheetRowValue[col] for col in colRange]
            rowValueList.append(rowValue)

    if isSwitchRowCol:
        valueList = []
        ncol = len(rowValueList[0])
        nrow = len(rowValueList)
        for col in xrange(0, ncol):
            rowValue = []
            for row in xrange(0, nrow):
                rowValue.append(rowValueList[row][col])
            valueList.append(rowValue)
        rowValueList = valueList
    return rowValueList, rowHeadValueList

def exportToLuaStr(configItem):
    luaFileName = configItem[0]
    luaTableList = []
    for info in configItem[1]:
        tableKeyName, bookName, sheetName, colName, mode, isSwitchRowCol = info[1], info[2], info[3], info[4], info[5], info[6]
        print(toLogString(str(info[0]) + ": " + ", ".join((luaFileName, tableKeyName, bookName, sheetName))))
        excelValue = getExcelValue(bookName, sheetName, colName, isSwitchRowCol)
        luaTableList.append(formatTable({"keyName":tableKeyName, "mode":mode, "rowValueList":excelValue}))

    luaStrList = []
    luaStrList.append("return {\n")
    for table in luaTableList:
        convertTableToStr(luaStrList, table)
    luaStrList.append("}\n")
    return luaStrList


def exportToJsonStr(configItem):
    jsonFileName = configItem[0]
    jsonObjList = []
    isList = True
    subKeyLists = {}
    for info in configItem[1]:
        tableKeyName, bookName, sheetName, colName, mode, isSwitchRowCol, curBookName = info[1], info[2], info[3], info[4], info[5], info[6], info[7]
        if bookName == None or bookName == "":
            bookName = curBookName
        print(toLogString(str(info[0]) + ": " + ", ".join((jsonFileName, tableKeyName, bookName, sheetName))))
        subKey = None
        if '.' in tableKeyName:
            subKey = tableKeyName[0:tableKeyName.index('.')]
            tableKeyName = tableKeyName[tableKeyName.index('.')+1:len(tableKeyName)]

        t = {"keyName":tableKeyName, "mode":mode, "colName":colName}
        if mode == 1100:
            colName = 1
        rowValueList, rowHeadValueList = getExcelValue(bookName, sheetName, colName, isSwitchRowCol)
        t["rowValueList"] = rowValueList
        t["rowHeadValueList"] = rowHeadValueList
        t = formatTable(t)
        m = t["mode"]
        t["isListObj"] = m in ListMode
        if subKey:
            if subKey not in subKeyLists:
                subKeyLists[subKey]=[]
                jsonObjList.append({"subKey":subKey, "objList":subKeyLists[subKey]})
            subKeyLists[subKey].append(t)
        else:
            jsonObjList.append(t)
        if tableKeyName != "*" or not t["isListObj"]:
            isList = False

    jsonStrList = []
    if isList:
        jsonStrList.append("[\n")
    else:
        jsonStrList.append("{\n")
    for i in xrange(0, len(jsonObjList)):
        table = jsonObjList[i]
        if "subKey" in table:
            if len(jsonStrList) > 1:
                if jsonStrList[-1].endswith("\n"):
                    jsonStrList[-1] = jsonStrList[-1][0:-1] +",\n"
                else:
                    jsonStrList[-1] = jsonStrList[-1] +","
            subIsList = len(table["objList"]) == 1 and table["objList"][0]["keyName"] == "*" and table["objList"][0]["isListObj"]
            jsonStrList.append(('    "%s":[\n'if subIsList else '    "%s":{\n') % table["subKey"])
            for subItem in table["objList"]:
                convertTableToJson(jsonStrList, subItem, table["subKey"])
            jsonStrList.append("    ]\n" if subIsList else "    }\n")
        else:
            convertTableToJson(jsonStrList, table, None)
    if isList:
        jsonStrList.append("]\n")
    else:
        jsonStrList.append("}\n")
    return jsonStrList


outExportConfig = []
outFileNameKeys = {}
def convertConfig(config, exportFormat, outDir, excelName):
    for i in xrange(1,len(config)):
        rowValue = config[i]
        if rowValue[0] and rowValue[1] and rowValue[2]:
            luaKeyList = None
            luaName = rowValue[1]
            del rowValue[1]
            if luaName in outFileNameKeys:
                luaKeyList = outFileNameKeys[luaName]
            else:
                luaKeyList = []
                outFileNameKeys[luaName] = luaKeyList
                outExportConfig.append((luaName, luaKeyList, exportFormat, outDir))
            rowValue.append(excelName)
            while len(rowValue) < 8:rowValue.append(excelName)
            rowValue[7] = excelName
            luaKeyList.append(rowValue)
    return outExportConfig


exportedBooks={}
def export():
    for item in outExportConfig:
        outDir = item[3]
        if item[2] == "lua":
            luaStrList = exportToLuaStr(item)
            if luaStrList:
                luaFile = open(getOutPath(item[0] + ".lua", outDir), "wb")
                luaFile.writelines(luaStrList)
                luaFile.close()
        elif item[2] == "json":
            jsonStrList = exportToJsonStr(item)
            if jsonStrList:
                jsonFile = open(getOutPath(item[0] + ".json", outDir), "wb")
                jsonFile.writelines(jsonStrList)
                jsonFile.close()

def addToExportList(excelFullPath):
    if excelFullPath in exportedBooks:
        return
    print(toLogString(excelFullPath))
    excelName = os.path.basename(excelFullPath)
    configRowValueList = getSheetRowValueList(excelName, 1)
    outDir = getRelPath(configRowValueList[1][configRowValueList[0].index("导出路径")], Workdir)
    exportFormat = "lua"
    if "导出格式" in configRowValueList[0]:
        exportFormat = configRowValueList[1][configRowValueList[0].index("导出格式")]
    exportedBooks[excelFullPath] = 1
    linkList = []
    if "链接表格" in configRowValueList[0]:
        linkIndex = configRowValueList[0].index("链接表格")
        for i in xrange(1,1000):
            if i < len(configRowValueList) and configRowValueList[i][linkIndex]:
                linkList.append(getRelPath(configRowValueList[i][linkIndex], Workdir))
            else:
                break

    if "导出路径" != configRowValueList[0][0]:
        if not os.path.exists(outDir):os.makedirs(outDir)
        convertConfig(configRowValueList, exportFormat, outDir, excelName)
    print("")

    for linkPath in linkList:
        addToExportList(linkPath.decode("gbk").encode("utf-8"))


def parseExportPath(fileName):
    currentPath = os.path.split(os.path.realpath(__file__))[0]
    if isWindows:
        xlsxPath = os.path.normpath(os.path.join(currentPath, fileName.encode("gbk"))).decode("gbk")
        xlsxPath2 = os.path.normpath(fileName.encode("gbk")).decode("gbk")
    else:
        xlsxPath = os.path.normpath(os.path.join(currentPath, fileName))
        xlsxPath2 = os.path.normpath(fileName)

    excelFullPath = ""
    if os.path.exists(xlsxPath):
        excelFullPath = xlsxPath
    elif os.path.exists(xlsxPath2):
        excelFullPath = xlsxPath2

    if isWindows:
        excelFullPath = excelFullPath.encode("utf-8")
    global Workdir
    excelFullPath = os.path.normpath(excelFullPath)
    Workdir = os.path.dirname(excelFullPath)
    addToExportList(excelFullPath)

def isArgIsXlsxFile(argv, index):
    if len(argv) <= index:
        return False
    fileName = argv[index]
    if isWindows:
        xlsxPath = os.path.normpath(os.path.join(currentPath, fileName.encode("gbk"))).decode("gbk")
    else:
        xlsxPath = os.path.normpath(fileName)
    if os.path.exists(xlsxPath) and fileName.endswith(".xlsx"):
        return True
    return False

if __name__ == '__main__':
    try:
        if len(sys.argv) <= 2:
            if len(sys.argv) > 1:
                IsFromUnity = sys.argv[1] == "unity"

            if isArgIsXlsxFile(sys.argv, 1):
                parseExportPath(sys.argv[1])
            else:
                currentPath = os.path.split(os.path.realpath(__file__))[0]
                fileList = os.listdir(currentPath)
                def fileNameComp(elemA, elemB):
                    lenA =  len(elemA)
                    lenB =  len(elemB)
                    if lenA > lenB: return 1
                    if lenA < lenB: return -1
                    return cmp(elemA, elemB)
                fileList.sort(fileNameComp)
                for fileName in fileList:
                    if fileName.endswith(toString("数值总表.xlsx")) and not fileName.startswith("~$") and not fileName.startswith("._") and not fileName.startswith(".~"):
                        if isWindows:
                            parseExportPath(fileName.decode('gbk'))
                        else:
                            parseExportPath(fileName)
            export()
        elif len(sys.argv) >= 3:
            for i in xrange(1, len(sys.argv)):
                if isArgIsXlsxFile(sys.argv, i):
                    parseExportPath(sys.argv[i])
            export()
    except Exception:
        import traceback
        traceback.print_exc()
    finally:
        pass
        # os.system('pause()')
        # raw_input("enter to continue!")
else:
    pass
