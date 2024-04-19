/* Modelo Físico: */
create database db_catBuddy;
use db_catBuddy;

CREATE TABLE tbl_logradouro (
    cod_id_logradouro int PRIMARY KEY auto_increment,
    nomeLogradouro varchar(10) UNIQUE);
    
CREATE TABLE tbl_categoria (
    cod_id_categoria int PRIMARY KEY auto_increment,
    nomeCategoria varchar(35) UNIQUE);

CREATE TABLE tbl_fornecedor (
    cod_id_fornecedor int PRIMARY KEY auto_increment,
    cnpj varchar(14) UNIQUE,
    razaoSocial varchar(80),
    nomeFantasia varchar(35));
    
CREATE TABLE tbl_contatoFornecedor (
	cod_id_contato int PRIMARY KEY auto_increment,
	numero varchar(11),
	nomeContato varchar(20),
	cod_fornecedor int,
	foreign key (cod_fornecedor) references tbl_fornecedor (cod_id_fornecedor));

CREATE TABLE tbl_enderecoFornecedor (
    cod_id_endereco int PRIMARY KEY auto_increment,
    endereco varchar(35),
    bairro varchar(35),
    cep varchar(8),
    cod_logradouro int,
    nomeEndereco varchar(35),
    cod_fornecedor int,
   foreign key (cod_fornecedor) references tbl_fornecedor (cod_id_fornecedor),
   foreign key (cod_logradouro) references tbl_logradouro (cod_id_logradouro));

CREATE TABLE tbl_produto (
    cod_id_produto int PRIMARY KEY auto_increment,
    cod_categoria int not null,
    descricao text not null,
    qtdEstoque int,
    cod_fornecedor int not null,
    idade varchar(13),
    sabor varchar(45),
    informacoesNutricionais text,
    cor varchar(45),
    medidasAproximadas varchar(45),
    material varchar(45),
    composição text,
    preco float not null,
    imgPath varchar(200),
    ds_nome varchar(40),
    foreign key (cod_categoria) references tbl_categoria (cod_id_categoria),
    foreign key (cod_fornecedor) references tbl_fornecedor (cod_id_fornecedor));

CREATE TABLE tbl_Usuario (
    cod_id_usuario int PRIMARY KEY auto_increment,
    NivelAcesso int,
    nomeUsuario varchar(50),
    CPF varchar(11));
    
 
 -- TODO: criar tabela pagamento
CREATE TABLE tbl_pedido (
    cod_id_pedido int PRIMARY KEY auto_increment,
    cod_usuario int,
    cod_pagamento int,
    valorTotal int,
    dataPedido datetime,
    foreign key (cod_usuario) references tbl_usuario (cod_id_usuario) );
    
-- TODO: Criar a tabela vacina 
CREATE TABLE tbl_carteiraVacina (
    cod_id_carteirinha int PRIMARY KEY auto_increment,
    cod_vacina int,
    nomeGato varchar(30),
    pesoGato varchar(5),
    IsFemeaGato bool,
    PelagemGato varchar(20),
    dataNascimentoGato datetime,
    Raca varchar(20));

CREATE TABLE tbl_enderecoUsuario (
    cod_id_endereco int PRIMARY KEY auto_increment,
    enderecoUsuario varchar(35),
    bairroUsuario varchar(35),
    cepUsuario varchar(8),
    cod_logradouro int,
    nomeEnderecoUsuario varchar(35),
    cod_usuario int,
    foreign key (cod_logradouro) references tbl_logradouro (cod_id_logradouro),
    foreign key (cod_usuario) references tbl_usuario (cod_id_usuario));

CREATE TABLE tbl_itemPedido (
    cod_id_item int PRIMARY KEY auto_increment,
    cod_prod int,
    cod_usuario int,
    quantidadeProduto int,
    somatorioItem int,
    cod_pedido int,
    foreign key (cod_prod) references tbl_produto (cod_id_produto),
    foreign key (cod_usuario) references tbl_usuario (cod_id_usuario),
    foreign key (cod_pedido) references tbl_pedido (cod_id_pedido));
 