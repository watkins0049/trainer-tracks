-- User: tt_app_user
-- DROP USER tt_app_user;

CREATE USER tt_app_user WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;

GRANT tt_app_role TO tt_app_user;