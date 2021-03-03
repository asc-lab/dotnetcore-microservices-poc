FROM gradle:6.8.3-jdk8 AS build
COPY --chown=gradle:gradle . /home/gradle/src
WORKDIR /home/gradle/src
RUN gradle build --no-daemon 

FROM openjdk:8-jre-slim

EXPOSE 8761

RUN mkdir /app

COPY --from=build /home/gradle/src/build/libs/*.jar /app/eureka-server.jar

ENTRYPOINT ["java","-jar","/app/eureka-server.jar", "--server.port=8761"]