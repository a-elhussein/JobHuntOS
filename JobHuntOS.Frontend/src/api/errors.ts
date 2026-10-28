import axios from 'axios'

export const getErrorMessage = (error: unknown): string => {
    if (axios.isAxiosError(error)) {
        if (!error.response) return 'Cannot reach the server'

        const data = error.response.data
        if (Array.isArray(data)) return data.join(' ')
        if (data?.detail) return data.detail
        if (data?.message) return data.message
    }
    return 'Something went wrong'
}