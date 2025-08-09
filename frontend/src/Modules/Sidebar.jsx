import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';

export default function Sidebar({ setSelectedNote }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [data, setData] = useState({ folders: [], notes: [] });

    const token = localStorage.getItem('token')
    const decodedToken = token ? jwtDecode(token) : null;

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


    }, []); // Should refetch when the data changes

    return (
        <aside className="bg-red-600 col-start-1 row-start-2 box-border">
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