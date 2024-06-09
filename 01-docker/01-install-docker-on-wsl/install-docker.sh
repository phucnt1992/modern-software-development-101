#!/bin/bash

# Get script directory
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

# Update all packages
echo "Updating all packages..."
sudo apt update; sudo apt full-upgrade -y; sudo apt autoremove -y; sudo apt autoclean;

# Install Docker, Docker Compose and Docker Machine
# Reference to https://docs.docker.com/engine/install/ubuntu/

echo "Installing Docker, Docker Compose and Docker Machine..."
# Add Docker's official GPG key
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc;

# Add the repository to Apt sources:
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt update

# Install Docker pakages
sudo apt install -y \
    docker-ce \
    docker-ce-cli \
    containerd.io \
    docker-buildx-plugin \
    docker-compose-plugin;

# Post installation steps
# Reference to https://docs.docker.com/engine/install/linux-postinstall/

echo "Post installation steps..."

# Manage Docker as a non-root user
sudo groupadd docker
sudo usermod -aG docker $USER
newgrp docker

# Fix permissions
sudo chown "$USER":"$USER" /home/"$USER"/.docker -R
sudo chmod g+rwx "$HOME/.docker" -R

# Configure Docker to start on boot
echo "Configuring Docker to start on boot..."
sudo systemctl enable docker.service
sudo systemctl enable containerd.service

echo "Enable daemon.json config.."
# Config Docker daemon
# Reference to https://docs.docker.com/reference/cli/dockerd/#daemon-configuration-file
sudo mkdir -p /etc/docker
sudo cp $DIR/daemon.json /etc/docker/daemon.json
sudo chmod 0644 /etc/docker/daemon.json

# Overridde dockerd to allow daemon.json config
sudo mkdir -p /etc/systemd/system/docker.service.d
sudo cp $DIR/override.conf /etc/systemd/system/docker.service.d/override.conf
sudo chmod 0644 /etc/systemd/system/docker.service.d/override.conf

# Restart Docker service
sudo systemctl daemon-reload
sudo systemctl restart docker.service

# Test docker
echo "Testing Docker installation..."
docker run hello-world
