import LoginForm from "../Modules/LoginForm";
import RegisterForm from "../Modules/RegisterForm";

export default function Login() {
    return (
        <div className="flex flex-col justify-center items-center w-screen h-screen gap-7">
            <h1 className="">my app</h1>
            <br />
            <section className="flex justify-center gap-5">
                <div>
                    <h1>Login</h1>
                    <LoginForm />
                </div>

                <div>
                    <p>Register</p>
                    <RegisterForm />
                </div>

            </section>
        </div>
    );
}