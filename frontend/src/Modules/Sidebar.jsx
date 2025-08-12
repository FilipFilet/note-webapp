import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import SideBarBtns from './SidebarBtns';

export default function Sidebar({ updatedNoteData, setSelectedNote }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [data, setData] = useState({ folders: [], notes: [] });

    const token = localStorage.getItem('token')
    const decodedToken = token ? jwtDecode(token) : null;

    // Functions
    function appendNote(note) {
        setData(prevData => ({
            ...prevData,
            notes: [...prevData.notes, note]
        }));
    }

    function appendFolder(folder) {
        setData(prevData => ({
            ...prevData,
            folders: [...prevData.folders, folder]
        }));
    }

    function handleUpdateNote(note) {
        setData(prevData => {
            const updatedNotes = prevData.notes.map(n => n.id === note.id ? note : n);
            return { ...prevData, notes: updatedNotes };
        });
    }

    useEffect(() => {
        async function fetchUserNotesFolders() {
            const response = await fetch(`${apiUrl}/users/me/content`,
                {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${token}`
                    }
                }
            );
            const result = await response.json();
            setData(result);
            console.log("Fetched user content:", result);
        }

        fetchUserNotesFolders();


    }, []);

    useEffect(() => {
        handleUpdateNote(updatedNoteData);
    }, [updatedNoteData]);

    return (
        <aside className="bg-red-600 col-start-1 row-start-2 box-border">
            <SideBarBtns appendNote={appendNote} appendFolder={appendFolder} />

            <ul className='pl-5'>
                {
                    data ? (
                        <>
                            {data.folders.map(folder => (
                                <li key={folder.id}>{folder.name}
                                    <ul className='pl-5'>
                                        {
                                            folder.notes.map(note => (
                                                <li onClick={() => setSelectedNote(note)} key={note.id}>{note.title}</li>
                                            ))}
                                    </ul>
                                </li>


                            ))}
                            {
                                data.notes.map(note => (
                                    <li onClick={() => setSelectedNote(note)} key={note.id}>{note.title}</li>
                                ))
                            }
                        </>
                    )
                        : <p>No content available</p>
                }
            </ul>
        </aside>
    )
}