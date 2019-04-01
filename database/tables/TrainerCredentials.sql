CREATE TABLE "tt"."TrainerCredentials" (
  "TrainerId" int PRIMARY KEY,
  "Hash" varchar,
  "Salt" varchar
);

ALTER TABLE "tt"."TrainerCredentials" ADD FOREIGN KEY ("TrainerId") REFERENCES "tt"."Trainer" ("TrainerId");