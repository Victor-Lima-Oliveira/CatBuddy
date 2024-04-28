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
   
   create table tbl_informacoesNutricionais (
cod_produto int, 
TamanhoOuPorcao varchar(10),
caloriaPorPorcao varchar(10),
proteinas varchar(10),
carboidratos varchar(10),
vitaminas varchar(10),
mineirais varchar(10),
fibraDiétrica varchar(10),
Colesterol varchar(10),
Sodio varchar(10));

CREATE TABLE tbl_produto (
    cod_id_produto int PRIMARY KEY auto_increment,
    cod_categoria int not null,
    descricao text not null,
    qtdEstoque int,
    cod_fornecedor int not null,
    idade varchar(13),
    sabor varchar(45),
    cor varchar(45),
    medidasAproximadas varchar(45),
    material varchar(45),
    composicao text,
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

create table tbl_tiposDePagamento(
cod_id_pagamento int primary key auto_increment,
nomePagamento varchar(30));

CREATE TABLE tbl_pedido (
    cod_id_pedido int PRIMARY KEY auto_increment,
    cod_usuario int,
    cod_pagamento int,
    valorTotal float,
    dataPedido datetime,
    foreign key (cod_usuario) references tbl_usuario (cod_id_usuario),
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
 t1.idade, 
 t1.sabor, 
 t1.cor,
t1.medidasaproximadas, 
t1.material,
 t1.composicao, 
 t1.preco, 
 t1.imgpath,
 t1.ds_nome,
 t2.cod_id_fornecedor "codFornecedor",
t2.nomeFantasia "fornecedor" ,
t3.cod_id_categoria "codCategoria",
t3.nomeCategoria "Categoria"
FROM tbl_produto t1 
INNER JOIN tbl_fornecedor t2 ON  t1.cod_fornecedor = t2.cod_id_fornecedor
INNER JOIN tbl_categoria t3 ON t1.cod_categoria = t3.cod_id_categoria;