import { useEffect, useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import SideBarBtns from './SidebarBtns';
import { createPortal } from 'react-dom';
import EditFolderModal from '../Modals/EditFolderModal';
import CreateNoteModal from '../Modals/CreateNoteModal';
import React from 'react';
import DeleteNoteModal from '../Modals/DeleteNoteModal';
import DeleteFolderModal from '../Modals/DeleteFolderModal';

export default function Sidebar({ updatedNoteData, setSelectedNote }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [data, setData] = useState({ folders: [], notes: [] });
    const [showEditModal, setShowEditModal] = useState(false);
    const [selectedFolder, setSelectedFolder] = useState({});
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [showDeleteNoteModal, setShowDeleteNoteModal] = useState(false);
    const [noteToDelete, setNoteToDelete] = useState({});
    const [folderToDelete, setFolderToDelete] = useState({});
    const [showDeleteFolderModal, setShowDeleteFolderModal] = useState(false);

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

    async function handleDeleteNote(note) {
        const response = await fetch(`${apiUrl}/notes/${note.id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error(`Failed to delete note, error: ${errorMessage}`);
            return;
        }

        // Remove the note from the sidebar
        setData(prevData => {

            // If note is not in a folder
            if (!note.folderId) {
                return { ...prevData, notes: prevData.notes.filter(n => n.id !== note.id) };

                // If note is contained in a folder
            } else {
                // Returns a new array of folders, where the found note is removed from its folder
                const folders = prevData.folders.map(f => {
                    if (f.id === note.folderId) {
                        // Returns a new array of notes, where the note to be removed is filtered out
                        return { ...f, notes: f.notes.filter(n => n.id !== note.id) };
                    }
                    return f;
                });

                // Returns new data object with the updated folder array
                return { ...prevData, folders };
            }
        });

        setShowDeleteNoteModal(false); // Close the delete note modal
    }


    async function handleDeleteFolder(folder) {
        // Possibly move the fetch to the modal? The sidebar shouldnt have this responsability, only that of which is concerned with the sidebar
        const response = await fetch(`${apiUrl}/folder/${folder.id}`,
            {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`
                }
            }
        );

        if (!response.ok) {
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error(`Failed to delete folder, error: ${errorMessage}`);
        }

        // Remove folder from sidebar
        setData(prevData => ({
            ...prevData,

            // filters out the deleted folder
            folders: prevData.folders.filter(f => f.id !== folder.id)
        }))

        setShowDeleteFolderModal(false); // Close the delete folder modal
    }

    // useEffect for fetching user notes and folders
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
        }

        fetchUserNotesFolders();

    }, []);

    // useEffect for updating notes. Rerender sidebar on note update
    // In a useState because it should trigger a re-render when updatedNoteData changes
    useEffect(() => {
        handleUpdateNote(updatedNoteData);
    }, [updatedNoteData]);

    return (
        <aside className="bg-[#161616] col-start-1 row-start-2 box-border text-white">
            <SideBarBtns appendNote={appendNote} appendFolder={appendFolder} />

            <ul className='pl-5'>
                {
                    data ? (
                        <>
                            {data.folders.map(folder => (
                                <React.Fragment key={folder.id}>
                                    <li >
                                        <div className='hover:bg-[#252525] flex justify-between pr-2 pl-2 group'>
                                            <p>{folder.name}</p>
                                            <div className='flex gap-2 **:cursor-pointer **:opacity-50 **:hover:opacity-100'>
                                                <button className='hidden group-hover:inline' onClick={() => { setFolderToDelete(folder); setShowDeleteFolderModal(true); }}>Delete</button>
                                                <button className='hidden group-hover:inline' onClick={() => { setShowEditModal(true); setSelectedFolder(folder); }}>Edit</button>
                                                <button className='hidden group-hover:inline' onClick={() => { setShowCreateModal(true); setSelectedFolder(folder); }}>+</button>
                                            </div>
                                        </div>
                                    </li>
                                    <ul className='pl-5'>
                                        {
                                            folder.notes.map(note => (
                                                <li className="hover:bg-[#252525] cursor-pointer group" onClick={() => setSelectedNote(note)} key={note.id}>
                                                    <div className='flex justify-between pr-2 pl-2 '>
                                                        <p>{note.title}</p>
                                                        <button className='hidden group-hover:inline  cursor-pointer opacity-50 hover:opacity-100' onClick={(e) => { e.stopPropagation(); setNoteToDelete(note); setShowDeleteNoteModal(true); }}>Delete</button>
                                                    </div>
                                                </li>

                                            ))}
                                    </ul>

                                </React.Fragment>
                            ))}
                            {
                                data.notes.map(note => (
                                    <li onClick={() => setSelectedNote(note)} key={note.id} className="hover:bg-[#252525] cursor-pointer">
                                        <div className='flex justify-between pr-2 pl-2 group'>
                                            <p>{note.title}</p>
                                            <button className='hidden group-hover:inline cursor-pointer opacity-50 hover:opacity-100' onClick={() => { setNoteToDelete(note); setShowDeleteNoteModal(true); }}>Delete</button>
                                        </div>

                                    </li>
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

            {/* Delete functionality is seperated for better separation of concerns */}
            {showDeleteNoteModal && createPortal(
                <DeleteNoteModal onClose={() => setShowDeleteNoteModal(false)} handleDeleteNote={handleDeleteNote} noteToDelete={noteToDelete} />, document.body
            )}

            {showDeleteFolderModal && createPortal(
                <DeleteFolderModal onClose={() => setShowDeleteFolderModal(false)} handleDeleteFolder={handleDeleteFolder} folderToDelete={folderToDelete} />, document.body
            )}
        </aside>
    )
}