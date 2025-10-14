import { Navigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { useEffect } from "react";

// Setup for checking if user is valid, and can access /content
export default function ProtectedRoute({ children }) {
    const apiUrl = import.meta.env.VITE_API_URL;
    const token = localStorage.getItem('token');

    // exp is the expiration 5 minutes from now
    let decodedToken = token ? jwtDecode(token) : null;
    const exp = decodedToken?.exp;

    async function checkToken() {
        // of no token or token is expired
        if (!token) {
            return <Navigate to="/" replace />;
        }

        if (Date.now() < exp * 1000) {
            // refresh token logic
            try {
                const response = await fetch(`${apiUrl}/api/auth/refresh`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        accessToken: token,
                    }),
                    credentials: 'include' // Include cookies in the request
                });

                if (response.ok) {
                    const data = await response.json();
                    localStorage.setItem('token', data.accessToken);
                } else {
                    console.error("Failed to refresh token");
                }
            } catch (error) {
                console.error("Error refreshing token:", error);
            }
        }
    }

    useEffect(() => {
        checkToken();
    }, [token]);

    return children;
}
