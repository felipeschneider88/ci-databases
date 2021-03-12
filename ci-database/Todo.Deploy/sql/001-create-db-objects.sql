--Create user for the access
--DO
--$do$
--BEGIN
--   IF NOT EXISTS (
--      SELECT FROM pg_catalog.pg_roles  -- SELECT list can be empty for this
--      WHERE  rolname = 'autoCI') THEN
	  
--CREATE ROLE  autoCI  LOGIN PASSWORD 'Postgr@s321!';
--   END IF;
--END
--$do$;
CREATE ROLE  autoCI  LOGIN PASSWORD 'Postgr@s321!';

--Create the schema 
create schema IF NOT EXISTS web authorization autoCI;



CREATE SEQUENCE serial START 1;

CREATE TABLE IF NOT EXISTS todos (
	id int not null primary key default nextval('serial'),
	title varchar(100) not null,
	completed boolean not null default (false)
);

-- Create some default values 
insert into todos (title) values ('task 1');
insert into todos (title) values ('write blog post');
insert into todos (title) values ('get a hair cut');
insert into todos (title) values ('clen my desktop');
insert into todos (title, completed) values ('task 2', true);

