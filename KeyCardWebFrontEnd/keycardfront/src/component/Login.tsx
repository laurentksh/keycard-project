import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { AuthGrantViewModel, LoginDto } from "../models/AuthModels";


const Login = (): JSX.Element => {
    const navigate = useNavigate()
    
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
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
        } catch (error) { }

        if (authGrant.token) {
            localStorage.setItem("token", authGrant.token);

            navigate("/");
        } else {
            setError("Invalid email or password");
        }
    }

    return (
        <div className="ml-40 mr-40 bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 flex flex-col">
            <div className="mb-4">
                <label className="block text-grey-darker text-sm font-bold mb-2" htmlFor="username">
                    Username
                </label>
                <input onChange={(e) => setEmail(e.target.value)} className="shadow appearance-none border rounded w-full py-2 px-3 text-grey-darker" id="username" type="text" placeholder="Username"/>
            </div>
            <div className="mb-6">
                <label className="block text-grey-darker text-sm font-bold mb-2" htmlFor="password">
                    Password
                </label>
                <input onChange={(e) => setPassword(e.target.value)} className="shadow appearance-none border border-red rounded w-full py-2 px-3 text-grey-darker mb-3" id="password" type="password" placeholder="******************"/>
                <p style={{ display: error ? 'block' : 'none'}} className="text-red-500 text-xs italic">{error}</p>
            </div>
            <div className="flex items-center justify-between">
                <button onClick={() => doLogin()} className="bg-blue hover:bg-blue-dark text-black font-bold py-2 px-4 rounded" type="button">
                    Sign In
                </button>
            </div>
        </div>
    );
}

export default Login;