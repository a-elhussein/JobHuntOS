import type {CreateApplicationDto, JobApplication, UpdateApplicationDto} from "../types/idex.ts";
import client from "./client.ts";

export const applicationsApi = {
    getAll: async (status?:string): Promise<JobApplication[]> => {
        const params = status ? {status} : {};
        const response = await client.get('/api/applications', {params});
        return response.data;
    },

    getById: async (id: string): Promise<JobApplication> => {
        const response = await client.get(`/api/applications/${id}`);
        return response.data;
    },

    create: async (dto: CreateApplicationDto): Promise<JobApplication> => {
        const response = await client.post<JobApplication>
        ('/api/applications', dto);
        return response.data;
    },

    update: async (id: string, dto: UpdateApplicationDto): Promise<JobApplication> => {
        const response = await client.put
        (`/api/applications/${id}`, dto);
        return response.data;
    },

    delete: async (id: string): Promise<void> => {
        await client.delete(`/api/applications/${id}`);
    },

    getReminders: async (): Promise<JobApplication[]> => {
        const response = await client.get<JobApplication[]>
        ('/api/applications/reminders');
        return response.data;
    },
}