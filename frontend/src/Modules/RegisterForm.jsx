import { useState } from "react"

export default function RegisterForm() {
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
            console.error("Registration failed:", errorMessage);
        }
    }


    return (
        <form action="" onSubmit={handleRegister}>
            <input type="text" name="username" placeholder="Username" onChange={(e) => setUsername(e.target.value)} />
            <br />
            <input type="password" name="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} />
            <br />

            <input type="submit" value="Register" />
        </form>
    )
}