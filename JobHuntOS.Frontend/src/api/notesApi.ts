import type {CreateNoteDto, Note} from "../types/idex.ts";
import client from "./client.ts";

export const notesApi = {
    getByApplicationId: async (applicationId: string): Promise<Note[]> => {
        const response = await client.get
        (`/api/applications/${applicationId}/notes`);
        return response.data;
    },

    create: async (applicationId: string, dto: CreateNoteDto): Promise<Note> => {
        const response = await client.post
        (`/api/applications/${applicationId}/notes`, dto);
        return response.data;
    },

    delete: async (applicationId: string, noteId: string): Promise<void> => {
        await client.delete(`/api/applications/${applicationId}/notes/${noteId}`);
    },
}