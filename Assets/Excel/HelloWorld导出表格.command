#!/bin/bash
# work_path=$(pwd)
# cd ~/${work_path}
work_path=$(dirname $0)
python ${work_path}/XlsxToJson.py
echo press any key to continue
read -n 1
