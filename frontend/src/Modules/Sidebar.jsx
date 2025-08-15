import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import SideBarBtns from './SidebarBtns';
import { createPortal } from 'react-dom';
import EditFolderModal from '../Modals/EditFolderModal';
import CreateNoteModal from '../Modals/CreateNoteModal';
import React from 'react';

export default function Sidebar({ updatedNoteData, setSelectedNote }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [data, setData] = useState({ folders: [], notes: [] });
    const [showEditModal, setShowEditModal] = useState(false);
    const [selectedFolder, setSelectedFolder] = useState({});
    const [showCreateModal, setShowCreateModal] = useState(false);

    const token = localStorage.getItem('token')
    const decodedToken = token ? jwtDecode(token) : null;

    // Functions
    function appendNote(note) {
        setData(prevData => {
            if (!note.folderId) {
                return { ...prevData, notes: [...prevData.notes, note] };
            }
            else {
                const folders = prevData.folders.map(f => {
                    if (f.id === note.folderId) {
                        return { ...f, notes: [...f.notes, note] };
                    }
                    return f;
                });
                return { ...prevData, folders };
            }

        });
    }

    function appendFolder(folder) {
        setData(prevData => ({
            ...prevData,
            folders: [...prevData.folders, folder]
        }));
    }

    function handleUpdateNote(note) {

        setData(prevData => {
            // If note is not in a folder
            if (!note.folderId) {
                // returns an array of all the notes without a folderid, and the one note being updated
                const updatedNotes = prevData.notes.map(n => n.id === note.id ? note : n);
                return { ...prevData, notes: updatedNotes };
            }

            // If note is in a folder
            else {
                const folder = prevData.folders.map(
                    f => {
                        if (f.id === note.folderId) {
                            // returns the notes that belong to the folder, with the one note being updated
                            const updatedFolderNotes = f ? f.notes.map(n => n.id === note.id ? note : n) : [];
                            return { ...f, notes: updatedFolderNotes };
                        }
                        return f;
                    }
                );

                return { ...prevData, folders: folder };
            }
        });
    }

    function handleUpdateFolder(folder) {
        setData(prevData => {
            const updatedFolders = prevData.folders.map(f => f.id === folder.id ? folder : f);
            return { ...prevData, folders: updatedFolders };
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
                                <React.Fragment key={folder.id}>
                                    <li className="group" >
                                        <div className='group-hover:bg-amber-500 flex justify-between pr-2'>{folder.name}
                                            <div className='flex gap-2 **:cursor-pointer'>
                                                <button className='hidden group-hover:inline' onClick={() => { setShowEditModal(true); setSelectedFolder(folder); }}>Edit</button>

                                                <button className='hidden group-hover:inline' onClick={() => { setShowCreateModal(true); setSelectedFolder(folder); }}>+</button>
                                            </div>
                                        </div>
                                    </li>
                                    <ul className='pl-5'>
                                        {
                                            folder.notes.map(note => (
                                                <li className="hover:bg-amber-500 cursor-pointer" onClick={() => setSelectedNote(note)} key={note.id}>{note.title}</li>
                                            ))}
                                    </ul>

                                </React.Fragment>
                            ))}
                            {
                                data.notes.map(note => (
                                    <li onClick={() => setSelectedNote(note)} key={note.id} className="hover:bg-amber-500 cursor-pointer">{note.title}</li>
                                ))
                            }
                        </>
                    )
                        : <p>No content available</p>
                }
            </ul>

            {showEditModal && createPortal(
                <EditFolderModal onClose={() => setShowEditModal(false)} handleUpdateFolder={handleUpdateFolder} selectedFolder={selectedFolder} />, document.body
            )}

            {showCreateModal && createPortal(
                <CreateNoteModal onClose={() => setShowCreateModal(false)} appendNote={appendNote} folderId={selectedFolder.id} />, document.body
            )}
        </aside>
    )
}