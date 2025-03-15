# BP-Microservices

## Journal

### Saturday 15/03/2025

I created the first service, which is the ProductService. The service uses a local SQL Server database for development. Then when this service will run in Docker and Kubernetes there will be a new SQL Server database specifically for production environment.

In addition, this first simple service contains 2 endpoints. The first endpoint is a GetProduct that retrieves all products. A second endpoint is the GetProductById that retrieves a specific product. The service uses AutoMapper to easily convert internal objects to DTOs.
