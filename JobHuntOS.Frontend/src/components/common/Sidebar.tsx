import { NavLink } from 'react-router-dom'
import {useAuth} from "../../auth/AuthContext.tsx";

const navigation = [
    { name: 'Dashboard', href: '/dashboard', icon: '📊' },
    { name: 'Applications', href: '/applications', icon: '📝' },
    { name: 'Analysis', href: '/analysis', icon: '🤖' },
    { name: 'Settings', href: '/settings', icon: '⚙️' },
]

const Sidebar = () => {
    const { logout } = useAuth()
    return (
        <div className="w-64 h-full bg-gray-900 text-white flex flex-col">
            <div className="p-6 border-b border-gray-700">
                <h1 className="text-xl font-bold text-white">Job Hunt OS</h1>
                <p className="text-gray-400 text-sm mt-1">Track your job search</p>
            </div>

            <nav className="flex-1 p-4 space-y-1">
                {navigation.map((item) => (
                    <NavLink
                        key={item.name}
                        to={item.href}
                        className={({ isActive }) =>
                            `flex items-center gap-3 px-4 py-3 rounded-lg text-sm font-medium transition-colors ${
                                isActive
                                    ? 'bg-blue-600 text-white'
                                    : 'text-gray-300 hover:bg-gray-800 hover:text-white'
                            }`
                        }
                    >
                        <span>{item.icon}</span>
                        <span>{item.name}</span>
                    </NavLink>
                ))}
            </nav>
            <div className="p-4 border-t border-gray-700">
                <button
                    onClick={logout}
                    className="w-full text-left px-4 py-3 text-sm text-gray-300 hover:bg-gray-800 hover:text-white rounded-lg"
                >
                    🚪 Log out
                </button>
            </div>
        </div>
    )
}

export default Sidebar