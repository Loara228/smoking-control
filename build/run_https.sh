#!/bin/bash

# Usage: api https --key <PATH> --sert <PATH>

# Options:
#   -k, --key <PATH>   
#   -s, --sert <PATH>  

source ./conf.sh

cd ../api
cargo run -- --addr $SRV_IP https --key $SRV_KEY --sert $SRV_SERT