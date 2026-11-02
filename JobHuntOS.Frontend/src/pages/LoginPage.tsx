import { useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
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
    const [agreed, setAgreed] = useState(false)
    const [error, setError] = useState('')

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
                <p className="text-gray-500 text-sm mb-4">
                    {mode === 'login' ? 'Log in to your account' : 'Create an account'}
                </p>

                <p className="text-xs bg-yellow-50 border border-yellow-200 text-yellow-800 rounded p-2 mb-4">
                    Demo project. Please use sample data, not your real CV.
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

                    {mode === 'register' && (
                        <label className="flex items-start gap-2 text-xs text-gray-600">
                            <input
                                type="checkbox"
                                checked={agreed}
                                onChange={e => setAgreed(e.target.checked)}
                                className="mt-0.5"
                            />
                            <span>
                                I agree to the{' '}
                                <Link to="/terms" className="text-blue-600 hover:underline">Terms</Link>{' '}
                                and have read the{' '}
                                <Link to="/privacy" className="text-blue-600 hover:underline">Privacy Notice</Link>.
                            </span>
                        </label>
                    )}

                    {error && <p className="text-red-600 text-sm">{error}</p>}

                    <button
                        type="submit"
                        disabled={mutation.isPending || (mode === 'register' && !agreed)}
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

                <p className="mt-4 text-xs text-gray-400">
                    <Link to="/terms" className="hover:underline">Terms</Link>
                    {' · '}
                    <Link to="/privacy" className="hover:underline">Privacy</Link>
                </p>
            </div>
        </div>
    )
}

export default LoginPage