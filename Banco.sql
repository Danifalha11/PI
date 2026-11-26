
CREATE DATABASE IF NOT EXISTS SistemaDoacoes;

USE SistemaDoacoes;


CREATE TABLE IF NOT EXISTS Doador (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20)
);


CREATE TABLE IF NOT EXISTS Necessitado (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20),
    NivelUrgencia INT NOT NULL
);


CREATE TABLE IF NOT EXISTS InstituicaoDoacao (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    CNPJ VARCHAR(20),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Endereco VARCHAR(200),
    Telefone VARCHAR(20)
);


CREATE TABLE IF NOT EXISTS Item (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Quantidade INT NOT NULL,
    Tipo INT NOT NULL,
    Disponivel BOOLEAN DEFAULT TRUE,
    DoadorId INT NULL,
    NecessitadoId INT NULL,

    FOREIGN KEY (DoadorId) REFERENCES Doador(Id),
    FOREIGN KEY (NecessitadoId) REFERENCES Necessitado(Id),

    CHECK (Quantidade >= 0),

    CHECK (
        (DoadorId IS NOT NULL AND NecessitadoId IS NULL)
        OR
        (DoadorId IS NULL AND NecessitadoId IS NOT NULL)
    )
);
