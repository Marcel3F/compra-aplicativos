# compra-aplicativos 
Solução para uma aplicação de compra de aplicativos

Nesse projeto foram utilizadas as seguintes tecnologias:
* .Net Core 3.1
* MongoDB
* RabbitMQ
* Memory Cache

## Design arquitetural

Desenho de solução:
<p align="center">
    <img alt="Api Compra Aplicativos" src="https://raw.githubusercontent.com/Marcel3F/compra-aplicativos/main/imagem-readme/solucao-compra-aplicativos.png?token=AC6YYEHSOGSDGRZSQNWTZ2DA46GKC" />
</p>

Desenho das aplicações

*Api:*
Foi implementada uma abordagem do padrão arquitetural conhecido por "Clean Architecture".
Abaixo está o desenho do fluxo de execução:
<p align="center">
    <img alt="Clean Architecture" src="https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg" />
</p>

*Worker:*
Foi utilizada uma abordagem simplista, com o foco na velocidade de desenvolvimento e com o objetivo de processar as mensagens da fila.

## Como rodar a solução
Para rodar a solução primeiramente é necessário instalar o .Net Core 3.1 e o Docker.

Após ter realizado as instalações, é necessário executar em ordem os seguintes comandos:
```
docker-compose -f './api/docker-compose.yml' up --b
dotnet run --project ./api/src/CompraAplicativos.Api/CompraAplicativos.Api.csproj
dotnet run --project ./servico-consumer/src/CompraAplicativos.Consumer/CompraAplicativos.Consumer.csproj
```

