import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { applicationsApi } from '../api/applicationsApi'
import type { CreateApplicationDto } from '../types/index.ts'
import NotesModal from "../components/applications/NotesModal.tsx";

const ApplicationsPage = () => {
    const queryClient = useQueryClient()
    const [showForm, setShowForm] = useState(false)
    const [statusFilter, setStatusFilter] = useState<number | undefined>(undefined)
    const [notesFor, setNotesFor] = useState<{ id: string; name: string } | null>(null)

    const statuses = [
        { label: 'All', value: undefined },
        { label: 'Applied', value: 0 },
        { label: 'Interview', value: 1 },
        { label: 'Offer', value: 2 },
        { label: 'Rejected', value: 3 },
        { label: 'Withdrawn', value: 4 },
    ]

    const { data: applications, isLoading } = useQuery({
        queryKey: ['applications', statusFilter],
        queryFn: () => applicationsApi.getAll(statusFilter),
    })

    const deleteMutation = useMutation({
        mutationFn: applicationsApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['applications'] })
            queryClient.invalidateQueries({ queryKey: ['dashboard-stats'] })
        },
    })

    const updateMutation = useMutation({
        mutationFn: ({ id, status }: { id: string; status: number }) =>
            applicationsApi.update(id, { status }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['applications'] })
            queryClient.invalidateQueries({ queryKey: ['dashboard-stats'] })
        },
    })

    if (isLoading) return <div className="p-8">Loading...</div>

    return (
        <div className="p-8">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-gray-900">Applications</h1>
                <button
                    onClick={() => setShowForm(true)}
                    className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors"
                >
                    + Add Application
                </button>
            </div>

            <div className="flex gap-2 mb-6">
                {statuses.map(status => (
                    <button
                        key={status.label}
                        onClick={() => setStatusFilter(status.value)}
                        className={`px-3 py-1 rounded-full text-sm font-medium transition-colors ${
                            statusFilter === status.value
                                ? 'bg-blue-600 text-white'
                                : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
                        }`}
                    >
                        {status.label}
                    </button>
                ))}
            </div>

            <div className="bg-white rounded-lg border border-gray-200 overflow-hidden">
                <table className="w-full">
                    <thead className="bg-gray-50 border-b border-gray-200">
                    <tr>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Company</th>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Role</th>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Status</th>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Applied</th>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Follow Up</th>
                        <th className="text-left p-4 text-sm font-medium text-gray-600">Actions</th>
                    </tr>
                    </thead>
                    <tbody>
                    {applications?.map(app => (
                        <tr key={app.id} className="border-b border-gray-100 hover:bg-gray-50">
                            <td className="p-4 font-medium text-gray-900">{app.companyName}</td>
                            <td className="p-4 text-gray-600">{app.jobTitle}</td>
                            <td className="p-4">
                                <StatusBadge status={app.status} />
                            </td>
                            <td className="p-4 text-gray-600 text-sm">
                                {new Date(app.appliedDate).toLocaleDateString()}
                            </td>
                            <td className="p-4 text-gray-600 text-sm">
                                {app.followUpDate
                                    ? new Date(app.followUpDate).toLocaleDateString()
                                    : '-'}
                            </td>
                            <td className="p-4 flex gap-2 items-center">
                                <select
                                    value={app.status}
                                    onChange={e => updateMutation.mutate({ id: app.id, status: parseInt(e.target.value) })}
                                    className="text-sm border border-gray-300 rounded px-2 py-1"
                                >
                                    <option value={0}>Applied</option>
                                    <option value={1}>Interview</option>
                                    <option value={2}>Offer</option>
                                    <option value={3}>Rejected</option>
                                    <option value={4}>Withdrawn</option>
                                </select>
                                <button
                                    onClick={() => setNotesFor({ id: app.id, name: app.companyName })}
                                    className="text-blue-600 hover:text-blue-800 text-sm"
                                >
                                    Notes
                                </button>
                                <button
                                    onClick={() => deleteMutation.mutate(app.id)}
                                    className="text-red-500 hover:text-red-700 text-sm"
                                >
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>

                {applications?.length === 0 && (
                    <div className="p-8 text-center text-gray-500">
                        No applications yet. Add your first one!
                    </div>
                )}
            </div>

            {showForm && (
                <AddApplicationModal
                    onClose={() => setShowForm(false)}
                    onSuccess={() => {
                        setShowForm(false)
                        queryClient.invalidateQueries({ queryKey: ['applications'] })
                        queryClient.invalidateQueries({ queryKey: ['dashboard-stats'] })
                    }}
                />
            )}
            {notesFor && (
                <NotesModal
                    applicationId={notesFor.id}
                    companyName={notesFor.name}
                    onClose={() => setNotesFor(null)}
                />
            )}
        </div>
    )
}

const StatusBadge = ({ status }: { status: number }) => {
    const labels: Record<number, string> = {
        0: 'Applied',
        1: 'Interview',
        2: 'Offer',
        3: 'Rejected',
        4: 'Withdrawn',
    }
    return (
        <span className="px-2 py-1 rounded-full text-xs font-medium bg-gray-100">
      {labels[status] ?? 'Unknown'}
    </span>
    )
}

const AddApplicationModal = ({ onClose, onSuccess }: {
    onClose: () => void
    onSuccess: () => void
}) => {
    const [formData, setFormData] = useState({
        companyName: '',
        jobTitle: '',
        jobUrl: '',
        location: '',
        isRemote: false,
        salaryMin: '',
        salaryMax: '',
        appliedDate: new Date().toISOString().split('T')[0],
        followUpDays: 7,
    })

    const createMutation = useMutation({
        mutationFn: (dto: CreateApplicationDto) => applicationsApi.create(dto),
        onSuccess,
    })

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        createMutation.mutate({
            companyName: formData.companyName,
            jobTitle: formData.jobTitle,
            jobUrl: formData.jobUrl || undefined,
            location: formData.location || undefined,
            isRemote: formData.isRemote,
            salaryMin: formData.salaryMin ? parseInt(formData.salaryMin) : undefined,
            salaryMax: formData.salaryMax ? parseInt(formData.salaryMax) : undefined,
            appliedDate: formData.appliedDate + 'T12:00:00.000Z',
            followUpDays: formData.followUpDays,
        })
    }

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-md">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-lg font-bold">Add Application</h2>
                    <button onClick={onClose} className="text-gray-400 hover:text-gray-600">✕</button>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <input
                        required
                        placeholder="Company Name"
                        value={formData.companyName}
                        onChange={e => setFormData(p => ({ ...p, companyName: e.target.value }))}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    <input
                        required
                        placeholder="Job Title"
                        value={formData.jobTitle}
                        onChange={e => setFormData(p => ({ ...p, jobTitle: e.target.value }))}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    <input
                        placeholder="Job URL"
                        value={formData.jobUrl}
                        onChange={e => setFormData(p => ({ ...p, jobUrl: e.target.value }))}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    <input
                        placeholder="Location"
                        value={formData.location}
                        onChange={e => setFormData(p => ({ ...p, location: e.target.value }))}
                        className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                    />
                    <div className="flex gap-2">
                        <input
                            placeholder="Min Salary"
                            type="number"
                            value={formData.salaryMin}
                            onChange={e => setFormData(p => ({ ...p, salaryMin: e.target.value }))}
                            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                        />
                        <input
                            placeholder="Max Salary"
                            type="number"
                            value={formData.salaryMax}
                            onChange={e => setFormData(p => ({ ...p, salaryMax: e.target.value }))}
                            className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                        />
                    </div>
                    <div className="flex gap-4 items-center">
                        <div>
                            <label className="text-sm text-gray-600">Applied Date</label>
                            <input
                                type="date"
                                value={formData.appliedDate}
                                onChange={e => setFormData(p => ({ ...p, appliedDate: e.target.value }))}
                                className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                            />
                        </div>
                        <div>
                            <label className="text-sm text-gray-600">Follow Up (days)</label>
                            <input
                                type="number"
                                value={formData.followUpDays}
                                onChange={e => setFormData(p => ({ ...p, followUpDays: parseInt(e.target.value) }))}
                                className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm"
                            />
                        </div>
                    </div>
                    <label className="flex items-center gap-2 text-sm">
                        <input
                            type="checkbox"
                            checked={formData.isRemote}
                            onChange={e => setFormData(p => ({ ...p, isRemote: e.target.checked }))}
                        />
                        Remote
                    </label>

                    <div className="flex gap-2 pt-2">
                        <button
                            type="button"
                            onClick={onClose}
                            className="flex-1 border border-gray-300 text-gray-700 py-2 rounded-lg text-sm"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            disabled={createMutation.isPending}
                            className="flex-1 bg-blue-600 text-white py-2 rounded-lg text-sm hover:bg-blue-700 disabled:opacity-50"
                        >
                            {createMutation.isPending ? 'Adding...' : 'Add Application'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default ApplicationsPage