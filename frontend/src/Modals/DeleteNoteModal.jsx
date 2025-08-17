export default function DeleteNoteModal({ onClose, handleDeleteNote, noteToDelete }) {
    return (
        <>
            <div className="absolute left-0 top-0 w-screen h-screen bg-black opacity-50"></div>
            <section className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] bg-[#161616] p-5 rounded-lg shadow-lg text-white w-100">
                <p>Are you sure you want to delete the note with the title "{noteToDelete.title}"?</p>

                <div className="flex justify-center **:text-black **:bg-white gap-3 **:cursor-pointer **:px-3 **:py-1 **:rounded-full">
                    <button onClick={() => handleDeleteNote(noteToDelete)}>Delete</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </section>
        </>
    )
}