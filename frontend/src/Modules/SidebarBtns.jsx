import { createPortal } from 'react-dom';
import { useState } from 'react';
import CreateNoteModal from '../Modals/CreateNoteModal';

export default function SideBarBtns({ appendNote }) {
    const [showCreateNoteModal, setShowCreateNoteModal] = useState(false);


    return (
        <div>
            <button className="cursor-pointer bg-gray-900 text-white py-1 px-2 m-1" onClick={() => setShowCreateNoteModal(true)}>Create Note</button>
            <button className="cursor-pointer bg-gray-900 text-white py-1 px-2 m-1">Create Folder</button>

            {
                showCreateNoteModal && createPortal(
                    <CreateNoteModal onClose={() => setShowCreateNoteModal(false)} appendNote={appendNote} />, document.body
                )
            }
        </div>

    );
}