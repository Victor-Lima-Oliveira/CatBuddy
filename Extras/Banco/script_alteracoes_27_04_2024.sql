use db_CatBuddy;

-- Retirada da razão social do fornecedor 
alter table tbl_fornecedor drop column razaosocial;

-- Criacao da tabela pedido 
drop table tbl_itempedido;
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
 t1.informacoesnutricionais, 
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

-- Criacao da tabela de pagamentos
drop table tbl_itempedido;
drop table tbl_pedido;

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
 
-- Adicionado os tipos de pagamento aceitos 
insert into tbl_tiposdepagamento values
(null, "AINDA NÃO SELECIONADO"),(null, "PIX"), (null,"BOLETO");

-- Criação de um usuario basico
insert into tbl_usuario values(
null, 1, "Cliente 1", "46871235422");





