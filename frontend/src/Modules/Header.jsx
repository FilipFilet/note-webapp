import { jwtDecode } from "jwt-decode"
import { useState } from "react"
import UserModal from "../Modals/UserModal"
import { createPortal } from "react-dom"

export default function Header() {
    const token = localStorage.getItem("token")
    const [user, setUser] = useState({})

    const [showUserModal, setShowUserModal] = useState(false)

    // fetch user data
    useState(async () => {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/users/me`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })

        if (!response.ok) {
            const errorData = await response.json()
            console.error("Error fetching user data:", errorData.message)
            return
        }

        const userData = await response.json()
        setUser(userData)
    }, [])
    // store in state
    // pass state and state function to usermodal

    return (
        <>
            <header className="bg-[#161616] col-span-2 row-start-1 flex justify-between items-center p-3 text-white">
                <h1>Notes</h1>
                <figure className="flex items-center cursor-pointer" onClick={() => setShowUserModal(true)}>
                    <figcaption>{user.username}</figcaption>
                    <img src="https://placehold.co/50x50" alt="Profile Image" className="w-12.5 h-12.5 rounded-full ml-3" />
                </figure>
            </header>

            {showUserModal && createPortal(<UserModal currentUser={user} onClose={() => setShowUserModal(false)} setUser={setUser} />, document.body)}
        </>
    )
}