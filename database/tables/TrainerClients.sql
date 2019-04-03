CREATE TABLE "tt"."TrainerClients" (
  "ClientEmailAddress" varchar,
  "TrainerEmailAddress" varchar,
	PRIMARY KEY ("ClientEmailAddress", "TrainerEmailAddress")
);

ALTER TABLE "tt"."TrainerClients" ADD FOREIGN KEY ("TrainerEmailAddress") REFERENCES "tt"."Trainer" ("EmailAddress");
ALTER TABLE "tt"."TrainerClients" ADD FOREIGN KEY ("ClientEmailAddress") REFERENCES "tt"."Client" ("EmailAddress");