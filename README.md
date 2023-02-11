<!-- ABOUT THE PROJECT -->
# Distributed caching in ASP.NET Core

A distributed cache is a cache shared by multiple app servers, typically maintained as an external service to the app servers that access it. A distributed cache can improve the performance and scalability of an ASP.NET Core app, especially when the app is hosted by a cloud service or a server farm.

A distributed cache has several advantages over other caching scenarios where cached data is stored on individual app servers.

When cached data is distributed, the data:

Is coherent (consistent) across requests to multiple servers.
Survives server restarts and app deployments.
Doesn't use local memory.


<!-- GETTING STARTED -->
## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites

Things you need to use the software and how to install them.
* [Visual Studio / Visual Studio Code](https://visualstudio.microsoft.com/)
* [.NET 7](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/)
* [Docker](https://www.docker.com/)

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/gitViwe/distributed-caching.git
   ```
2. Run via Docker
   ```
   cd distributed-caching
   docker compose up -d
   ```

### Then navigate to [http://localhost:5034/swagger](http://localhost:5034/swagger)
