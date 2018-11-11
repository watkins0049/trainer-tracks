CREATE TABLE "tt"."Client" (
  "ClientId" serial PRIMARY KEY,
  "FirstName" varchar,
  "LastName" varchar,
  "EmailAddress" varchar,
  "Sex" char,
  "DateOfBirth" date,
  "PhoneNumber" varchar(10),
  "Occupation" varchar
);