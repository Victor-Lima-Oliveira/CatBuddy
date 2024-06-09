 use db_catBuddy;

 -- Insert fixos
 insert into tbl_logradouro values
(default, 'Rua'), -- 1
(default, 'Avenida'), -- 2
(default, 'Praça'), -- 3 
(default, 'Travessa'), -- 4
(default, 'Alameda'), -- 5 
(default, 'Beco'), -- 6
(default, 'Estrada'), -- 7
(default, 'Viela'), -- 8
(default, 'Largo'), -- 9 
(default, 'Rodovia'); -- 10

insert into tbl_categoria values
(default, 'Ração Seca'), -- 1
(default, 'Ração Úmida'), -- 2
(default, 'Petisco'), -- 3 
(default, 'Comedouros e fontes'), -- 4
(default, 'Coleiras e Peitorais'), -- 5
(default, 'Esconderijos'), -- 6
(default, 'Transporte'), -- 7
(default, 'Higiene'), -- 8 
(default, 'Roupas gatitos'), -- 9
(default, 'Mimos Humanos'); -- 10

 insert into tbl_tiposdepagamento values -- pagamento
 (default, 'Pix'),
 (default, 'Cartão de Crédito'),
 (default, 'Boleto');
 
 insert into tbl_genero values -- Genero 
 (null, "Feminino"), 
 (null, "Masculino"), 
 (null, "Gênero neutro"), 
 (null, "Não binário"), 
 (null, "Prefiro não informar");
 
 insert into tbl_nivelDeAcesso values 
 (null, "Vendedor"), 
 (null, "Desenvolvedor"),
 (null, "Gerente"),
 (null, "RH"),
 (null, "Master");
 