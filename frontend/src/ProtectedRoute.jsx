import { Navigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { useEffect, useState } from "react";

// Setup for checking if user is valid, and can access /content
export default function ProtectedRoute({ children }) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const token = localStorage.getItem('token');
    const { isValid, setIsValid } = useState(false);

    // exp is the expiration 5 minutes from now
    let decodedToken = token ? jwtDecode(token) : null;
    const exp = decodedToken?.exp;

    async function checkToken() {
        if (!token) {
            setIsValid(false);
            return;
        }

        if (Date.now() < exp * 1000) {
            // refresh token logic
            try {
                const response = await fetch(`${apiUrl}/token/refresh`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    credentials: 'include', // Include cookies in the request
                    body: JSON.stringify({
                        accessToken: token
                    }),
                });

                debugger;
                if (response.ok) {
                    const data = await response.json();
                    localStorage.setItem('token', data.accessToken);
                } else {
                    const errorData = await response.text();
                    console.error("Failed to refresh token:", errorData);
                    return;
                }
            } catch (error) {
                console.error("Error refreshing token:", error);
            }
        }
    }

    useEffect(() => {
        checkToken();

    }, [token]);

    if (!isValid) {
        return <Navigate to="/" replace />;
    }

    return children;
}
