#!/bin/bash

source ./conf.sh

cd ../api
cargo run -- --addr $SERVER_IP http --port $SERVER_PORT