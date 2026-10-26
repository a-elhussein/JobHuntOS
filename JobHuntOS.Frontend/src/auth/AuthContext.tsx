import { createContext, useContext, useState, type ReactNode } from 'react'
import { useQueryClient } from '@tanstack/react-query'
import { tokenStorage } from './tokenStorage'

interface AuthContextValue {
    isAuthenticated: boolean
    login: (token: string) => void
    logout: () => void
}

const AuthContext = createContext<AuthContextValue | null>(null)

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const queryClient = useQueryClient()
    const [isAuthenticated, setIsAuthenticated] = useState(!!tokenStorage.get())

    const login = (token: string) => {
        tokenStorage.set(token)
        queryClient.clear()
        setIsAuthenticated(true)
    }

    const logout = () => {
        tokenStorage.clear()
        queryClient.clear()
        setIsAuthenticated(false)
    }

    return (
        <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
    {children}
    </AuthContext.Provider>
)
}

export const useAuth = () => {
    const ctx = useContext(AuthContext)
    if (!ctx) throw new Error('useAuth must be used inside AuthProvider')
    return ctx
}