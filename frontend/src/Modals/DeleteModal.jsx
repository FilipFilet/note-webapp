export default function DeleteModal({ onClose, handleDelete, itemToDelete, message }) {


    return (
        <>
            <div className="absolute left-0 top-0 w-screen h-screen bg-black opacity-50"></div>
            <section className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] bg-[#161616] p-5 rounded-lg shadow-lg text-white w-100">
                {/* Are you sure you want to delete the folder with the title? */}
                <p>{message}</p>

                <div>
                    <button onClick={() => handleDelete(itemToDelete)}>Delete</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </section>
        </>
    )
}