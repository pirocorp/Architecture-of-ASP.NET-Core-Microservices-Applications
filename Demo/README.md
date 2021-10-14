# Creating Docker Compose File

Defines version of the yml file

```yml
version: "3.8"
```

Services are all managed containers

```yml
services:
```

Networks are used to allow comunications between containers. Only containers in one network can comunicate with each other freely.

```yml
networks:
```

Volumes are persistance storages for containers because containers are immutable.
```yml
volumes:
```
