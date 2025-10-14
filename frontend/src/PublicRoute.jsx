import { useState, useEffect } from "react";
import { Navigate } from "react-router-dom";

export default function PublicRoute({ children }) {
    const [loading, setLoading] = useState(true);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const tryRefresh = async () => {
            try {
                const res = await fetch("https://localhost:5001/api/auth/refresh", {
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
                    setIsLoggedIn(false);
                }
            } catch (err) {
                setIsLoggedIn(false);
            } finally {
                setLoading(false);
            }
        };

        tryRefresh();
    }, []);

    if (loading) return <div>Checking session...</div>;

    if (isLoggedIn) return <Navigate to="/content" replace />;

    return children;
}
