# Create WSL context
docker context create wsl --description "Docker CLI for WSL" --docker "host=tcp://127.0.0.1:2375"

# List docker contexts, you should see the new context wsl and the default context
docker context ls

# Switch to WSL context
docker context use wsl

# Test the context by running a simple command
docker run hello-world

# Switch back to default context
docker context use default

# Test the context by running a simple command
docker run hello-world
