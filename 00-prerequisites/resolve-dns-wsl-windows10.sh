#!/bin/bash
# Because DNS tunneling feature is only available on Windows 11
# We must run these steps to resolve DNS in WSL2 on Windows 10

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

# Apply wsl.config to disable network generate resolv config
cp $DIR/wsl.conf /etc/wsl.conf
sudo chown root:root /etc/wsl.conf

# Restart WSL
wsl.exe --shutdown

# Apply DNS to WSL
sudo bash -c "cat <<EOF >> /etc/resolv.conf
nameserver 8.8.8.8
nameserver 8.8.4.4
nameserver 1.1.1.1
nameserver 1.0.0.1
EOF"

# Test DNS
nslookup google.com

