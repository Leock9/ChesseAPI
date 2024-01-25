// mongo-init.js

// Nome do banco de dados
const dbName = "fiap-tech-challenge";

// Nome de usu�rio e senha para autentica��o
const userName = "root";
const password = "root@123";

// Adiciona o usu�rio administrador
db.createUser({
    user: userName,
    pwd: password,
    roles: [
        {
            role: "readWrite",
            db: dbName
        }
    ]
});

Console.Log("Usu�rio criado com sucesso!");

// Usa o banco de dados especificado
db = db.getSiblingDB(dbName);

// Cria cole��es ou executa outras configura��es iniciais, se necess�rio
db.createCollection("ClientDocument");

Console.Log("Cole��o ClientDocument criada com sucesso!");

