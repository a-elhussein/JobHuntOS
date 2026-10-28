import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import {MutationCache, QueryClient, QueryClientProvider} from '@tanstack/react-query'
import { BrowserRouter } from 'react-router-dom'
import './index.css'
import App from './App.tsx'
import {AuthProvider} from "./auth/AuthContext.tsx";
import toast, {Toaster} from "react-hot-toast"
import {getErrorMessage} from "./api/errors.ts";

const queryClient = new QueryClient({
    mutationCache: new MutationCache({
        onError: (error, _variables, _context, mutation) => {
            if (mutation.meta?.silent) return // pages that show their own error
            toast.error(getErrorMessage(error))
        },
    }),
    defaultOptions: {
        queries: {
            staleTime: 1000 * 60 * 5,
            retry: 1,
            refetchOnWindowFocus: false,
        },
    },
})

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <QueryClientProvider client={queryClient}>
                <AuthProvider>
                    <App />
                    <Toaster position="top-right" />
                </AuthProvider>
            </QueryClientProvider>
        </BrowserRouter>
    </StrictMode>,
)