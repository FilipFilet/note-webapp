import { createPortal } from 'react-dom';
import { useState } from 'react';
import CreateNoteModal from '../Modals/CreateNoteModal';
import CreateFolderModal from '../Modals/CreateFolderModal';

export default function SideBarBtns({ appendNote, appendFolder }) {
    const [showCreateNoteModal, setShowCreateNoteModal] = useState(false);
    const [showCreateFolderModal, setShowCreateFolderModal] = useState(false);


    return (
        <div>
            <button className="cursor-pointer bg-gray-900 text-white py-1 px-2 m-1" onClick={() => setShowCreateNoteModal(true)}>Create Note</button>
            <button className="cursor-pointer bg-gray-900 text-white py-1 px-2 m-1" onClick={() => setShowCreateFolderModal(true)}>Create Folder</button>

            {
                showCreateNoteModal && createPortal(
                    <CreateNoteModal onClose={() => setShowCreateNoteModal(false)} appendNote={appendNote} />, document.body
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