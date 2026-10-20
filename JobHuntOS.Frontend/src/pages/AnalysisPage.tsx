import { useState } from 'react'
import { useMutation, useQuery } from '@tanstack/react-query'
import { analysisApi } from '../api/analysisApi'
import { cvApi } from '../api/cvApi'

const AnalysisPage = () => {
    const [jobDescription, setJobDescription] = useState('')

    const { data: cv } = useQuery({
        queryKey: ['cv'],
        queryFn: cvApi.getCurrent,
        retry: false,
    })

    const analyseMutation = useMutation({
        mutationFn: () => analysisApi.analyse({ jobDescription }),
    })

    const handleAnalyse = () => {
        if (!jobDescription.trim()) return
        analyseMutation.mutate()
    }

    return (
        <div className="p-8 max-w-4xl">
            <h1 className="text-2xl font-bold text-gray-900 mb-2">JD Analysis</h1>
            <p className="text-gray-500 mb-6">Paste a job description to get an AI-powered fit analysis against your CV</p>

            {!cv && (
                <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6 text-yellow-800 text-sm">
                    ⚠️ No CV uploaded. Go to Settings to upload your CV first.
                </div>
            )}

            <div className="mb-4">
                <label className="block text-sm font-medium text-gray-700 mb-2">Job Description</label>
                <textarea
                    value={jobDescription}
                    onChange={e => setJobDescription(e.target.value)}
                    placeholder="Paste the full job description here..."
                    rows={10}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm resize-none focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
            </div>

            <button
                onClick={handleAnalyse}
                disabled={analyseMutation.isPending || !jobDescription.trim() || !cv}
                className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 transition-colors"
            >
                {analyseMutation.isPending ? 'Analysing...' : 'Analyse Fit'}
            </button>

            {analyseMutation.data && (
                <div className="mt-8 space-y-6">
                    {/* Fit Score */}
                    <div className="bg-white border border-gray-200 rounded-lg p-6">
                        <h2 className="text-lg font-bold mb-2">Fit Score</h2>
                        <div className="flex items-center gap-4">
                            <div className={`text-5xl font-bold ${
                                analyseMutation.data.fitScore >= 70 ? 'text-green-600' :
                                    analyseMutation.data.fitScore >= 40 ? 'text-yellow-600' : 'text-red-600'
                            }`}>
                                {analyseMutation.data.fitScore}%
                            </div>
                            <div className="flex-1 bg-gray-200 rounded-full h-3">
                                <div
                                    className={`h-3 rounded-full ${
                                        analyseMutation.data.fitScore >= 70 ? 'bg-green-500' :
                                            analyseMutation.data.fitScore >= 40 ? 'bg-yellow-500' : 'bg-red-500'
                                    }`}
                                    style={{ width: `${analyseMutation.data.fitScore}%` }}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Matching Skills */}
                    <div className="bg-white border border-gray-200 rounded-lg p-6">
                        <h2 className="text-lg font-bold mb-3">Matching Skills</h2>
                        <div className="flex flex-wrap gap-2">
                            {analyseMutation.data.matchingSkills.map(skill => (
                                <span key={skill} className="px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm">
                  ✓ {skill}
                </span>
                            ))}
                        </div>
                    </div>

                    {/* Missing Skills */}
                    <div className="bg-white border border-gray-200 rounded-lg p-6">
                        <h2 className="text-lg font-bold mb-3">Missing Skills</h2>
                        <div className="flex flex-wrap gap-2">
                            {analyseMutation.data.missingSkills.length === 0 ? (
                                <p className="text-gray-500 text-sm">No missing skills identified</p>
                            ) : (
                                analyseMutation.data.missingSkills.map(skill => (
                                    <span key={skill} className="px-3 py-1 bg-red-100 text-red-700 rounded-full text-sm">
                    ✗ {skill}
                  </span>
                                ))
                            )}
                        </div>
                    </div>

                    {/* Recommendations */}
                    <div className="bg-white border border-gray-200 rounded-lg p-6">
                        <h2 className="text-lg font-bold mb-3">Recommendations</h2>
                        <p className="text-gray-700 text-sm leading-relaxed">{analyseMutation.data.recommendations}</p>
                    </div>
                </div>
            )}
        </div>
    )
}

export default AnalysisPage