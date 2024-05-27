# Building Qwitter V2

In the new version of Qwitter there are 4 dotnet microservices
- [qwitter-users](./qwitter-users/)
- [qwitter-ledger](./qwitter-ledger)
- [qwitter-payments](./qwitter-payments/)
- [qwitter-content](./qwitter-content/)

There is also a frontend react app [qwitter-client](./qwitter-client/), and a infrastructure class library [qwitter-core/Qwitter.Core.Application](./qwitter-core/Qwitter.Core.Application/).

The frontend currently only implements features from qwitter-user and qwitter-ledger while qwitter-payments and qwitter-content are only accessible through swagger.

There is a lot of legacy code in this repository, such as [server](./server/) and [client](./client/) which is the old version of qwitter created in 2022 that is where the pictures from the readme is from. The folders that begin with a capital Q in Qwitter is also legacy and can be ignored.

## Requirements
- [Docker](https://docs.docker.com/engine/install/)
- [Node and npm for React](https://nodejs.org/en/download/prebuilt-installer)
- [.NET Runtime and .NET SDK](https://dotnet.microsoft.com/en-us/download)

Once you have docker installed you need to create a Microsoft SQL Server container and a Kafka container, this can be done with
```
docker compose up -d
```
Once you have these containers up and running you need to create Kafka topics for the events. Although this should not be necessary since Masstransit should create topic for you when they are produced, but sometimes they don't get created automatically and you need to add them manually. To see what events are being produced you can search the repository for the prefix ``[Message("``. This attribute is defined in the infrastructure library [here](qwitter-core\Qwitter.Core.Application\Kafka\MessageAttribute.cs) and is used by the [eventproducer](qwitter-core\Qwitter.Core.Application\Kafka\EventProducer.cs) to automatically produce the correct event from a class to the right topic.

Once you have a list of all the evens, you can exec into the running kafka container with
```
docker exec -it qwitter-service-kafka sh
```
In the kafka shell you can add all the topic with
```
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic user-created
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic user-state-changed
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic transaction-created
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic transaction-completed
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic transaction-overdraft
kafka-topics.sh --bootstrap-server localhost:9092 --create --topic invoice-overpayed
exit
```

Once you're created all the topic you need to create all the SQL database tables for each of the microservices, this can be done with entity framework. To install entity framework, run
```
dotnet tool install --global dotnet-ef
```
Once you've installed entity framework you need to apply migrations and update the databased
```
cd qwitter-user/Qwitter.User
dotnet ef migrations add InitialCommit
dotnet ef database update
```
This needs to be done for all microservices (user, ledger, payments and content).

Before running the backend you need to trust the developer certificates to be able to use HTTPS, this can be done with
```
dotnet dev-certs https --trust
```

You also need to generate x509 certificates used for assymetric JWT authentication. This requires openssl, installing openssl on windows is a bit difficult so I recommend using openssl in ubuntu with WSL. If you do not have WSL, openssl is installed along with git and can be found in ``C:\Program Files\Git\usr\bin\openssl.exe``. To generate the certificates you need to run
```
mkdir certificates
cd certificates
mkdir root_ca
openssl genrsa -out root_ca/root_key.pem 4096
openssl req -x509 -new -nodes -key root_ca/root_key.pem \
    -sha256 -days 1024 -out root_ca/root_cert.pem \
    -subj "/O=Qwitter/OU=Qwitter Root CA"
mkdir user_auth
openssl genrsa -out user_auth/user_auth_key.pem 4096
openssl req -new \
    -key user_auth/user_auth_key.pem \
    -out user_auth/csr.csr \
    -subj "/O=Qwitter/OU=Qwitter User Auth"
openssl x509 -req -in user_auth/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out user_auth/user_auth_cert.pem
rm user_auth/csr.csr
```
If you're using openssl from git in windows you need to replace openssl with the path to openssl.

Now you can finally launch the backend, one terminal window is required for each microservice but user and ledger is the most important
```
cd qwitter-user/Qwitter.User
dotnet run
```
In another terminal
```
cd qwitter-ledger/Qwitter.Ledger
dotnet run
```
Now in a third terminal you can launch the frontend, ``npm install`` can take a few minutes to run since all node_modules needs to be installed
```
cd qwitter-client
npm install
npm start
```
Now you can open the frontend on port 3000 (localhost:3000) in the browser.

Most of the features are not implemented in the frontend yet but can be tested through swagger by calling the API's directly on their respective ports. For example the user microservice is running on port 7001 and can be accessed by going to https://localhost:7001/swagger/index.html. The ledger microservice is running on port 7005 and can be accessed on https://localhost:7005/swagger/index.html.