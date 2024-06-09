# Optionally enable required Windows features if needed
Enable-WindowsOptionalFeature -Online -FeatureName Containers –All
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V –All

# Download and install Docker CE for Windows Containers
curl.exe -o docker.zip -LO https://download.docker.com/win/static/stable/x86_64/docker-26.1.3.zip
Expand-Archive docker.zip -DestinationPath C:\

# Set the PATH environment variable
[Environment]::SetEnvironmentVariable("Path", "$($env:path);C:\docker", [System.EnvironmentVariableTarget]::Machine)
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine")

# Register dockerd as a service
dockerd --register-service
Start-Service docker
docker run hello-world
