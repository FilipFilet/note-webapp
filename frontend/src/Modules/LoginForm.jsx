import { useState } from 'react';
import { useNavigate } from 'react-router';

export default function LoginForm()
{
    const [error, setError] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    async function handleSubmit(event) {
        event.preventDefault();

        try
        {
            let response = await fetch(`${import.meta.env.VITE_API_URL}/auth/login`, {
                method: 'POST',
                body: JSON.stringify({ username, password}),
                headers: {
                    'Content-Type': 'application/json'
                },
            })

            if (!response.ok)
            {
                let errorMessage = "Login failed";

                if (response.status === 400)
                {
                    const errorData = await response.json();

                    const errors = Object.values(errorData.errors).flat();
                    errorMessage = errors.join(", ");
                }
                else
                {
                    errorMessage = await response.text();
                }

                setError(errorMessage || "Login failed");
                console.error("Login error:", errorMessage);
            }
            else
            {
                const token = await response.text();
                localStorage.setItem('token', token);
                navigate('/Content');
            }

        }
        catch (e) {
            setError(e.message);
            console.error(e);
            return;
        }
    }

    return (
        <form action="" className="login-form" onSubmit={handleSubmit}>
            <input type="text" placeholder="Username" className="border-2 border-gray-900" onChange={(e) => setUsername(e.target.value)} />
            <br />
            <input type="password" placeholder="Password" className="border-2 border-gray-900" onChange={(e) => setPassword(e.target.value)} />
            <br />
            <input type="submit" value="Login" className="bg-gray-900 text-white py-1 px-2" />
        </form>
    )
}