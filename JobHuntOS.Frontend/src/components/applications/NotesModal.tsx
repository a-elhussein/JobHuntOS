import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { notesApi } from '../../api/notesApi'

interface Props {
    applicationId: string
    companyName: string
    onClose: () => void
}

const NotesModal = ({ applicationId, companyName, onClose }: Props) => {
    const queryClient = useQueryClient()
    const [content, setContent] = useState('')

    const { data: notes, isLoading } = useQuery({
        queryKey: ['notes', applicationId],
        queryFn: () => notesApi.getByApplicationId(applicationId),
    })

    const refresh = () =>
        queryClient.invalidateQueries({ queryKey: ['notes', applicationId] })

    const createMutation = useMutation({
        mutationFn: () => notesApi.create(applicationId, { content }),
        onSuccess: () => {
            setContent('')
            refresh()
        },
    })

    const deleteMutation = useMutation({
        mutationFn: (noteId: string) => notesApi.delete(applicationId, noteId),
        onSuccess: refresh,
    })

    return (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-full max-w-lg max-h-[80vh] flex flex-col">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-lg font-bold">Notes: {companyName}</h2>
                    <button onClick={onClose} className="text-gray-400 hover:text-gray-600">✕</button>
                </div>

                <div className="flex-1 overflow-auto space-y-2 mb-4">
                    {isLoading && <p className="text-sm text-gray-500">Loading...</p>}
                    {notes?.length === 0 && (
                        <p className="text-sm text-gray-500">No notes yet.</p>
                    )}
                    {notes?.map(note => (
                        <div key={note.id} className="border border-gray-200 rounded-lg p-3">
                            <p className="text-sm text-gray-800 whitespace-pre-wrap">{note.content}</p>
                            <div className="flex justify-between items-center mt-2">
                <span className="text-xs text-gray-400">
                  {new Date(note.createdAt).toLocaleString()}
                </span>
                                <button
                                    onClick={() => deleteMutation.mutate(note.id)}
                                    className="text-red-500 hover:text-red-700 text-xs"
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                    ))}
                </div>

                <textarea
                    value={content}
                    onChange={e => setContent(e.target.value)}
                    placeholder="Add a note (recruiter name, interview feedback, next steps...)"
                    rows={3}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm resize-none"
                />
                <button
                    onClick={() => createMutation.mutate()}
                    disabled={createMutation.isPending || !content.trim()}
                    className="mt-3 bg-blue-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-blue-700 disabled:opacity-50"
                >
                    {createMutation.isPending ? 'Saving...' : 'Add note'}
                </button>
            </div>
        </div>
    )
}

export default NotesModal