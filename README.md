# choreography
choreography saga demo


##Kafka e Zookeeper (docker-compose)
###Subir os containers
docker-compose up -d
###Logs do zookeeper
docker-compose logs zookeeper | grep -i binding
###Analisar saude do kafka
docker-compose logs kafka | grep -i started
###Analisar logs do kafka
docker-compose logs --f
###Listar topicos
winpty docker-compose exec kafka kafka-topics --list --zookeeper zookeeper:2181
###Criar topico
winpty docker-compose exec kafka kafka-topics --create --topic orderrequests --partitions 1 --replication-factor 1 --if-not-exists --zookeeper zookeeper:2181
###Se quiser confirmar se o Topic foi criado, execute o comando abaixo:
winpty docker-compose exec kafka kafka-topics --describe --topic orderrequests --zookeeper zookeeper:2181
###Teste produzindo mensagem
winpty docker-compose exec kafka bash -c "seq 100 | kafka-console-producer --request-required-acks 1 --broker-list localhost:29092 --topic meu-topico-legal && echo 'Produced 100 messages.'"
###Consumindo 
winpty docker-compose exec kafka kafka-console-consumer --bootstrap-server localhost:29092 --topic orderrequests --from-beginning --max-messages 100

