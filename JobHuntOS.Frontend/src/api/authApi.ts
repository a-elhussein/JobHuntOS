import client from './client'
import type {AuthResponse} from "../types/idex.ts";


export const authApi = {
    register: async (email: string, password: string, displayName?: string): Promise<AuthResponse> => {
        const response = await client.post<AuthResponse>
        ('/api/auth/register', { email, password, displayName })
        return response.data
    },

    login: async (email: string, password: string): Promise<AuthResponse> => {
        const response = await client.post<AuthResponse>
        ('/api/auth/login', { email, password })
        return response.data
    },
}