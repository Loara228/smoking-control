#!/bin/bash

# Usage: api http [OPTIONS]

# Options:
#   -p, --port <PORT>  [default: 8080]

source ./conf.sh

cd ../api
cargo run -- --addr $SRV_IP http --port $SRV_PORT