# Usage of Docker Engine and Docker Desktop with this Project

1. Docker Engine
2. Docker Desktop

## Docker Engine:
To use Docker Engine on Linux to run this project locally you need to follow these steps:

1. Start Docker Engine with sudo systemctl start docker.service
1. pull the docker hub repository with *docker pull manofward/blazorapp1:latest*
1. run the container with *docker run -d -p 3000:8080 manofward/blazorapp1:latest*
1. open your browser and navigate to http://localhost:3000
1. to stop the container use *docker ps* to get the container id and then use *docker stop <container_id>*

tips:
you can use *docker logs <container_id>* to see the logs of the container

## Docker Desktop:
To use Docker Desktop on Windows to run this project locally you need to follow these steps:

1. Start Docker Desktop
1. pull the docker hub repository with *docker pull manofward/blazorapp1:latest*
1. go to the images tab and find the image you just pulled
1. click on the run button to start a new container and give the container a different port to use for webservers (3000 for example)
1. open your browser and navigate to http://localhost:3000
1. to stop the container go to the containers tab and click on the stop button for the container you want to stop
1. to see the logs of the container click on the logs button for the container you want to see the logs for
1. to remove the container click on the delete button for the container you want to remove
1. to remove the image go to the images tab and click on the delete button for the image you want to remove
1. to update the image pull the latest version from docker hub and then stop and remove the old container and start a new container with the new image

https://hub.docker.com/repository/docker/manofward/blazorapp1/general