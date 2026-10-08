import client from './client';
import type {Cv, UpsertCvDto} from "../types/idex.ts";


export const cvApi = {
    getCurrent: async (): Promise<Cv> => {
        const response = await client.get<Cv>('/api/cv');
        return response.data;
    },

    upsert: async (dto: UpsertCvDto): Promise<Cv> => {
        const response = await client.post<Cv>('/api/cv', dto);
        return response.data;
    },

    delete: async (): Promise<void> => {
        await client.delete('/api/cv');
    },
};