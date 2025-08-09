import { useEffect, useState, useRef } from 'react';
import { jwtDecode } from 'jwt-decode';

export default function Editor({ selectedNote }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [note, setNote] = useState(selectedNote || {});
    const prevSelectedNoteRef = useRef(note);

    const token = localStorage.getItem('token');
    const decodedToken = token ? jwtDecode(token) : null;


    useEffect(() => {
        async function updateNote() {
            console.log("Selected note:", selectedNote);

            if (!prevSelectedNoteRef.current || !prevSelectedNoteRef.current.id) {
                return; // No note selected or no ID available
            }

            const responseGet = await fetch(`${apiUrl}/notes/${selectedNote.id}`,
                {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`
                    }
                }
            )

            const responsePut = await fetch(`${apiUrl}/notes/${prevSelectedNoteRef.current.id}`,
                {
                    method: 'PUT',
                    body: JSON.stringify(prevSelectedNoteRef.current),
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`
                    },
                }
            );

            if (responsePut.ok) {
                console.log("Note updated successfully");
            }

            if (responseGet.ok) {
                const updatedNote = await responseGet.json();
                setNote(updatedNote);
                prevSelectedNoteRef.current = updatedNote; // Update the ref with the new note
            }

        }

        updateNote();
        setNote(selectedNote || {}); // Update the note state with the selected note
        prevSelectedNoteRef.current = note || {}; // Update the ref with the current note
    }, [selectedNote]);

    function handleNoteChange(e) {
        const { name, value } = e.target;
        const updatedNote = { ...note, [name]: value };
        setNote(updatedNote);
        prevSelectedNoteRef.current = updatedNote;
    }

    return (
        <div className="bg-green-600 col-start-2 row-start-2">
            {Object.keys(note).length > 0 ? ( // Check if the note has more than 0 keys
                <>
                    <h1>{note.title}</h1>
                    <textarea name="content" value={note.content} onChange={handleNoteChange} />
                </>
            )
                : <p>Select a note to view its content</p>}
        </div>
    )
}