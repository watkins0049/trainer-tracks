-- Role: tt_app_role
-- DROP ROLE tt_app_role;

CREATE ROLE tt_app_role WITH
  NOLOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;

-- Schema grants  
GRANT USAGE ON SCHEMA tt TO tt_app_role;
  
-- Table grants
GRANT SELECT ON TABLE tt."Trainer" TO tt_app_role;

-- Procedure/function grants
GRANT EXECUTE ON FUNCTION tt."GetUserLoginCredentials" TO tt_app_role;