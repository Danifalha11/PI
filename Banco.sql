
CREATE DATABASE IF NOT EXISTSSistemaDoacoes;

USE SistemaDoacoes;


CREATE TABLE IF NOT EXISTS Doador (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20)
);


CREATE TABLE IF NOT EXISTS Necessitado (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20),
    NivelUrgencia INT
);


CREATE TABLE IF NOT EXISTS InstituicaoDoacao (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100),
    CNPJ VARCHAR(20),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Endereco VARCHAR(200),
    Telefone VARCHAR(20)
);


CREATE TABLE IF NOT EXISTS Item (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(100),
    Quantidade INT,
    Tipo INT,
    Disponivel BOOLEAN DEFAULT TRUE,
    DoadorId INT NULL,
    NecessitadoId INT NULL,
    FOREIGN KEY (DoadorId) REFERENCES Doador(Id),
    FOREIGN KEY (NecessitadoId) REFERENCES Necessitado(Id)
);
    