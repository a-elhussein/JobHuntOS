import client from './client';
import type {DashboardStats} from "../types";


export const dashboardApi = {
    getStats: async (): Promise<DashboardStats> => {
        const response = await client.get<DashboardStats>
        ('/api/dashboard/stats');
        return response.data;
    },
};