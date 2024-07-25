
-- location_id | client_id | duree | date_debut | bien_id |      ?column?
create or replace view v_location_date_fin as
select 
l.location_id,
    l.duree ,
    l.date_debut,
    l.bien_id,
    client_id,
   
    ((date_trunc('month', l.date_debut::date) + make_interval(months => l.duree))::date - interval '1 day')::date AS date_fin,
    b.proprietaire_id
from   location l 
join bien b 
on l.bien_id=b.bien_id;

create or replace view v_location as
select 
dl.detail_location_id,
    dl.location_id ,
    l.client_id,
    l.duree ,
    l.date_debut,
    l.bien_id,
    ((date_trunc('month', l.date_debut::date) + make_interval(months => l.duree))::date - interval '1 day')::date AS date_fin,
    dl.loyer,
    dl.commission,
    b.proprietaire_id,
    dl.mois,
    dl.num_mois_location
from 
detail_location as dl 
join location l 
on dl.location_id = l.location_id
join bien b 
on l.bien_id=b.bien_id;

create or replace view v_location_bien as
select 
location_id,
client_id,
duree,
date_debut,
b.bien_id,
b.reference,
b.loyer,
t.commission,
p.proprietaire_id,
p.contact,
 ((date_trunc('month', l.date_debut::date) + make_interval(months => l.duree))::date - interval '1 day')::date AS date_fin
from location l 
join bien b on b.bien_id=l.bien_id
join proprietaire p on p.proprietaire_id=b.proprietaire_id
join type_bien t on t.type_bien_id=b.type_bien_id;

create or replace view v_date_fin as 
select 

    ((date_trunc('month', l.date_debut::date) + make_interval(months => l.duree))::date - interval '1 day')::date AS date_fin
from 
detail_location as dl 
join location l 
on dl.location_id = l.location_id
join bien b 
on l.bien_id=b.bien_id;


SELECT *
FROM location
WHERE EXTRACT(MONTH FROM date_debut) < 3 AND EXTRACT(YEAR FROM date_debut) = 2024;

SELECT *
FROM v_location 
WHERE EXTRACT(MONTH FROM date_debut) <= 12 AND EXTRACT(YEAR FROM date_debut) <= 2024
AND EXTRACT(MONTH FROM date_fin) >= 12 AND EXTRACT(YEAR FROM date_fin) >= 2024 ;

SELECT 
    (date_trunc('month', '2024-01-10'::date) + interval '3 month')::date - interval '1 day' AS datefin;


select 
    reference,
    b.nom,
    b.description,
    b.region,
    b.loyer,
    tb.type_bien_id,
    p.proprietaire_id
from bien_csv b
join type_bien tb on tb.nom=b.type 
join proprietaire p on p.contact=b.proprietaire;


select 
c.client_id,
l.duree,
l.date_debut,
b.bien_id
from location_csv l 
join client c on c.email = l.client 
join bien b on b.reference = l.reference ;



select 
location_id ,
loyer ,
commission
from location_csv l_csv 
join v_location_bien l on l.date_debut=l_csv.date_debut and l.reference=l_csv.reference;