import { jwtDecode } from "jwt-decode"
import { useState } from "react"
import UserModal from "../Modals/UserModal"
import { createPortal } from "react-dom"

export default function Header() {
    const token = localStorage.getItem("token")
    const user = jwtDecode(token)
    const username = user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] // name from the decoded jwt object

    const [showUserModal, setShowUserModal] = useState(false)

    // fetch user data
    // store in state
    // pass state and state function to usermodal

    return (
        <>
            <header className="bg-[#161616] col-span-2 row-start-1 flex justify-between items-center p-3 text-white">
                <h1>My App</h1>
                <figure className="flex items-center cursor-pointer" onClick={() => setShowUserModal(true)}>
                    <figcaption>{username}</figcaption>
                    <img src="https://placehold.co/50x50" alt="Profile Image" className="w-12.5 h-12.5 rounded-full ml-3" />
                </figure>
            </header>

            {showUserModal && createPortal(<UserModal currentUser={user} onClose={() => setShowUserModal(false)} />, document.body)}
        </>
    )
}