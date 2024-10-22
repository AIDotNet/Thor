FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-amd64 AS base
USER root
RUN apk add --no-cache tzdata
ENV TZ=Asia/Shanghai
RUN cp /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node as lobe
WORKDIR /src
COPY lobe .
RUN npm config set registry https://registry.npmmirror.com
RUN yarn
RUN npm i
RUN yarn run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Thor.Service/Thor.Service.csproj", "src/Thor.Service/"]
COPY ["src/extensions/Thor.AzureOpenAI/Thor.AzureOpenAI.csproj", "src/extensions/Thor.AzureOpenAI/"]
COPY ["src/Thor.Abstractions/Thor.Abstractions.csproj", "src/Thor.Abstractions/"]
COPY ["src/extensions/Thor.Claudia/Thor.Claudia.csproj", "src/extensions/Thor.Claudia/"]
COPY ["src/extensions/Thor.ErnieBot/Thor.ErnieBot.csproj", "src/extensions/Thor.ErnieBot/"]
COPY ["src/extensions/Thor.Hunyuan/Thor.Hunyuan.csproj", "src/extensions/Thor.Hunyuan/"]
COPY ["src/extensions/Thor.MetaGLM/Thor.MetaGLM.csproj", "src/extensions/Thor.MetaGLM/"]
COPY ["src/extensions/Thor.Ollama/Thor.Ollama.csproj", "src/extensions/Thor.Ollama/"]
COPY ["src/extensions/Thor.OpenAI/Thor.OpenAI.csproj", "src/extensions/Thor.OpenAI/"]
COPY ["src/extensions/Thor.Qiansail/Thor.Qiansail.csproj", "src/extensions/Thor.Qiansail/"]
COPY ["src/extensions/Thor.SparkDesk/Thor.SparkDesk.csproj", "src/extensions/Thor.SparkDesk/"]
COPY ["src/framework/Thor.BuildingBlocks.Event/Thor.BuildingBlocks.Event.csproj", "src/framework/Thor.BuildingBlocks.Event/"]
COPY ["src/framework/Thor.LocalEvent/Thor.LocalEvent.csproj", "src/framework/Thor.LocalEvent/"]
COPY ["src/framework/Thor.BuildingBlocks.Cache/Thor.BuildingBlocks.Cache.csproj", "src/framework/Thor.BuildingBlocks.Cache/"]
COPY ["src/framework/Thor.LocalMemory.Cache/Thor.LocalMemory.Cache.csproj", "src/framework/Thor.LocalMemory.Cache/"]
COPY ["src/framework/Thor.RedisMemory.Cache/Thor.RedisMemory.Cache.csproj", "src/framework/Thor.RedisMemory.Cache/"]
COPY ["src/framework/Thor.Analyzers/Thor.Analyzers.csproj", "src/framework/Thor.Analyzers/"]
RUN dotnet restore "src/Thor.Service/Thor.Service.csproj"
COPY . .
WORKDIR "/src/src/Thor.Service"
RUN dotnet build "Thor.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Thor.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=lobe /src/dist ./wwwroot
ENTRYPOINT ["dotnet", "Thor.Service.dll"]
