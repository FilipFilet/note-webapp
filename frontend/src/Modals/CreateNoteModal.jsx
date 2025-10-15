import { useState } from 'react';

// just for testing
import { jwtDecode } from 'jwt-decode';

export default function CreateNoteModal({ onClose, appendNote, folderId }) {
    const [noteTitle, setNoteTitle] = useState('');

    console.log(`folderId: ${folderId}`);

    const apiUrl = import.meta.env.VITE_API_URL;

    const accessToken = localStorage.getItem('accessToken');

    const tokenToLog = jwtDecode(accessToken);

    async function createNote(e) {
        console.log(tokenToLog);

        e.preventDefault();

        const response = await fetch(`${apiUrl}/notes`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${accessToken}`
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
            <form action="" onSubmit={createNote} className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] flex flex-col gap-4 bg-[#161616] p-5 rounded-lg shadow-lg text-white ">
                <h1 className="mb-4">Create note</h1>
                <input type="text" name="" id="" placeholder="Note Title" value={noteTitle} onChange={e => setNoteTitle(e.target.value)} className='border-1 border-gray-400' />
                <br />
                <div className="flex gap-2">
                    <input className='bg-white text-black py-1 flex-1 cursor-pointer' type="submit" value="Create Note" />
                    <button type="button" className='bg-white text-black py-1 flex-1 cursor-pointer' onClick={onClose}>Close</button>

                </div>
            </form>
        </>
    )
}