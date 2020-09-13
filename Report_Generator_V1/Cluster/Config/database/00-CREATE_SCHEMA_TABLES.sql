create schema safra;

create table safra.modelo (
conta varchar(50),
entrada float,
saida float,
cluster varchar(100)
);

create table safra.transferencias (
conta varchar(50),
entrada float,
saida float,
);