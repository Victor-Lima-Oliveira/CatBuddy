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
    cnpj varchar(14),
    nomeFantasia varchar(35),
    telefone varchar(11),
    endereco varchar(100),
    bairro varchar(35), 
    cep varchar(8),
    cod_logradouro int,
    municipio varchar(30),
    IsFornecedorAtivo bool,
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
    IsProdutoAtivo bool,
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
cod_cliente int not null,
qtd int not null,
subtotal float not null,
foreign key (cod_produto) references tbl_produto (cod_id_produto),
foreign key (cod_pedido) references tbl_pedido (cod_id_pedido),
foreign key (cod_cliente) references tbl_cliente (cod_id_cliente));

create table tbl_cartaoCliente(
cod_id_pagamento int primary key auto_increment,
cod_cliente int not null, 
nomeTitular varchar(50),
numeroCartaoCred varchar(16),
dataDeValidade varchar(4),
codSeguranca varchar(3),
foreign key (cod_cliente) references tbl_cliente (cod_id_cliente));

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
 t1.IsProdutoAtivo,
 t2.cod_id_fornecedor "codFornecedor",
t2.nomeFantasia "fornecedor" ,
t3.cod_id_categoria "codCategoria",
t3.nomeCategoria "Categoria"
FROM tbl_produto t1 
INNER JOIN tbl_fornecedor t2 ON  t1.cod_fornecedor = t2.cod_id_fornecedor
INNER JOIN tbl_categoria t3 ON t1.cod_categoria = t3.cod_id_categoria;

-- Criada uma view do pedido para facilitar as consultas
create view vwPedido as
select 
pedido.cod_id_pedido,
pedido.cod_cliente,
cliente.nomeUsuario,
pedido.cod_pagamento,
pagto.nomepagamento,
pedido.valortotal,
pedido.datapedido,
(select count(cod_produto) from tbl_itempedido 
where cod_pedido = pedido.cod_id_pedido AND cod_cliente = cliente.cod_id_cliente) as qtdProdutos
from tbl_pedido pedido
inner join tbl_cliente cliente on cliente.cod_id_cliente = pedido.cod_cliente
inner join tbl_tiposdepagamento pagto on pagto.cod_id_pagamento = pedido.cod_pagamento;

-- Criada uma view dos itens do pedido para facilitar consultas
create  view vwItensPedido As 
select
item.cod_produto,
produto.ds_nome,
item.cod_pedido,
pedido.valortotal,
pedido.datapedido,
item.qtd,
item.subtotal,
produto.imgPath,
produto.isprodutoativo
from tbl_itempedido item
inner join tbl_produto produto on produto.cod_id_produto = item.cod_produto
inner join tbl_pedido pedido on pedido.cod_id_pedido = item.cod_pedido;

-- Criada uma view dos fornecedor com o nome do logradouro
create view vwFornecedor as 
select * from 
tbl_fornecedor fornecedor
inner join tbl_logradouro logradouro on logradouro.cod_id_logradouro = fornecedor.cod_id_fornecedor;

-- Criada uma view do endereco com o nome do logradouro
create view vwEndereco as
select * from 
tbl_enderecocliente endereco
inner join tbl_logradouro logradouro on logradouro.cod_id_logradouro = endereco.cod_id_endereco;
