# Apache-Kafka

Repositório criado para praticar conceitos referentes ao Apache Kafka.

## Workflow

![apache-kafka-workflow-preview](/images/apache-kafka-workflow.drawio.png)

## Tracing using Jaeger

![workflow-tracing-preview](/images/workflow-tracing.jpg)

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

#### Deletar

```batch
.\bin\windows\kafka-topics.bat --delete --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

#### Listar

```batch
.\bin\windows\kafka-topics.bat --list --bootstrap-server localhost:9094,localhost:9095
```

#### Descrever

##### Todos

```batch
.\bin\windows\kafka-topics.bat --describe --bootstrap-server localhost:9094,localhost:9095
```

##### Único tópico

```batch
.\bin\windows\kafka-topics.bat --describe --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

#### Alterar número de partições

```batch
.\bin\windows\kafka-topics.bat --alter --topic TopicName --partitions 3 --bootstrap-server localhost:9094,localhost:9095
```

### Eventos

#### Enviar

```batch
.\bin\windows\kafka-console-producer.bat --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

#### Receber

##### Novas

```batch
.\bin\windows\kafka-console-consumer.bat --topic TopicName --bootstrap-server localhost:9094,localhost:9095
```

##### Retroativo

```batch
.\bin\windows\kafka-console-consumer.bat --topic TopicName --from-beginning --bootstrap-server localhost:9094,localhost:9095
```

Argumentos opcionais:
  - ```--property print.partition=true```
  - ```--property print.offset=true```
  - ```--group GroupName```

## Kafka Tool

Para facilitar o gerenciamento do nosso cluster, podemos utilizar o kafka tool para visualização do nosso cluster, como pode ser visto na imagem abaixo.

![kafka-tool-preview](/images/kafka-tool-preview.png)