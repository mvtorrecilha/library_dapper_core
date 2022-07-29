# Library .Net Core

Esse projeto tem como objetivo controlar o empréstimo de livros de uma biblioteca fictícia.

Algumas funcionalidades já estão presentes, são elas:
* Login
* Listagem de livros
* Pegar emprestado


## Ferramentas
Para rodar esse projeto você vai precisar:

1. .Net core sdk 3.1 (https://dotnet.microsoft.com/download)

1. MSSql LocalDb - (https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi)

1. NodeJs (https://nodejs.org/en/download/)

Com todas essas ferramentas instaladas, basta executar os seguintes comandos no terminal do seu sistema operacional:

### Backend:

```
dotnet run --project .\backend\src\Library.Api\Library.Api.csproj -- seed
```
Esse comando irá executar o site na url "https://localhost:5001" e o banco de dados já será criado com alguns dados já inseridos. Você pode verificar os dados inseridos no arquivo **DatabaseBoostrp.cs**.

**Obs.: Toda vez que vc executar esse comando os dados das tabelas serão apagados. Para não apagar os dados é só tirar o argumento "-- seed"**

### Frontend:

```
cd frontend
npm install
npm run serve
```

Esse comando irá executar o frontend na url "http://localhost:8080"



