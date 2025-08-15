import { createPortal } from 'react-dom';
import { useState } from 'react';
import CreateNoteModal from '../Modals/CreateNoteModal';
import CreateFolderModal from '../Modals/CreateFolderModal';

export default function SideBarBtns({ appendNote, appendFolder }) {
    const [showCreateNoteModal, setShowCreateNoteModal] = useState(false);
    const [showCreateFolderModal, setShowCreateFolderModal] = useState(false);


    return (
        <div className="flex justify-end text-black **:bg-white **:rounded-full **:px-3 **:py-1 **:cursor-pointer gap-4 p-3">
            <button onClick={() => setShowCreateNoteModal(true)}>Create Note</button>
            <button onClick={() => setShowCreateFolderModal(true)}>Create Folder</button>

            {
                showCreateNoteModal && createPortal(
                    <CreateNoteModal onClose={() => setShowCreateNoteModal(false)} appendNote={appendNote} folderId={null} />, document.body
                )
            }

            {
                showCreateFolderModal && createPortal(
                    <CreateFolderModal onClose={() => setShowCreateFolderModal(false)} appendFolder={appendFolder} />, document.body
                )
            }
        </div>

    );
}