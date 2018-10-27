CREATE TABLE "tt"."TrainerCredentials" (
  "TrainerId" int PRIMARY KEY,
  "Password" varchar,
  "Salt" varchar
);

ALTER TABLE "tt"."TrainerCredentials" ADD FOREIGN KEY ("TrainerId") REFERENCES "tt"."Trainer" ("TrainerId");