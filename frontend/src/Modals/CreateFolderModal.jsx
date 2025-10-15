import { useState } from "react";

export default function CreateFolderModal({ onClose, appendFolder }) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const accessToken = localStorage.getItem('accessToken');

    const [folderName, setFolderName] = useState("");

    async function createFolder(e) {
        e.preventDefault();

        let response = await fetch(`${apiUrl}/folder`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${accessToken}`
            },
            body: JSON.stringify({ name: folderName })
        });

        if (!response.ok) {
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error(`Failed to create folder, error: ${errorMessage}`);
            return;
        }

        const newFolder = await response.json();
        appendFolder(newFolder);
        onClose();
    }

    return (
        <>
            <div className='absolute left-0 top-0 w-screen h-screen bg-black opacity-50'></div>
            <form action="" onSubmit={createFolder} className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] flex flex-col gap-4 bg-[#161616] p-5 rounded-lg shadow-lg text-white ">
                <h1 className="mb-4">Create Folder</h1>
                <input type="text" placeholder="Folder Name" value={folderName} onChange={(e) => setFolderName(e.target.value)} className='border-1 border-gray-400' />
                <br />
                <div className="flex gap-2">
                    <button type="submit" className='bg-white text-black py-1 flex-1 cursor-pointer'>Create Folder</button>
                    <button type="button" className='bg-white text-black py-1 flex-1 cursor-pointer' onClick={onClose}>Close</button>

                </div>
            </form>
        </>
    )
}