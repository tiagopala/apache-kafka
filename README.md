# Apache-Kafka

Repositório criado para praticar conceitos referentes ao Apache Kafka.

## Apache Kafka (localhost)

Para subir o kafka em ambiente local, executar o seguinte comando: ```docker-compose up -d```. Ele irá subir o container do zooekeeper e do kafka com a configuração default (padrão), à partir dele já podemos criar nossos tópicos, partições, offsets e assim por diante.

## Manipulando features

Foi adicionado os binários da versão 2.13-3.3.1 do Apache Kafka para facilitar a manipulação das features cluster.

A partir do diretório ```kafka``` é possível utilizar os comandos abaixo.

> Os comandos abaixo são utilizando o sistema operacional Windows. Para linux e mac substituir o file path de ```.\bin\windows\``` por ```.\bin\``` e a extensão do arquivo de ```.bat``` para ```.sh```.

### Tópicos

#### Criação novo tópico

```batch
.\bin\windows\kafka-topics.bat --create --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

##### Com partição

```batch
.\bin\windows\kafka-topics.bat --create --topic TopicName --partitions 2 --replication-factor 2 --bootstrap-server localhost:9094,localhost:9095
```

#### Deletar tópico

```batch
.\bin\windows\kafka-topics.bat --delete --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

#### Listagem de tópicos

```batch
.\bin\windows\kafka-topics.bat --list --bootstrap-server localhost:9094,localhost:9095
```