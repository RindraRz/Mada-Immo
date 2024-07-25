--(maison, appartement, villa, immeuble

insert into type_bien (nom,commission) values ('maison',5);
insert into type_bien (nom,commission) values ('appartement',10);
insert into type_bien (nom,commission) values ('villa',20);

insert into proprietaire (contact) values ('0341517076');
insert into proprietaire (contact) values ('0343780217');
insert into proprietaire (contact) values ('0322225364');

insert into client (email) values ('a@gmail.com');
insert into client (email) values ('b@gmail.com');

insert into bien (nom,description,region,loyer,type_bien_id,proprietaire_id) 
          values ('A','3 pieces','analamanga',2000000,1,1);
insert into bien (nom,description,region,loyer,type_bien_id,proprietaire_id) 
          values ('B','5 pieces et piscine','analamanga',1500000000,3,2);


insert into location(client_id,duree,date_debut,bien_id)
        values (1,10,'2024-01-20',1);
insert into detail_location(location_id,loyer,commission)
        values (3,2000000,5);        

insert into location(client_id,duree,date_debut,bien_id)
        values (2,5,'2024-05-14',2);
insert into detail_location(location_id,loyer,commission)
        values (4,1500000000,20);         

insert into location(client_id,duree,date_debut,bien_id)
        values (2,1,'2024-01-31',1);
