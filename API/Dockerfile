FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

# copy csproj and restore first for better caching
COPY ["API.csproj", "./"]
RUN dotnet restore "API.csproj"

# copy rest of the app
COPY . .

# listen on port 5000 inside container
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# use dotnet watch for fast development
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]
