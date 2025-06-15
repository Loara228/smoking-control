#!/bin/bash

source ./conf.sh
./conf_maui.sh

cd ../smoking_control
dotnet publish -f net9.0-windows10.0.19041.0 -c Debug -p:PublishReadyToRun=true -p:WindowsPackageType=None
# "bin\Debug\net9.0-windows10.0.19041.0\win10-x64\publish\smoking_control.exe