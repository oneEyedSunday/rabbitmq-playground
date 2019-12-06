## RabbitMQ quickstart

Tutorials from [here](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html).
The consumer is located in `Receive` while the producer is located in `Send`

### Docker container
You may want to just use a dockerized rabbitMQ server
```sh
docker pull rabbitmq
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

To test open two terminals, then run:
```sh
cd Receive
dotnet run
```

then in the second terminal

```sh
cd Send
dotnet run
```


## TODOs
[] Accept messages to broadcast from command line

[] Try sending objects other than strings
