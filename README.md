# Choreography
choreography saga demo

### Sobre
Projeto para simular coreografia entre serviços utilizando Apache Kafka, MongoDB e .netCore. Demo apresentada em meetup, com fins didáticos, para permitir conhecer o funcionamento do Kafka.  
Serão executados em docker: Apache Kafka, Zookeeper, Mongo e Mongo-Express (web-based admin interface)

Nuget Package  
Confluent.Kafka -> *Confluent's .Net Client for Apache Kafka*

### Como executar?
Para executar é necessário ter instalado:
* .net core
* docker 

1. Baixar o projeto (clone)
2. Utilizar "docker-compose up -d" na pasta raiz, para baixar as imagens e executar (kafka, zookeeper, mongo)
3. Executar dotnet run em cada pasta (order.service, flight.service, payment.service, hotel.service, order.webapi, etc)
4. Abrir no browser a webapi (order) e realizar um post (post com valores acima de 100, terão pagamento negados, assim realizando compensação\desfazimento)

Após realizar o "docker-compose up -d" na raiz da pasta src, e executar os projetos ("dotnet run" em cada pasta de projeto):
* Para abrir o Mongo-Express, digitar "http://localhost:8081/"
* Para abrir a WebAPI (apos executar o order.webapi), digitar "http://localhost:5000/swagger"

## Comandos Kafka e Zookeeper (docker-compose)
Abaixo alguns comandos que podem ser utilizados nesta Demo para aprender mais sobre o Kafka, docker-compose e .net Core.
## Docker Compose
### Subir os containers
    docker-compose up -d
### Parar e remover os containers
    docker-compose down
## Apache Kafka e Zookeeper
### Logs do zookeeper
    docker-compose logs zookeeper | grep -i binding
### Analisar saude do kafka
    docker-compose logs kafka | grep -i started
### Analisar logs do kafka
    docker-compose logs --f
### Listar topicos (OBS: utilize winpty caso utilize windows com terminal bash)
    winpty docker-compose exec kafka kafka-topics --list --zookeeper zookeeper:2181
### Criar topico (OBS: utilize winpty caso utilize windows com terminal bash)
    winpty docker-compose exec kafka kafka-topics --create --topic orderrequests --partitions 1 --replication-factor 1 --if-not-exists --zookeeper zookeeper:2181
### Se quiser confirmar se o Topic foi criado  (OBS: utilize winpty caso utilize windows com terminal bash)
    winpty docker-compose exec kafka kafka-topics --describe --topic orderrequests --zookeeper zookeeper:2181
### Teste produzindo mensagem  (OBS: utilize winpty caso utilize windows com terminal bash)
    winpty docker-compose exec kafka bash -c "seq 100 | kafka-console-producer --request-required-acks 1 --broker-list localhost:29092 --topic meu-topico-legal && echo 'Produced 100 messages.'"
### Consumindo mensagens (OBS: utilize winpty caso utilize windows com terminal bash)
    winpty docker-compose exec kafka kafka-console-consumer --bootstrap-server localhost:29092 --topic orderrequests --from-beginning --max-messages 100
## .Net Core
### Criar um novo aplicativo\projeto
    dotnet new ... (escreva o tipo de projeto, ex console, webapi, webapp, mvc, etc)
### Compilar um aplicativo\projeto
    dotnet build
### Execucao projeto .netCore (acessar a pasta do projeto)
    dotnet run
### Limpar saídas build
    dotnet clean
### Executar os testes
    dotnet test
   

