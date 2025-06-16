#!/bin/bash

source ./conf.sh
./conf_maui.sh

cd ../smoking_control
dotnet publish -f net9.0-android -c Release