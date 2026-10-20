import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { cvApi } from '../api/cvApi'

const SettingsPage = () => {
    const queryClient = useQueryClient()
    const [cvContent, setCvContent] = useState('')

    const { data: cv } = useQuery({
        queryKey: ['cv'],
        queryFn: cvApi.getCurrent,
        retry: false,
    })

    const upsertMutation = useMutation({
        mutationFn: () => cvApi.upsert({ content: cvContent }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['cv'] })
            setCvContent('')
        },
    })

    const deleteMutation = useMutation({
        mutationFn: cvApi.delete,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['cv'] })
        },
    })

    return (
        <div className="p-8 max-w-2xl">
            <h1 className="text-2xl font-bold text-gray-900 mb-6">Settings</h1>

            <div className="bg-white border border-gray-200 rounded-lg p-6">
                <h2 className="text-lg font-bold mb-2">Your CV</h2>
                <p className="text-gray-500 text-sm mb-4">
                    Paste your CV as plain text. This is used for AI analysis.
                </p>

                {cv && (
                    <div className="mb-4 p-3 bg-green-50 border border-green-200 rounded-lg flex justify-between items-center">
                        <p className="text-green-700 text-sm">✓ CV uploaded on {new Date(cv.uploadedAt).toLocaleDateString()}</p>
                        <button
                            onClick={() => deleteMutation.mutate()}
                            className="text-red-500 hover:text-red-700 text-sm"
                        >
                            Remove
                        </button>
                    </div>
                )}

                <textarea
                    value={cvContent}
                    onChange={e => setCvContent(e.target.value)}
                    placeholder="Paste your CV content here..."
                    rows={12}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                />

                <button
                    onClick={() => upsertMutation.mutate()}
                    disabled={upsertMutation.isPending || !cvContent.trim()}
                    className="mt-4 bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 transition-colors"
                >
                    {upsertMutation.isPending ? 'Saving...' : cv ? 'Replace CV' : 'Save CV'}
                </button>
            </div>
        </div>
    )
}

export default SettingsPage