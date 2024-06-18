# SimpleHealthcheckApi README
## Do all in one
```sh
docker stop simple-health-check-api-container |
docker rm simple-health-check-api-container |
docker build -f src/SimpleHealthcheckApi/Dockerfile -t simple-health-check-api-image . |
docker run -d -p 8000:80 --name simple-health-check-api-container simple-health-check-api-image
```

## Stop running Container & Remove Container
```sh
docker stop simple-health-check-api-container;docker rm simple-health-check-api-container
```

## Create Docker Image
```sh
docker build -f src/SimpleHealthcheckApi/Dockerfile -t simple-health-check-api-image .
```

## Create & Start Docker Containter
```sh
docker run -d -p 8000:80 --name simple-health-check-api-container simple-health-check-api-image
```
