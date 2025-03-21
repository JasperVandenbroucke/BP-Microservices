# BP-Microservices

## Journal

### Saturday 15/03/2025

I created the first service, which is the ProductService. The service uses a local SQL Server database for development. Then when this service will run in Docker and Kubernetes there will be a new SQL Server database specifically for production environment.

In addition, this first simple service contains 2 endpoints. The first endpoint is a GetProduct that retrieves all products. A second endpoint is the GetProductById that retrieves a specific product. The service uses AutoMapper to easily convert internal objects to DTOs.

### Sunday 16/03/2025

Docker will be an important part of this PoC, so I did some learning in a new project to work with Docker.

I tried to create a Docker image based on the new project to then set up a Docker container. I then tried to deploy this container in Kubernetes.

In addition, I also tried to get the new project to work with SQL Server and then transfer this to a Docker container as well. However, this did not go so well. There were some problems regarding Docker and the connection to the local SQL Server database.
