# BP-Microservices

## Journal

### Saturday 15/03/2025

I created the first service, which is the ProductService. The service uses a local SQL Server database for development. Then when this service will run in Docker and Kubernetes there will be a new SQL Server database specifically for production environment.

In addition, this first simple service contains 2 endpoints. The first endpoint is a GetProduct that retrieves all products. A second endpoint is the GetProductById that retrieves a specific product. The service uses AutoMapper to easily convert internal objects to DTOs.

### Sunday 16/03/2025

Docker will be an important part of this PoC, so I did some learning in a new project to work with Docker.

I tried to create a Docker image based on the new project to then set up a Docker container. I then tried to deploy this container in Kubernetes.

In addition, I also tried to get the new project to work with SQL Server and then transfer this to a Docker container as well. However, this did not go so well. There were some problems regarding Docker and the connection to the local SQL Server database.

### Friday 21/03/2025

Today I decided to move away from the SQL Server database for in development. This is because there were problems with the connection between Docker and SQL Server and given my limited knowledge of Docker, I have switched to an InMemory database for simplicity. Once the services will be deployed in Kubernetes a SQL Server database will be set up in Kubernetes though.

Before I started programming further I took some time today to make a visual representation of what I plan to develop. Since this is a proof-of-concept, and therefore should not be a too large application, I focused on 3 core features for an e-commerce that can also represent the way microservices work.

![image](https://github.com/user-attachments/assets/7e2afaf2-07a5-447d-9888-3b77f2f6f855)

### Sunday 23/03/2025

Today I have been working on the ShoppingCartService, which will be responsible for managing shopping carts. 

I also set up synchronous communication between the ProductService and the ShoppingCartService.

### Tuesday 25/03/2025

First, I wanted to keep the app fairly simple and avoid an AuthenticationService, but the more I thought about it the more I realised that I did need that service. So after the internship I did some research on how to simply add authentication to your microservice.

### Wednesday 26/03/2025

I have started a new separate little project to do some testing with authentication and microservices. An article I found described how to use basic-auth annotation in conjunction with a Kubernetes secret. So I implemented this and got good results. The first request that came in was blocked by the Nginx Ingress controller. It then asked for the username and password that matched the Kubernetes secret.

This method of authentication worked, however only 1 user could log in.... So I was going to try working perhaps with Auth0.

### Thursday 27/03/2025

After visiting the graduate fair in Ghent, I went back to work and set up an application and api in Auth0. I then tried to link this to the Nginx Ingress controller, so that each request was checked for a valid JWT token, if not it had to be referred to the default Auth0 login page.

This method of authentication did not work well. Auth0 needed a public callback url after login and my microservices were growing locally which caused problems.

### Friday 28/03/2025

Today I tried to set up a simple authentication service myself with Node JS. Then I configured the Nginx Ingress controller to do validation with my authentication service and, if validated correctly, to forward the userId and username in the response headers to the underlying service.

This approach works well and gives good results. Tomorrow I then want to look at setting this up in my project as well.

![image](https://github.com/user-attachments/assets/cc7d44e3-5c40-470d-a071-4a58c9838c24)
