import { useState } from "react"

export default function RegisterForm({ setErrorMessage }) {
    const apiUrl = import.meta.env.VITE_API_URL;

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function handleRegister(e) {
        e.preventDefault();

        const response = await fetch(`${apiUrl}/auth/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            const errorData = await response.json();
            const errorMessage = Object.values(errorData.errors).flat().join(", ");
            //console.error("Registration failed:", errorMessage);
            setErrorMessage(errorMessage || "Registration failed");
        }

        alert("Registration successful!");
    }


    return (
        <form action="" className="flex flex-col gap-2" onSubmit={handleRegister}>
            <input type="text" name="username" placeholder="Username" onChange={(e) => setUsername(e.target.value)} className="border-none rounded-sm bg-white p-0.5" />
            <br />
            <input type="password" name="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} className="border-none rounded-sm bg-white p-0.5" />
            <br />

            <input type="submit" className="bg-white py-1 px-2 w-[50%] self-center rounded-sm" value="Register" />
        </form>
    )
}