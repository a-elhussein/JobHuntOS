import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useMutation } from '@tanstack/react-query'
import { authApi } from '../api/authApi'
import { useAuth } from '../auth/AuthContext'
import { warmUpApi } from '../api/health'

const LoginPage = () => {
    const { login } = useAuth()
    const navigate = useNavigate()
    const [mode, setMode] = useState<'login' | 'register'>('login')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')

    // Wake the API container while the user is still typing
    useEffect(() => {
        warmUpApi()
    }, [])

    const mutation = useMutation({
        meta: { silent: true },
        mutationFn: () =>
            mode === 'login'
                ? authApi.login(email, password)
                : authApi.register(email, password),
        onSuccess: data => {
            login(data.token)
            navigate('/dashboard', { replace: true })
        },
        onError: (err: any) => {
            const data = err.response?.data
            setError(
                Array.isArray(data) ? data.join(' ') : data?.message ?? 'Something went wrong'
            )
        },
    })

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        setError('')
        mutation.mutate()
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
            <div className="bg-white border border-gray-200 rounded-lg p-8 w-full max-w-sm">
                <h1 className="text-2xl font-bold mb-1">Job Hunt OS</h1>
                <p className="text-gray-500 text-sm mb-6">
                    {mode === 'login' ? 'Log in to your account' : 'Create an account'}
                </p>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <input
                        type="email"
                        required
                        placeholder="Email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    <input
                        type="password"
                        required
                        minLength={8}
                        placeholder="Password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    {mode === 'register' && (
                        <p className="text-xs text-gray-500">
                            8+ characters, with a number and a symbol (for example !).
                        </p>
                    )}

                    {error && <p className="text-red-600 text-sm">{error}</p>}

                    <button
                        type="submit"
                        disabled={mutation.isPending}
                        className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50"
                    >
                        {mutation.isPending ? 'Please wait...' : mode === 'login' ? 'Log in' : 'Register'}
                    </button>

                    {mutation.isPending && (
                        <p className="text-xs text-gray-500 text-center">
                            First visit can take up to 30 seconds while the server wakes up.
                        </p>
                    )}
                </form>

                <button
                    onClick={() => { setMode(mode === 'login' ? 'register' : 'login'); setError('') }}
                    className="mt-4 text-sm text-blue-600 hover:underline"
                >
                    {mode === 'login' ? "No account? Register" : 'Have an account? Log in'}
                </button>
            </div>
        </div>
    )
}

export default LoginPage