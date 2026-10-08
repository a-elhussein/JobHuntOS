import client from './client';
import type {AnalysisRequest, AnalysisResponse} from "../types/idex.ts";


export const analysisApi = {
    analyse: async (dto: AnalysisRequest): Promise<AnalysisResponse> => {
        const response = await client.post<AnalysisResponse>
        ('/api/analysis', dto);
        return response.data;
    },

    getByApplicationId: async (applicationId: string): Promise<AnalysisResponse> => {
        const response = await client.get<AnalysisResponse>
        (`/api/analysis/${applicationId}`);
        return response.data;
    },
};