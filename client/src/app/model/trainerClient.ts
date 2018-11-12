import { Client } from "./client";

export interface TrainerClient {
    trainerId: number;
    clientId: number;
    client: Client;
}