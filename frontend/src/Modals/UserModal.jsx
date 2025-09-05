import { useState } from "react";
import { useNavigate } from "react-router";
import { jwtDecode } from "jwt-decode";
import { useRef } from "react";

export default function UserModal({ currentUser, onClose, setUser }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [isEditing, setIsEditing] = useState(false);
    const preEditUsername = useRef(currentUser.username);

    const navigate = useNavigate();

    const token = localStorage.getItem("token");


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
            const errorData = await response.text();
            console.error("Error updating username:", errorData);
            return;
        }

        setUser(prevUser => ({ // prevUser represents the latest user state.
            ...prevUser,
            username: newName
        }))

        preEditUsername.current = currentUser.username; // Update the pre-edit username

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
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            console.error("Error deleting user:", errorMessage);
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
                                <input type="text" value={currentUser.username} onChange={(e) => { setUser({ ...currentUser, username: e.target.value }) }} />
                                :
                                <figcaption>{currentUser.username}</figcaption>
                        }

                    </figure>

                    {
                        isEditing ?
                            <div className="flex flex-col gap-2 **:cursor-pointer **:bg-white **:text-black **:px-3 **:py-1 **:rounded-full">
                                <button onClick={() => updateUsername(currentUser.username)}>Confirm</button>
                                <br />
                                <button onClick={() => { setIsEditing(false), setUser({ ...currentUser, username: preEditUsername.current }) }}>Cancel</button>
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