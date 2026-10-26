import { Routes, Route, Navigate, Outlet } from 'react-router-dom'
import Sidebar from './components/common/Sidebar'
import DashboardPage from './pages/DashboardPage'
import ApplicationsPage from './pages/ApplicationsPage'
import AnalysisPage from './pages/AnalysisPage'
import SettingsPage from './pages/SettingsPage'
import LoginPage from './pages/LoginPage'
import { useAuth } from './auth/AuthContext'

const ProtectedLayout = () => {
    const { isAuthenticated } = useAuth()
    if (!isAuthenticated) return <Navigate to="/login" replace />

    return (
        <div className="flex h-screen overflow-hidden bg-gray-50">
            <Sidebar />
            <main className="flex-1 overflow-auto">
                <Outlet />
            </main>
        </div>
    )
}

function App() {
    return (
        <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route element={<ProtectedLayout />}>
                <Route path="/" element={<Navigate to="/dashboard" replace />} />
                <Route path="/dashboard" element={<DashboardPage />} />
                <Route path="/applications" element={<ApplicationsPage />} />
                <Route path="/analysis" element={<AnalysisPage />} />
                <Route path="/settings" element={<SettingsPage />} />
            </Route>
        </Routes>
    )
}

export default App