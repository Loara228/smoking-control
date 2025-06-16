#!/bin/bash

source ./conf.sh

if [ "$SRV_USE_SSL" = true ]; then
    protocol="https"
else
    protocol="http"
fi

cd "../smoking_control/smoking control/"
cat << EOF > CONSTANTS.cs
public static class CONSTANTS {
    public static string PROTOCOL = "$protocol";
    public static string HOSTNAME = "$SRV_HOSTNAME";
    public static UInt16 PORT = $SRV_PORT;
}
EOF
cat CONSTANTS.cs