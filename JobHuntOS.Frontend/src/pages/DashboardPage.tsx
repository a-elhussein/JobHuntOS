import { useQuery } from '@tanstack/react-query'
import { dashboardApi } from '../api/dashboardApi'

const DashboardPage = () => {
    const { data: stats, isLoading, isError } = useQuery({
        queryKey: ['dashboard-stats'],
        queryFn: dashboardApi.getStats,
    })

    if (isLoading) return <div className="p-8">Loading...</div>
    if (isError) return <div className="p-8 text-red-500">Failed to load stats</div>

    return (
        <div className="p-8">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">Dashboard</h1>

            <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
                <StatCard label="Total Applications" value={stats?.totalApplications ?? 0} color="blue" />
                <StatCard label="Applied" value={stats?.applied ?? 0} color="yellow" />
                <StatCard label="Interviews" value={stats?.interviews ?? 0} color="purple" />
                <StatCard label="Offers" value={stats?.offers ?? 0} color="green" />
                <StatCard label="Rejected" value={stats?.rejected ?? 0} color="red" />
                <StatCard label="Withdrawn" value={stats?.withdrawn ?? 0} color="gray" />
                <StatCard label="Overdue Follow Ups" value={stats?.overdueFollowUps ?? 0} color="orange" />
                <StatCard label="Response Rate" value={`${stats?.responseRate ?? 0}%`} color="teal" />
            </div>
        </div>
    )
}

interface StatCardProps {
    label: string
    value: number | string
    color: string
}

const StatCard = ({ label, value, color }: StatCardProps) => {
    const colorMap: Record<string, string> = {
        blue: 'bg-blue-50 text-blue-700 border-blue-200',
        yellow: 'bg-yellow-50 text-yellow-700 border-yellow-200',
        purple: 'bg-purple-50 text-purple-700 border-purple-200',
        green: 'bg-green-50 text-green-700 border-green-200',
        red: 'bg-red-50 text-red-700 border-red-200',
        gray: 'bg-gray-50 text-gray-700 border-gray-200',
        orange: 'bg-orange-50 text-orange-700 border-orange-200',
        teal: 'bg-teal-50 text-teal-700 border-teal-200',
    }

    return (
        <div className={`p-4 rounded-lg border ${colorMap[color]}`}>
            <p className="text-sm font-medium">{label}</p>
            <p className="text-3xl font-bold mt-1">{value}</p>
        </div>
    )
}

export default DashboardPage