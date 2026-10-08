import { Routes, Route, Navigate } from 'react-router-dom'
import DashboardPage from './pages/DashboardPage'
import ApplicationsPage from './pages/ApplicationsPage'
import AnalysisPage from './pages/AnalysisPage'
import SettingsPage from './pages/SettingsPage'

function App() {
  return (
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Navigate to="/dashboard" replace />} />
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/applications" element={<ApplicationsPage />} />
          <Route path="/analysis" element={<AnalysisPage />} />
          <Route path="/settings" element={<SettingsPage />} />
        </Routes>
      </div>
  )
}

export default App