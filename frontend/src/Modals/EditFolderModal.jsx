import { useState } from 'react';

export default function EditFolderModal({ selectedFolder, onClose, handleUpdateFolder }) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const accessToken = localStorage.getItem('accessToken')

    const [folder, setFolder] = useState(selectedFolder);

    async function editFolder(e) {
        e.preventDefault();

        let response = await fetch(`${apiUrl}/folder/${folder.id}`, {
            method: 'PUT',
            body: JSON.stringify({ name: folder.name }),
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${accessToken}`
            }
        })

        if (!response.ok) {
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error(`Failed to edit folder, error: ${errorMessage}`);
        }

        setFolder({}); // Reset the folder variable with empty object
        handleUpdateFolder(folder); // Update the folder in the sidebar
        onClose(); // Close the sidebar
    }

    return (
        <>
            <div className="absolute left-0 top-0 w-screen h-screen bg-black opacity-50"></div>
            <form action="" onSubmit={editFolder} className="absolute left-[50%] top-[50%] transform -translate-x-1/2 -translate-y-1/2 bg-gray-900 p-3 text-white">
                <h1 className="mb-4">Edit Folder</h1>
                <input type="text" name="" id="" value={folder.name} onChange={(e) => setFolder({ ...folder, name: e.target.value })} className="border-1 border-gray-400" />
                <br />
                <input type="submit" value="Edit Folder" />
            </form>
        </>
    )
}