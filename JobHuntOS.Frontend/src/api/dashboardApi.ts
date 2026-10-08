import client from './client';
import type {DashboardStats} from "../types/idex.ts";


export const dashboardApi = {
    getStats: async (): Promise<DashboardStats> => {
        const response = await client.get<DashboardStats>
        ('/api/dashboard/stats');
        return response.data;
    },
};