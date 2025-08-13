import { Navigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

// Setup for checking if user is valid, and can access /content
export default function ProtectedRoute({ children }) {
    const token = localStorage.getItem('token');

    // exp is the expiration 5 minutes from now
    let decodedToken = token ? jwtDecode(token) : null;
    const exp = decodedToken?.exp;

    // of no token or token is expired
    if (!token || Date.now() >= exp * 1000) {
        localStorage.removeItem('token');

        return <Navigate to="/" replace />;
    }

    return children;
}
