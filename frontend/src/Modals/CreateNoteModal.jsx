import { useState } from 'react';

// just for testing
import { jwtDecode } from 'jwt-decode';

export default function CreateNoteModal({ onClose, appendNote, folderId }) {
    const [noteTitle, setNoteTitle] = useState('');

    console.log(`folderId: ${folderId}`);

    const apiUrl = import.meta.env.VITE_API_URL;

    const token = localStorage.getItem('token');

    const tokenToLog = jwtDecode(token);

    async function createNote(e) {
        console.log(tokenToLog);

        e.preventDefault();

        const response = await fetch(`${apiUrl}/notes`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify({ title: noteTitle, content: '', folderId: folderId })
        });

        if (!response.ok) {
            let errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error(`Failed to create note, error: ${errorMessage}`);
            return;
        }

        const newNote = await response.json();
        appendNote(newNote);
        onClose();
    }

    return (
        <>
            <div className='absolute left-0 top-0 w-screen h-screen bg-black opacity-50'></div>
            <form action="" onSubmit={createNote} className="absolute left-[50%] top-[50%] transform -translate-x-1/2 -translate-y-1/2 bg-gray-900 p-3 text-white">
                <h1 className="mb-4">Create note</h1>
                <input type="text" name="" id="" placeholder="Note Title" value={noteTitle} onChange={e => setNoteTitle(e.target.value)} className='border-1 border-gray-400' />
                <br />
                <input type="submit" value="Create Note" />
            </form>
        </>
    )
}