FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-alpine3.18-amd64 AS base
USER root
RUN apk add --no-cache tzdata
ENV TZ=Asia/Shanghai
RUN cp /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM node as builder
WORKDIR /src
COPY web .
RUN yarn config set strict-ssl false
RUN npm config set registry https://registry.npmmirror.com
RUN yarn
RUN yarn run build


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
COPY ["src/AIDotNet.API.Service/AIDotNet.API.Service.csproj", "src/AIDotNet.API.Service/"]
COPY ["src/extensions/AIDotNet.Qiansail/AIDotNet.Qiansail.csproj", "src/extensions/AIDotNet.Qiansail/"]
COPY ["src/AIDotNet.Abstractions/AIDotNet.Abstractions.csproj", "src/AIDotNet.Abstractions/"]
COPY ["src/extensions/AIDotNet.MetaGLM/AIDotNet.MetaGLM.csproj", "src/extensions/AIDotNet.MetaGLM/"]
COPY ["src/extensions/AIDotNet.OpenAI/AIDotNet.OpenAI.csproj", "src/extensions/AIDotNet.OpenAI/"]
COPY ["src/extensions/AIDotNet.SparkDesk/AIDotNet.SparkDesk.csproj", "src/extensions/AIDotNet.SparkDesk/"]
COPY ["src/extensions/AIDotNet.Claudia/AIDotNet.Claudia.csproj", "src/extensions/AIDotNet.Claudia/"]
RUN dotnet restore "src/AIDotNet.API.Service/AIDotNet.API.Service.csproj"
COPY . .
WORKDIR "/src/src/AIDotNet.API.Service"
RUN dotnet build "AIDotNet.API.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AIDotNet.API.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=builder /src/dist ./wwwroot
COPY --from=lobe /src/dist ./lobe
ENTRYPOINT ["dotnet", "AIDotNet.API.Service.dll"]
