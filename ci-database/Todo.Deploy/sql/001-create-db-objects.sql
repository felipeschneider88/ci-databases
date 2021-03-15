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
CREATE ROLE  autoci  LOGIN PASSWORD '$webAPIUSer$';

--Create the schema 
create schema IF NOT EXISTS web authorization autoci;
GRANT CONNECT ON DATABASE todo  TO autoci;


GRANT SELECT ON ALL TABLES IN SCHEMA web TO autoci;



CREATE SEQUENCE serial START 1;

CREATE TABLE IF NOT EXISTS web.todos (
	id int not null primary key default nextval('serial'),
	title varchar(100) not null,
	completed boolean not null default (false)
);

-- Create some default values 
insert into web.todos (title) values ('task 1');
insert into web.todos (title) values ('write blog post');
insert into web.todos (title) values ('get a hair cut');
insert into web.todos (title) values ('clen my desktop');
insert into web.todos (title, completed) values ('task 2', true);

