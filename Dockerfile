FROM --platform=linux/amd64 eclipse-temurin:17-jre-alpine
MAINTAINER darrenfinch
COPY target/feasty-web-1.jar feasty-web-1.jar
ENTRYPOINT ["java","-jar","/feasty-web-1.jar"]