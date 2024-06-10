# !!CAUTION: To run this script, you need to have Administrator rights
# This script will enable WSL2 on Windows 10 and install Ubuntu LTS from Microsoft Store.
# It will also set WSL2 as default version and upgrade the latest WSL2 Linux Kernel.

# Get the directory of the script
$ScriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Enable WSL on Windows features
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Windows-Subsystem-Linux

# Enable Virtual Machine Platform on Windows features
Enable-WindowsOptionalFeature -Online -FeatureName VirtualMachinePlatform

# Restart your computer to apply changes if needed

# Set WSL2 as default version
wsl --set-default-version 2

# Install Ubuntu stable LTS from Microsoft Store
# NOTE: we avoid using latest version to ensure compatibility with our tools on WSL2
wsl --install Ubuntu

# Upgrade latest WSL2 Linux Kernel
wsl --update --pre-release

# Test WSL2 version
wsl --version

# Setup wslconfig to USERPROFILE
# Refrence https://learn.microsoft.com/en-us/windows/wsl/wsl-config
cp $ScriptDirectory/.wslconfig $env:USERPROFILE/.wslconfig

# Start WSL2 and setup account
wsl -d Ubuntu
