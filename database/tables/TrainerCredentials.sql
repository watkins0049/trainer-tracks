CREATE TABLE "tt"."TrainerCredentials" (
  "EmailAddress" varchar PRIMARY KEY,
  "Hash" varchar,
  "Salt" varchar
);

ALTER TABLE "tt"."TrainerCredentials" ADD FOREIGN KEY ("EmailAddress") REFERENCES "tt"."Trainer" ("EmailAddress");