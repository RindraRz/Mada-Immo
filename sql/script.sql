create database mada_immo2;

\c mada_immo2;

create table admin(
    admin_id serial primary key,
    email varchar(100),
    mdp varchar(100)
);

insert into admin(email,mdp) values('admin@gmail.com','1234');

create table proprietaire(
    proprietaire_id serial primary key,
    contact varchar(20)
);


create table client(
    client_id serial primary key,
    email varchar(100)
);

create table type_bien(
    type_bien_id serial primary key ,
    nom varchar(100),
    commission double precision
);

create table bien (
    bien_id serial primary key,
    reference varchar(100) unique ,
    nom varchar(100),
    description varchar(300),
    region varchar(100),
    loyer double precision,
    type_bien_id int references type_bien(type_bien_id),
    proprietaire_id int references proprietaire(proprietaire_id) 
);

create table bien_photo (
    photo_id serial primary key,
    bien_id int references bien(bien_id),
    path varchar(100)
);


create table location(
    location_id serial primary key,
    client_id int references client(client_id),
    duree int ,
    date_debut date,
    date_fin date ,
    bien_id int references bien(bien_id)
);


create table detail_location(
    detail_location_id serial primary key ,
    location_id int references location(location_id),
    loyer double precision ,
    mois date,
    commission double precision,
    num_mois_location  int
);

create table bien_csv (
    bien_csv_id serial primary key,
    reference varchar(100),
    nom varchar(100),
    description varchar(800),
    type varchar(100),
    region varchar(100),
    loyer double precision ,
    proprietaire varchar(100)
);

create table location_csv (
    location_csv_id serial primary key,
    reference varchar(100),
    date_debut date ,
    duree int ,
    client varchar(100)
);

create table commission_csv (
    commission_csv_id serial primary key,
    type varchar(100),
    commission double precision
);

create table interval (
    id serial primary key,
    min double precision,
    max double precision
);
insert into interval (min,max) values (1000000,2000000);

update interval set min =0 , max=3000000; 



