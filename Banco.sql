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
    NivelUrgencia INT NOT NULL DEFAULT 0
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
    Quantidade INT NOT NULL DEFAULT 0,
    Tipo INT NOT NULL,
    DoadorId INT NULL,
    NecessitadoId INT NULL,

    CONSTRAINT fk_item_doador
        FOREIGN KEY (DoadorId)
        REFERENCES Doador(Id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT fk_item_necessitado
        FOREIGN KEY (NecessitadoId)
        REFERENCES Necessitado(Id)
        ON UPDATE CASCADE
        ON DELETE RESTRICT,

    CONSTRAINT chk_item_quantidade
        CHECK (Quantidade >= 0)
);
