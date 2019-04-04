-- Database: TrainerTracks
-- DROP DATABASE "TrainerTracks";
CREATE DATABASE "TrainerTracks"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- SCHEMA: tt
-- DROP SCHEMA tt ;
CREATE SCHEMA tt
    AUTHORIZATION postgres;