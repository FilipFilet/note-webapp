import { useState } from "react";
import { useNavigate } from "react-router";
import { jwtDecode } from "jwt-decode";

export default function UserModal({ currentUser, onClose }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [user, setUser] = useState(currentUser);
    const [isEditing, setIsEditing] = useState(false);
    const navigate = useNavigate();

    const token = localStorage.getItem("token");

    const [username, setUsername] = useState(user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]);


    function logout() {
        localStorage.removeItem("token");
        navigate("/"); // Redirect to the home page after logout
    }

    async function updateUsername(newName) {

        const response = await fetch(`${apiUrl}/users/updateuserinfo`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({ username: newName })
        });

        if (!response.ok) {
            const error = await response.json();
            console.error("Error updating username:", error);
            return;
        }

        setUser(prevUser => ({ // prevUser represents the latest user state.
            ...prevUser,
            // change this to username
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": newName
        }))

        // Close editing mode
        setIsEditing(false);
    }

    async function deleteUser() {
        const response = await fetch(`${apiUrl}/users/deleteuser`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const error = await response.json();
            console.error("Error deleting user:", error);
            return;
        }

        // Logout the user after successful deletion
        logout();
    }


    return (
        <>
            <div className="absolute left-0 top-0 w-screen h-screen bg-black opacity-50"></div>
            <section className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] bg-[#161616] p-5 rounded-lg shadow-lg text-white w-100">
                <h1 className="text-center">Profile</h1>

                <div className="flex justify-between">
                    <figure className="flex items-center gap-3">
                        <img src="https://placehold.co/100x100" alt="Profile Image" className="rounded-full" />

                        {
                            isEditing ?
                                <input type="text" value={username} onChange={(e) => { setUsername(e.target.value) }} />
                                :
                                <figcaption>{username}</figcaption>
                        }

                    </figure>

                    {
                        isEditing ?
                            <div className="flex flex-col gap-2 **:cursor-pointer **:bg-white **:text-black **:px-3 **:py-1 **:rounded-full">
                                <button onClick={() => updateUsername(username)}>Confirm</button>
                                <br />
                                <button onClick={() => setIsEditing(false)}>Cancel</button>
                            </div>
                            :
                            <div className="flex flex-col gap-2 **:cursor-pointer **:bg-white **:text-black **:px-3 **:py-1 **:rounded-full">
                                <button onClick={() => setIsEditing(true)}>Edit Username</button>
                                <br />
                                <button onClick={deleteUser}>Delete Account</button>
                            </div>
                    }
                </div>

                <div className="flex justify-center gap-3">
                    <button onClick={onClose} className="cursor-pointer bg-white text-black px-3 py-1 rounded-full">Close</button>
                    <button onClick={logout} className="cursor-pointer bg-white text-black px-3 py-1 rounded-full">Logout</button>
                </div>
            </section>
        </>
    );
}