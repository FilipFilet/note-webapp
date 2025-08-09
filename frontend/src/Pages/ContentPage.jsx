import Header from '../Modules/Header.jsx';
import Editor from '../Modules/Editor.jsx';
import Sidebar from '../Modules/Sidebar.jsx';
import { useState } from 'react';

export default function ContentPage() {
    const [selectedNote, setSelectedNote] = useState({});

    return (
        <>
            <div className="grid grid-cols-[1fr_4fr] grid-rows-[auto_1fr] h-screen">
                <Header />
                <Sidebar setSelectedNote={setSelectedNote} />
                <Editor selectedNote={selectedNote} />
            </div>

        </>
    )
}