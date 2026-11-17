
CREATE DATABASE SistemaDoacoes;

USE SistemaDoacoes;


CREATE TABLE Doador (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20)
);


CREATE TABLE Necessitado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Nacionalidade VARCHAR(50),
    Telefone VARCHAR(20),
    NivelUrgencia INT
);


CREATE TABLE InstituicaoDoacao (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100),
    CNPJ VARCHAR(20),
    Email VARCHAR(100),
    Cidade VARCHAR(100),
    Bairro VARCHAR(100),
    Endereco VARCHAR(200),
    Telefone VARCHAR(20)
);


CREATE TABLE Item (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100),
    Quantidade INT,
    Tipo INT,
    Disponivel BIT
);
