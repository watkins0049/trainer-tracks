CREATE TABLE "tt"."TrainerClients" (
  "ClientId" int PRIMARY KEY,
  "TrainerId" int
);

ALTER TABLE "tt"."TrainerClients" ADD FOREIGN KEY ("TrainerId") REFERENCES "tt"."Trainer" ("TrainerId");
ALTER TABLE "tt"."TrainerClients" ADD FOREIGN KEY ("ClientId") REFERENCES "tt"."Client" ("ClientId");