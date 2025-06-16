#!/bin/bash

export SRV_IP="0.0.0.0"
export SRV_HOSTNAME="192.168.0.148"
export SRV_PORT="8081"
export SRV_USE_SSL=false

export SRV_KEY="/home/user/code/smoking-control/build/secret/key.pem"
export SRV_CERT="/home/user/code/smoking-control/build/secret/sert.pem"


echo "IP: $SRV_IP"
echo "HOSTNAME: $SRV_HOSTNAME"
echo "PORT: $SRV_PORT"
echo "USE_SSL: $SRV_USE_SSL"