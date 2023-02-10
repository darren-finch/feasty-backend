FROM --platform=linux/amd64 eclipse-temurin:17-jre-alpine
MAINTAINER darrenfinch
ENTRYPOINT ["java","-jar","/feasty-web-1.jar"]