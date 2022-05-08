import axios, { AxiosRequestConfig } from "axios";
import { useState } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
  NavLink
} from "react-router-dom";
import Home from "./component/Home";
import Login from "./component/Login";

export default function App(): JSX.Element {
  axios.defaults.baseURL = process.env.REACT_APP_BACKEND;
  axios.interceptors.request.use((config: AxiosRequestConfig<any>) => {
    const token = localStorage.getItem("token");

    if (!config.headers)
      config.headers = {};

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config
  }, (error) => Promise.reject(error));

  const [mobileNavBarVisible, setMobileNavBarVisible] = useState(false)

  function displayMobileNavBar() {
    setMobileNavBarVisible(!mobileNavBarVisible)
  }


  const activeClassName = "block py-2 pr-4 pl-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 dark:text-white"
  const inactiveClassName = "block py-2 pr-4 pl-3 text-gray-700 border-b border-gray-100 hover:bg-gray-50 md:hover:bg-transparent md:border-0 md:hover:text-blue-700 md:p-0 dark:text-gray-400 md:dark:hover:text-white dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"

  return (
    <Router>
      <div className="w-full">
        <nav className="bg-white border-gray-200 px-2 sm:px-4 py-2.5 dark:bg-gray-700">
          <div className="container flex flex-wrap justify-between items-center mx-auto">
            <Link to="/" className="flex items-center">
              <img src="/favicon.ico" className="mr-3 h-6 sm:h-9" alt="App logo" />
              <span className="self-center text-xl font-semibold whitespace-nowrap dark:text-white">KeyCard TimeSheet WebPanel</span>
            </Link>
            <button onClick={() => displayMobileNavBar()} type="button" className="inline-flex items-center p-2 ml-3 text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600" aria-controls="mobile-menu" aria-expanded="false">
            <span className="sr-only">Open navigation menu</span>
              <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fillRule="evenodd" d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z" clipRule="evenodd"></path></svg>
              <svg className="hidden w-6 h-6" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fillRule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clipRule="evenodd"></path></svg>
            </button>
            <div className={"w-full md:block md:w-auto " + (mobileNavBarVisible ? 'block' : 'hidden')} id="mobile-menu">
              <ul className="flex flex-col mt-4 md:flex-row md:space-x-8 md:mt-0 md:text-sm md:font-medium">
                <li>
                  <NavLink to="/" className={({ isActive }) => isActive ? activeClassName : inactiveClassName }>Home</NavLink>
                </li>
                <li>
                  <NavLink to="/login" className={({ isActive }) => isActive ? activeClassName : inactiveClassName }>Login</NavLink>
                </li>
              </ul>
            </div>
          </div>
        </nav>

        <div className="rendered-component-container mt-6 rounded-md sm:w-2/3 mx-auto">
          <Routes>
            <Route path="/" element={<Home/>}></Route>
            <Route path="/login" element={<Login/>} ></Route>
          </Routes>
        </div>
      </div>
    </Router>
  );
}
