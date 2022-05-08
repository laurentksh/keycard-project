import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthGrantViewModel, LoginDto } from "../models/AuthModels";


const Login = (): JSX.Element => {
    const navigate = useNavigate()
    
    let email = "", password = "";
    const [error, setError] = useState("");

    const login = async (): Promise<AuthGrantViewModel> => {
        const response = await axios.post("api/v1/Auth", {
            email: email,
            password: password
        } as LoginDto);

        return response.data;
    }

    const doLogin = async () => {
        let authGrant = {} as AuthGrantViewModel;

        try {
            authGrant = await login();
        } catch (error) { console.error(error)}

        if (authGrant.token) {
            localStorage.setItem("token", authGrant.token);

            navigate("/");
        } else {
            setError("Invalid email or password");
        }
    }

    const doLogout = async () => {
        try {
            await axios.post("/api/v1/Auth/logout");
        } catch (err) { console.error(err) }

        localStorage.removeItem("token");

        navigate("/")
    }

    if (localStorage.getItem("token")) {
        return <div>
            <h1 className="text-lg mb-6">You are already logged in</h1>

            <button onClick={() => doLogout()} className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 focus:outline-none" type="button">
                Log out
            </button>
        </div>
    }

    return (
        <form onSubmit={(e) => { e.preventDefault(); doLogin()}} className="mx-auto xl:w-1/2 mt-40 bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 flex flex-col">
            <h1 className="mb-6 text-gray-500 text-lg">Employee SSO</h1>
            <div className="mb-4">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="username">
                    Email
                </label>
                <input onChange={(e) => email = e.target.value/*setEmail(e.target.value)*/} className="shadow appearance-none border rounded w-full py-2 px-3 text-grey-darker focus:shadow-md focus:outline-none" id="username" type="email" placeholder="Email" autoComplete="email"/>
            </div>
            <div className="mb-6">
                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="password">
                    Password
                </label>
                <input onChange={(e) => password = e.target.value/*setPassword(e.target.value)*/} className="shadow appearance-none border border-red rounded w-full py-2 px-3 text-grey-darker mb-3 focus:shadow-md focus:outline-none" id="password" type="password" placeholder="********" autoComplete="current-password"/>
                <p style={{ display: error ? 'block' : 'none'}} className="text-red-500 text-xs italic">{error}</p>
            </div>
            <div className="flex items-center justify-between">
                <button className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 focus:outline-none" type="submit">
                    Sign In
                </button>
            </div>
        </form>
    );
}

export default Login;