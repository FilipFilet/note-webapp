import { useState } from "react";
import { useNavigate } from "react-router";

export default function UserModal({ currentUser, onClose }) {
    const [user, setUser] = useState(currentUser);
    const Navigate = useNavigate();

    function logout() {
        localStorage.removeItem("token");
        Navigate("/"); // Redirect to the home page after logout
    }


    return (
        <>
            <div className="absolute left-0 top-0 w-screen h-screen bg-black opacity-50"></div>
            <section className="absolute left-[50%] top-[50%] translate-x-[-50%] translate-y-[-50%] bg-[#161616] p-5 rounded-lg shadow-lg text-white w-100">
                <h1 className="text-center">Profile</h1>

                <div className="flex justify-between">
                    <figure className="flex items-center gap-3">
                        <img src="https://placehold.co/100x100" alt="Profile Image" className="rounded-full" />
                        <figcaption>{user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}</figcaption>
                    </figure>

                    <div className="flex flex-col gap-3 **:cursor-pointer **:bg-white **:text-black **:px-3 **:py-1 **:rounded-full">
                        <button>Edit Username</button>
                        <br />
                        <button>Edit Picture</button>
                    </div>
                </div>

                <div className="flex justify-center gap-3">
                    <button onClick={onClose} className="cursor-pointer bg-white text-black px-3 py-1 rounded-full">Close</button>
                    <button onClick={logout} className="cursor-pointer bg-white text-black px-3 py-1 rounded-full">Logout</button>
                </div>
            </section>
        </>
    );
}