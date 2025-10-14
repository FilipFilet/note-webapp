import LoginForm from "../Modules/LoginForm";
import RegisterForm from "../Modules/RegisterForm";
import { useState } from "react";

export default function Login() {
    const [errorMessage, setErrorMessage] = useState("");

    return (
        <div className="flex flex-col justify-center items-center w-screen h-screen gap-7 bg-[#0f0f0f]">
            <h1 className="text-white font-bold text-4xl">Notes</h1>
            <br />
            <section className="flex flex-col justify-center gap-10 bg-[#161616] rounded-md p-7 shadow-[0_2px_30px_0_rgba(0,0,0)] [&_p]:text-white [&_p]:text-center [&_div]:flex [&_div]:flex-col [&_div]:gap-2">
                <section className="flex flex-row gap-10">
                    <div>
                        <p>Login</p>
                        <LoginForm setErrorMessage={setErrorMessage} />
                    </div>

                    <div>
                        <p>Register</p>
                        <RegisterForm setErrorMessage={setErrorMessage} />
                    </div>
                </section>

                <div>
                    {errorMessage && <p className="text-red-500 text-center">{errorMessage}</p>}
                </div>

            </section>
        </div>
    );
}