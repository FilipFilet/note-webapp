import { useEffect, useState, useRef } from 'react';
import TipTap from './TipTapEditor';

export default function Editor({ selectedNote, setUpdatedNoteData }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    // The current selected note
    const [note, setNote] = useState(selectedNote || {});
    const [editorInstance, setEditorInstance] = useState(null);

    useEffect(() => {
        console.log('note in Editor:', note);
    })

    // The note that has been updated. Represents the previous selected note with a useRef that doesnt get updated on each render
    const prevSelectedNoteRef = useRef(note);

    const token = localStorage.getItem('token');

    // Updates when clicking another note
    async function updateNote() {
        // If there is no note to update (initial load and first click)
        // On the first click, no need to fetch, since the information about it is already fetched in "Sidebar"
        if (!prevSelectedNoteRef.current || !prevSelectedNoteRef.current.id) {
            setNote(selectedNote || {}); // Update the note state with the selected (clicked) note
            prevSelectedNoteRef.current = note || {}; // Update the ref with the current note. the "note" variable is updated on each render
            return; // No note selected or no ID available
        }

        // Fetches the selected note.
        const responseGet = await fetch(`${apiUrl}/notes/${selectedNote.id}`,
            {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            }
        )

        // Before updating the note, save the previous note's content from the editor instance
        if (editorInstance) {
            prevSelectedNoteRef.current.content = JSON.stringify(editorInstance.getJSON());
        }

        // Updates the note in the database (when clicking another note)
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
            console.log('Previous note updated successfully', prevSelectedNoteRef.current);
            setUpdatedNoteData(prevSelectedNoteRef.current);
        }

        if (responseGet.ok) {
            const noteToUpdate = await responseGet.json();
            setNote(noteToUpdate);
            prevSelectedNoteRef.current = noteToUpdate; // Update the ref with the new note, after updating the previous note
        }

    }


    useEffect(() => {
        updateNote();
    }, [selectedNote]);

    function handleNoteChange(e) {
        const { name, value } = e.target;
        const updatedNote = { ...note, [name]: value };
        setNote(updatedNote);
        prevSelectedNoteRef.current = updatedNote;
    }

    return (
        <div className="bg-[#0f0f0f] col-start-2 row-start-2 text-white">
            {Object.keys(note).length > 0 ? ( // Check if the note has more than 0 keys
                <>
                    <input type="text" name="title" id="" value={note.title} onChange={handleNoteChange} />
                    <br />
                    <TipTap contentJSON={note.content} setEditorInstance={setEditorInstance} />
                    {/* <textarea name="content" value={note.content} onChange={handleNoteChange} /> */}
                </>
            )
                : <p>Select a note to view its content</p>}
        </div>
    )
}