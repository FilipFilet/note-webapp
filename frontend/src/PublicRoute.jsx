import { useState, useEffect } from "react";
import { Navigate } from "react-router-dom";

export default function PublicRoute({ children }) {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const apiUrl = import.meta.env.VITE_API_URL;

    useEffect(() => {
        debugger;
        const tryRefresh = async () => {
            try {
                const res = await fetch(`${apiUrl}/token/refresh`, {
                    method: "POST",
                    credentials: "include", // sends HttpOnly cookie automatically
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ accessToken: localStorage.getItem("token") || "" })
                });

                if (res.ok) {
                    const data = await res.json();
                    localStorage.setItem("token", data.accessToken);
                    setIsLoggedIn(true); // valid refresh token
                } else {
                    const errorData = await res.json();
                    console.error("Failed to refresh token:", errorData);
                    setIsLoggedIn(false);
                }
            } catch (err) {
                setIsLoggedIn(false);
            }

            tryRefresh();
        }

    }, []);

    if (isLoggedIn) return <Navigate to="/content" replace />;

    return children;
}
