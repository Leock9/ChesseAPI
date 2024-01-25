// mongo-init.js

// Nome do banco de dados
const dbName = "fiap-tech-challenge";

// Nome de usuário e senha para autenticação
const userName = "root";
const password = "root@123";

// Adiciona o usuário administrador
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

Console.Log("Usuário criado com sucesso!");

// Usa o banco de dados especificado
db = db.getSiblingDB(dbName);

// Cria coleções ou executa outras configurações iniciais, se necessário
db.createCollection("ClientDocument");

Console.Log("Coleção ClientDocument criada com sucesso!");

