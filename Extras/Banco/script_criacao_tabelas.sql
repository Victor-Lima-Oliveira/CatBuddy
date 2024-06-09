/* Modelo Físico: */
drop database db_catbuddy;
create database db_catBuddy;
use db_catBuddy;

CREATE TABLE tbl_logradouro (
    cod_id_logradouro int PRIMARY KEY auto_increment,
    nomeLogradouro varchar(10) UNIQUE);
    
CREATE TABLE tbl_categoria (
    cod_id_categoria int PRIMARY KEY auto_increment,
    nomeCategoria varchar(35) UNIQUE);
    
CREATE TABLE tbl_genero (
    cod_id_genero int primary key auto_increment,
    ds_genero varchar(20) not null unique);

CREATE TABLE tbl_fornecedor (
    cod_id_fornecedor int PRIMARY KEY auto_increment,
    cnpj varchar(14) UNIQUE,
    nomeFantasia varchar(35),
    telefone varchar(11),
    nomeContato varchar(50));

CREATE TABLE tbl_enderecoFornecedor (
    cod_id_endereco int PRIMARY KEY auto_increment,
    endereco varchar(100),
    bairro varchar(35),
    cep varchar(8),
    cod_logradouro int,
    nomeEndereco varchar(35),
    cod_fornecedor int,
    municipio varchar (30),
   foreign key (cod_fornecedor) references tbl_fornecedor (cod_id_fornecedor),
   foreign key (cod_logradouro) references tbl_logradouro (cod_id_logradouro));
   
CREATE TABLE tbl_produto (
    cod_id_produto int PRIMARY KEY auto_increment,
    cod_categoria int not null,
    descricao text not null,
    qtdEstoque int,
    cod_fornecedor int not null,
    idadeRecomendada varchar(30),
    sabor varchar(45),
    cor varchar(45),
    medidasAproximadas varchar(45),
    material varchar(45),
    composicao text,
    preco float not null,
    imgPath varchar(200),
    imgPathinfoNutricionais varchar(200),
    ds_nome varchar(100),
    foreign key (cod_categoria) references tbl_categoria (cod_id_categoria),
    foreign key (cod_fornecedor) references tbl_fornecedor (cod_id_fornecedor));

CREATE TABLE tbl_cliente (
    cod_id_cliente int PRIMARY KEY auto_increment,
	nomeUsuario varchar(50) not null,
    dtNascimento datetime not null,
    CPF varchar(11) not null unique,
    email varchar(100) not null unique, 
    telefone varchar(11),
    senha varchar(260) not null,
    cod_genero int not null ,
    IsClienteAtivo bool not null, 
    foreign key (cod_genero) references tbl_genero (cod_id_genero));
    
CREATE TABLE tbl_enderecoCliente (
    cod_id_endereco int PRIMARY KEY auto_increment,
    enderecoUsuario varchar(35),
    bairroUsuario varchar(35),
    cepUsuario varchar(8),
    cod_logradouro int,
    nomeEnderecoUsuario varchar(35),
    cod_cliente int,
    foreign key (cod_logradouro) references tbl_logradouro (cod_id_logradouro),
    foreign key (cod_cliente) references tbl_cliente (cod_id_cliente));
    
    create table tbl_nivelDeAcesso (
cod_id_nivelAcesso int primary key auto_increment, 
ds_nivelAcesso varchar (30) not null unique
);

Create table tbl_colaborador (
    cod_id_colaborador int primary key auto_increment, 
    nomeColaborador varchar(50) not null, 
    email varchar(100) not null unique,
    CPF varchar(11) not null unique,
    telefone varchar(11),
    senha varchar(260) not null,
    cod_nivelDeAcesso int not null,
    cod_genero int not null,
    IsColaboradorAtivo bool not null, 
    foreign key (cod_genero) references tbl_genero (cod_id_genero),
    foreign key (cod_nivelDeAcesso) references tbl_nivelDeAcesso (cod_id_nivelAcesso));

create table tbl_tiposDePagamento(
cod_id_pagamento int primary key auto_increment,
nomePagamento varchar(30));

CREATE TABLE tbl_pedido (
    cod_id_pedido int PRIMARY KEY auto_increment,
    cod_cliente int,
    cod_pagamento int,
    valorTotal float,
    dataPedido datetime,
    foreign key (cod_cliente) references tbl_cliente (cod_id_cliente),
    foreign key (cod_pagamento) references tbl_tiposDePagamento (cod_id_pagamento));
    
    create table tbl_itemPedido(
cod_produto int not null,
cod_pedido int not null,
qtd int not null,
subtotal float not null,
foreign key (cod_produto) references tbl_produto (cod_id_produto),
foreign key (cod_pedido) references tbl_pedido (cod_id_pedido));

-- Criada uma view do produto para facilitar as consultas
create view vwProduto as
SELECT 
t1.cod_id_produto, 
t1.descricao,
 t1.qtdestoque,
 t1.idadeRecomendada, 
 t1.sabor, 
 t1.cor,
t1.medidasaproximadas, 
t1.material,
 t1.composicao, 
 t1.preco, 
 t1.imgpath,
 t1.imgPathinfoNutricionais,
 t1.ds_nome,
 t2.cod_id_fornecedor "codFornecedor",
t2.nomeFantasia "fornecedor" ,
t3.cod_id_categoria "codCategoria",
t3.nomeCategoria "Categoria"
FROM tbl_produto t1 
INNER JOIN tbl_fornecedor t2 ON  t1.cod_fornecedor = t2.cod_id_fornecedor
INNER JOIN tbl_categoria t3 ON t1.cod_categoria = t3.cod_id_categoria;

