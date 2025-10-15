import Header from '../Modules/Header.jsx';
import Editor from '../Modules/Editor.jsx';
import Sidebar from '../Modules/Sidebar.jsx';
import { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { Navigate, useNavigate } from 'react-router-dom';

export default function ContentPage() {
    const [selectedNote, setSelectedNote] = useState({});
    const [updatedNoteData, setUpdatedNoteData] = useState({});
    const navigate = useNavigate();

    const apiUrl = import.meta.env.VITE_API_URL;
    const accessToken = localStorage.getItem('accessToken')

    // exp is the expiration 5 minutes from now
    let decodedaccessToken = accessToken ? jwtDecode(accessToken) : null;
    const exp = decodedaccessToken?.exp;

    async function checkToken() {
        // If access token does not exist, redirect to login page
        if (!accessToken) {
            navigate('/', { replace: true });
            return;
        }


        if (Date.now() >= exp * 1000) {
            try {
                const response = await fetch(`${apiUrl}/token/refresh`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        accessToken: accessToken,
                        refreshToken: localStorage.getItem('refreshToken')
                    })

                });

                if (response.ok) {
                    const data = await response.json();
                    localStorage.setItem('accessToken', data.accessToken);
                    localStorage.setItem('refreshToken', data.refreshToken);

                    console.log("Token is valid, user is authenticated");
                    console.log(`New accessToken: ${data.accessToken} \n New refreshToken: ${data.refreshToken}`);
                    return;
                }
                else {
                    const errorData = await response.text();
                    console.error("Failed to refresh token (refresh token invalid):", errorData);
                    navigate('/', { replace: true });
                    return;
                }

            }
            catch (error) {
                console.error("Error refreshing token:", error);
                return;
            }
        }
    }

    useEffect(() => {
        checkToken();
    }, [accessToken]) // Run checkToken when accessToken changes, e.g., after login or auto-login

    // async function checkToken() {
    //     if (!accessToken) {
    //         setIsValid(false);
    //         return;
    //     }

    //     if (Date.now() >= exp * 1000 || Date.now() < exp * 1000) {
    //         // refresh accessToken logic
    //         try {
    //             const response = await fetch(`${apiUrl}/token/refresh`, {
    //                 method: "POST",
    //                 headers: {
    //                     "Content-Type": "application/json",
    //                     "Authorization": `Bearer ${accessToken}`
    //                 },
    //                 body: JSON.stringify({
    //                     accesstoken: accessToken,
    //                     refreshtoken: localStorage.getItem('refreshToken')

    //                 }),
    //             });

    //             if (response.ok) {
    //                 const data = await response.json();
    //                 localStorage.setItem('accessToken', data.accessToken);
    //                 localStorage.setItem('refreshToken', data.refreshToken);
    //             } else {
    //                 const errorData = await response.text();
    //                 console.error(`Failed to refresh token: ${errorData} \n localStorage accessToken: ${localStorage.getItem('accessToken')} \n localStorage refreshToken: ${localStorage.getItem('refreshToken')}`);
    //                 return;
    //             }
    //         } catch (error) {
    //             console.error("Error refreshing access token:", error);
    //         }
    //     }
    // }

    // useEffect(() => {
    //     checkToken();
    // }, [accessToken]);

    // if (!isValid) {
    //     return <Navigate to="/" replace />;
    // }

    return (
        <>
            <div className="grid grid-cols-[1fr_4fr] grid-rows-[auto_1fr] h-screen">
                <Header />
                <Sidebar updatedNoteData={updatedNoteData} setSelectedNote={setSelectedNote} />
                <Editor selectedNote={selectedNote} setUpdatedNoteData={setUpdatedNoteData} />
            </div>

        </>
    )
}